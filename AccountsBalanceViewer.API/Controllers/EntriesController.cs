using AccountsViewer.API.Models.Constants;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EntriesController : ControllerBase
{
    private readonly IEntryService _entryService;

    public EntriesController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<AddAccountResponse>> AddEntries(List<AddEntryRequest> request)
    {
        if (request.Count == 0)
        {
            return BadRequest();
        }

        var entries = request.Select(e =>
            new Entry
            {
                AccountId = e.AccountId,
                Amount = e.Amount,
                Date = e.Date
            }
        );

        await _entryService.AddEntries(entries);

        return NoContent();
    }
}