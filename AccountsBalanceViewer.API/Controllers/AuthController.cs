using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public ActionResult<AuthResponse> Auth(AuthRequest request)
    {
        var (user, token, expiresAt) = _authService.Auth(request.Username!, request.Password!);

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(new AuthResponse
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Token = token,
            Expires = new DateTimeOffset(expiresAt).ToUnixTimeMilliseconds()
        });
    }

    [HttpPost("sign-up")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
    {
        var user = new User
        {
            Name = request.Name!,
            Email = request.Email!,
            Username = request.Username!,
            Password = request.Password!
        };
        
        await _authService.UserSignUp(user);

        return Ok(new SignUpResponse {
            Name = user.Name,
            Email = user.Email,
            Username = user.Username
        });
    }

    [HttpGet("profile")]
    [Authorize]
    public ActionResult<UserDTO> GetUser()
    {
        var user = _authService.GetAuthUser();
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
}