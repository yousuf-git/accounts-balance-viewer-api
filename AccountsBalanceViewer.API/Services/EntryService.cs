using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories.Interfaces;
using AccountsViewer.API.Services.Interfaces;

namespace AccountsViewer.API.Services;

public class EntryService : IEntryService
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _uow;

    public EntryService(IAuthService authService, IUnitOfWork uow)
    {
        _authService = authService;
        _uow = uow;
    }

    public async Task AddEntries(IEnumerable<Entry> entries)
    {
        var currentUser = _authService.GetAuthUser();
        if (currentUser == null)
        {
            throw new Exception("Unauthorized");
        }

        var entriesArr = entries as Entry[] ?? entries.ToArray();
        entriesArr.ToList().ForEach(e => e.CreatedBy = currentUser.Id);

        await _uow.EntryRepository.AddRange(entriesArr);
        await _uow.Commit();
    }
}