using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Controllers;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Controllers;

public class TestAuthController
{
    [Fact]
    public void Auth_OnSuccess_Returns200StatusCode()
    {
        var request = new AuthRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.Auth(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UsersFixture.GetTestUserDTO(), "bcrypt_token", DateTime.Now));

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.Auth(request).Result as OkObjectResult;

        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public void Auth_OnUserNotFound_Returns404StatusCode()
    {
        var request = new AuthRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.Auth(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((null, "", DateTime.MinValue));

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.Auth(request).Result as NotFoundObjectResult;

        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public void Auth_OnSuccess_ReturnsAuthResponse()
    {
        var request = new AuthRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.Auth(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UsersFixture.GetTestUserDTO(), "bcrypt_token", DateTime.Now));

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.Auth(request).Result;
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeOfType<AuthResponse>();
    }

    [Fact]
    public void Auth_WhenCalled_InvokesAuthServiceExactlyOnce()
    {
        var request = new AuthRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.Auth(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UsersFixture.GetTestUserDTO(), "bcrypt_token", DateTime.Now));

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.Auth(request);

        mockAuthService.Verify(
            service => service.Auth(It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Auth_WhenCalled_InvokesAuthServiceWithExpectedArgs()
    {
        var request = new AuthRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.Auth(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UsersFixture.GetTestUserDTO(), "bcrypt_token", DateTime.Now));

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.Auth(request);

        mockAuthService.Verify(
            service => service.Auth(request.Username, request.Password),
            Times.Once);
    }

    [Fact]
    public async Task SignUp_OnSuccess_Returns200StatusCode()
    {
        var request = new SignUpRequest { Username = "admin", Password = "123" };

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.UserSignUp(It.IsAny<User>()));

        var sut = new AuthController(mockAuthService.Object);

        var result = (await sut.SignUp(request)).Result as OkObjectResult;

        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task SignUp_OnSuccess_ReturnsAuthResponse()
    {
        var request = AuthFixture.GetTestSignUpRequest();

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.UserSignUp(It.IsAny<User>()));

        var sut = new AuthController(mockAuthService.Object);

        var result = (await sut.SignUp(request)).Result;
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeOfType<SignUpResponse>();
    }

    [Fact]
    public async Task SignUp_WhenCalled_InvokesAuthServiceExactlyOnce()
    {
        var request = AuthFixture.GetTestSignUpRequest();

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.UserSignUp(It.IsAny<User>()));

        var sut = new AuthController(mockAuthService.Object);

        var result = await sut.SignUp(request);

        mockAuthService.Verify(service => service.UserSignUp(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_WhenCalled_InvokesAuthServiceWithExpectedUserDetails()
    {
        var request = AuthFixture.GetTestSignUpRequest();

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.UserSignUp(It.IsAny<User>()));

        var sut = new AuthController(mockAuthService.Object);

        var result = await sut.SignUp(request);

        mockAuthService.Verify(service =>
                service.UserSignUp(
                    It.Is<User>(user =>
                        user.Name == request.Name &&
                        user.Email == request.Email &&
                        user.Username == request.Username &&
                        user.Password == request.Password)),
            Times.Once);
    }

    [Fact]
    public void GetUser_OnSuccess_Returns200StatusCode()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(UsersFixture.GetTestUserDTO());

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.GetUser().Result as OkObjectResult;

        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public void GetUser_OnUserNotFound_Returns401StatusCode()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(() => null);

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.GetUser().Result as UnauthorizedResult;

        result.StatusCode.Should().Be(401);
    }

    [Fact]
    public void GetUser_OnSuccess_ReturnsAuthUser()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(UsersFixture.GetTestUserDTO());

        var sut = new AuthController(mockAuthService.Object);

        var result = sut.GetUser().Result;
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeOfType<UserDTO>();
    }
}