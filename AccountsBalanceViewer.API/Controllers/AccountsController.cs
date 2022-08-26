using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration? _configuration;

    public AccountsController(IAccountService accountService, IConfiguration? configuration)
    {
        _accountService = accountService;
        if (configuration != null)
        {
            _configuration = configuration;
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<AccountDTO>>> GetAccounts(
        [FromQuery] DateTime? balanceFrom, [FromQuery] DateTime? balanceTo)
    {
        DateOnly from = DateOnly.FromDateTime(balanceFrom ?? DateTime.MinValue);
        DateOnly to = DateOnly.FromDateTime(balanceTo ?? DateTime.Now);

        var accounts = await _accountService.GetAccountsWithBalancesWithinRange(from, to);

        return Ok(accounts);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<AddAccountResponse>> AddAccount(AddAccountRequest request)
    {
        var account = new Account { Name = request.Name! };
        await _accountService.AddAccount(account);

        return Ok(new AddAccountResponse
        {
            Id = account.Id,
            Name = account.Name
        });
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<AddAccountResponse>> UpdateAccount(
        [FromRoute] long id, [FromBody] UpdateAccountRequest request)
    {
        var account = new Account { Name = request.Name! };
        await _accountService.UpdateAccount(id, account);

        return Ok(new UpdateAccountResponse
        {
            Id = account.Id,
            Name = account.Name
        });
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteAccount([FromRoute] long id)
    {
        await _accountService.DeleteAccount(id);
        return Ok();
    }


    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            Jwt = _configuration!.GetSection("Jwt").Get<List<String>>(),
            ConStr = _configuration!.GetSection("ConnectionStrings").Get<List<String>>(),
            App = _configuration!.GetConnectionString("AccountsDB")
        });
    }
}