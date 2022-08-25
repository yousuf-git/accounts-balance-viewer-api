using System.Security.Claims;
using AccountsViewer.API.Models.Requests;

namespace AccountsBalanceViewer.UnitTests.Fixtures;

public static class AuthFixture
{
    public static SignUpRequest GetTestSignUpRequest() => new()
    {
        Username = "admin",
        Password = "123",
        Email = "johndoe@example.com",
        Name = "John Doe"
    };

    public static Claim[] GetTestClaims() => new[]
    {
        new Claim(ClaimTypes.NameIdentifier, "44"),
        new Claim(ClaimTypes.Authentication, "max44"),
        new Claim(ClaimTypes.Email, "max@example.com"),
        new Claim(ClaimTypes.Name, "Max Verstappen"),
        new Claim(ClaimTypes.Role, "user")
    };
}