using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;

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

    public static User GetTestUser() =>
        new()
        {
            Id = 44,
            Name = "Max Verstappen",
            Email = "max@example.com",
            Role = "user",
            Username = "max44",
            Password = "max555"
        };

    public static List<User> GetTestUsers() => new()
    {
        new()
        {
            Id = 44,
            Name = "Max Verstappen",
            Email = "max@example.com",
            Role = "user",
            Username = "max44"
        },
        new()
        {
            Id = 33,
            Name = "Lewis Hamilton",
            Email = "lewis@example.com",
            Role = "user",
            Username = "lewis33"
        },
        new()
        {
            Id = 55,
            Name = "Charles Leclerc",
            Email = "charles@example.com",
            Role = "user",
            Username = "leclerc"
        }
    };
}