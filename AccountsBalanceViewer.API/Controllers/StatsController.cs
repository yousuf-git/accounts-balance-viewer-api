using System.ComponentModel.DataAnnotations;
using AccountsViewer.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("balance-change-by-months")]
    public async Task<IActionResult> FindBalanceChangeByMonths([Required] int year)
    {
        var data = await _statsService.FindBalanceChangeByMonths(year);
        return Ok(data);
    }

    [HttpGet("balance-change-by-years")]
    public async Task<IActionResult> FindBalanceChangeByYears()
    {
        var data = await _statsService.FindBalanceChangeByYears();
        return Ok(data);
    }

    [HttpGet("balance-by-months")]
    public async Task<IActionResult> FindBalanceByMonths([Required] int year)
    {
        var data = await _statsService.FindBalanceByMonths(year);
        return Ok(data);
    }

    [HttpGet("balance-by-years")]
    public async Task<IActionResult> FindBalanceByYears()
    {
        var data = await _statsService.FindBalanceByYears();

        return Ok(data);
    }

    [HttpGet("first-operation-year")]
    public IActionResult FindFirstOperationYear()
    {
        var year = _statsService.FindFirstOperationYear();
        return Ok(new
        {
            Year = year
        });
    }
}