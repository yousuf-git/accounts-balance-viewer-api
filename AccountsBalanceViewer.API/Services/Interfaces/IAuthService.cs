using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Services.Interfaces;

public interface IAuthService
{
    (UserDTO? userDto, string token, DateTime expiresAt) Auth(string username, string password);
    Task UserSignUp(User user);
    UserDTO? GetAuthUser();
}