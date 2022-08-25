using AccountsViewer.API.Models.DTOs;

namespace AccountsBalanceViewer.UnitTests.Fixtures;

public static class UsersFixture
{
    public static UserDTO GetTestUserDTO() => new()
    {
        Id = 99,
        Name = "John Doe",
        Email = "johndoe@example.com",
        Role = "admin", 
        Username = "johndoe"
    };
}