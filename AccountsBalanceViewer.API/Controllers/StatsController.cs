using System.ComponentModel.DataAnnotations;
using AccountsViewer.API.Models.Constants;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Reporting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsReporter _statsReporter;

    public StatsController(IStatsReporter statsReporter)
    {
        _statsReporter = statsReporter;
    }

    [HttpGet("balance-change-by-months")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<List<BalanceChangeByMonthsDTO>>> FindBalanceChangeByMonths([Required] int year)
    {
        var data = await _statsReporter.FindBalanceChangeByMonths(year);

        return Ok(data);
    }

    [HttpGet("balance-change-by-years")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<BalanceChangeByYearsDTO>> FindBalanceChangeByYears()
    {
        var data = await _statsReporter.FindBalanceChangeByYears();

        return Ok(data);
    }

    [HttpGet("balance-by-months")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<BalanceByMonthsDTO>> FindBalanceByMonths([Required] int year)
    {
        var data = await _statsReporter.FindBalanceByMonths(year);

        return Ok(data);
    }

    [HttpGet("balance-by-years")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<BalanceByYearsDTO>> FindBalanceByYears()
    {
        var data = await _statsReporter.FindBalanceByYears();

        return Ok(data);
    }

    [HttpGet("first-operation-year")]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult FindFirstOperationYear()
    {
        var year = _statsReporter.FindFirstOperationYear();

        return Ok(new
        {
            Year = year
        });
    }
}