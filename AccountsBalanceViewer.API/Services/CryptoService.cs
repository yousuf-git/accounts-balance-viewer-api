namespace AccountsViewer.API.Services;

public interface ICryptoService
{
    string Hash(string password);
    bool Verify(string text, string hash);
}

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