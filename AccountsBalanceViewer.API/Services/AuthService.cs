using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountsViewer.API.Config;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountsViewer.API.Services;

public interface IAuthService
{
    (UserDTO?, string) Auth(string username, string password);
    Task UserSignUp(User user);
    UserDTO? GetAuthUser();
}

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly JwtConfig _jwtConfig;
    private readonly IHttpContextAccessor _ctx;

    public AuthService(IUnitOfWork uow, IHttpContextAccessor ctx, IOptions<JwtConfig> jwtConfigs)
    {
        _uow = uow;
        _ctx = ctx;
        _jwtConfig = jwtConfigs.Value;
    }

    public (UserDTO?, string) Auth(string username, string password)
    {
        var user = _uow.UserRepository
            .Search(user => user.Username == username)
            .FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return (null, "");
        }

        return (new UserDTO(user), _GenerateToken(user));
    }

    public async Task UserSignUp(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Role = UserRoles.User;
        await _uow.UserRepository.Add(user);
        await _uow.Commit();
    }
    
    public UserDTO? GetAuthUser()
    {
        var identity = _ctx.HttpContext?.User.Identity as ClaimsIdentity;
        if (identity is null || !identity.IsAuthenticated) return null;

        var user = new UserDTO();
        identity.Claims.ToList().ForEach(claim =>
        {
            switch (claim.Type)
            {
                case ClaimTypes.NameIdentifier:
                    user.Id = long.Parse(claim.Value);
                    break;
                case ClaimTypes.Authentication:
                    user.Username = claim.Value;
                    break;
                case ClaimTypes.Email:
                    user.Email = claim.Value;
                    break;
                case ClaimTypes.Name:
                    user.Name = claim.Value;
                    break;
                case ClaimTypes.Role:
                    user.Role = claim.Value;
                    break;
            }
        });

        return user;
    }
    
    private string _GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Authentication, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtConfig.ExpireIn),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    } 
}