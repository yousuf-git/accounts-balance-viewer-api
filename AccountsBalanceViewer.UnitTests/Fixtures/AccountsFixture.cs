using AccountsViewer.API.Models.DTOs;

namespace AccountsBalanceViewer.UnitTests.Fixtures;

public static class AccountsFixture
{
    public static List<AccountDTO> GetTestAccountDTOs() => new()
    {
        new AccountDTO
        {
            Id = 911,
            Name = "R&D",
            Balance = 12_000.00f
        },
        new AccountDTO
        {
            Id = 912,
            Name = "Marketing",
            Balance = 15_500.50f
        },
        new AccountDTO
        {
            Id = 913,
            Name = "Operations",
            Balance = 8_000.25f
        }
    };
}