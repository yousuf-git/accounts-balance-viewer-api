using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;

namespace AccountsBalanceViewer.UnitTests.Fixtures;

public static class EntriesFixture
{
    public static List<AddEntryRequest> GetTestAddEntryRequest() => new()
    {
        new AddEntryRequest
        {
            AccountId = 1,
            Amount = 10_000f,
        },
        new AddEntryRequest
        {
            AccountId = 33,
            Amount = 21_300f
        },
        new AddEntryRequest
        {
            AccountId = 99,
            Amount = 91_900f
        }
    };

    public static List<Entry> GetTestEntries() => new()
    {
        new Entry()
        {
            AccountId = 1,
            Amount = 10_000f,
            Date = new DateTime(),
            CreatedBy = 2
        },
        new Entry()
        {
            AccountId = 4,
            Amount = 24_500f,
            Date = new DateTime(),
            CreatedBy = 4
        },
        new Entry()
        {
            AccountId = 9,
            Amount = 90_900f,
            Date = new DateTime(),
            CreatedBy = 10
        }
    };
}