using AccountsViewer.API.Services.Interfaces;

namespace AccountsViewer.API.Services;

public class CryptoService : ICryptoService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string text, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(text, hash);
    }
}