namespace AccountsViewer.API.Services.Interfaces;

public interface ICryptoService
{
    string Hash(string password);
    bool Verify(string text, string hash);
}