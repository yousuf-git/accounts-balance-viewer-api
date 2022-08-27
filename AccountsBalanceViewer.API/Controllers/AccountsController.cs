using AccountsViewer.API.Models.Constants;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<AccountDTO>>> GetAccounts(
        [FromQuery] DateTime? balanceFrom, [FromQuery] DateTime? balanceTo)
    {
        var from = DateOnly.FromDateTime(balanceFrom ?? DateTime.MinValue);
        var to = DateOnly.FromDateTime(balanceTo ?? DateTime.Now);

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
}