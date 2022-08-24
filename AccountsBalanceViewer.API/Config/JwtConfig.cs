namespace AccountsViewer.API.Config;

public class JwtConfig
{
    public const string Property = "Jwt";

    public string Key { get; set; } = "";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int ExpireIn { get; set; } // minutes
}