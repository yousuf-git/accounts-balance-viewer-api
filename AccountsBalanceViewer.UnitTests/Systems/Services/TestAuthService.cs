using System.Linq.Expressions;
using System.Security.Claims;
using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Config;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories;
using AccountsViewer.API.Repositories.Interfaces;
using AccountsViewer.API.Services;
using AccountsViewer.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Services;

public class TestAuthService
{
    [Fact]
    public void Auth_OnSuccess_ReturnsAuthData()
    {
        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Search(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(UsersFixture.GetTestUsers());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        var (user, token, expiresAt) = sut.Auth("admin", "123");

        user.Should().NotBeNull();
        token.Should().NotBeNull().And.NotBeEmpty();
        expiresAt.Should().BeAfter(DateTime.Now);
    }

    [Fact]
    public void Auth_OnUserNotFound_ReturnsEmptyValues()
    {
        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Search(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(new List<User>());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        var (user, token, expiresAt) = sut.Auth("admin", "123");

        user.Should().BeNull();
        token.Should().BeEmpty();
        expiresAt.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void Auth_WhenPasswordIsInvalid_ReturnsEmptyValues()
    {
        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Search(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(UsersFixture.GetTestUsers());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        var (user, token, expiresAt) = sut.Auth("admin", "123");

        user.Should().BeNull();
        token.Should().BeEmpty();
        expiresAt.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void Auth_WhenJwtKeyIsLessThan128Bits_ThrowsArgumentOutOfRangeException()
    {
        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Search(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(UsersFixture.GetTestUsers());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var config = Options.Create(
            new JwtConfig
            {
                Key = "test_key",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        var act = () => sut.Auth("admin", "123");

        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Auth_WhenCalled_InvokesCryptoServiceExactlyOnce()
    {
        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Search(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(UsersFixture.GetTestUsers());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        var (user, token, expiresAt) = sut.Auth("admin", "123");

        mockCryptoService.Verify(
            service => service.Verify(It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task UserSignUp_OnSuccess_MutatesUserId()
    {
        var user = UsersFixture.GetTestUser();

        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Add(It.IsAny<User>()))
            .Callback<User>(user => user.Id = 1);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        await sut.UserSignUp(user);

        user.Id.Should().Be(1);
    }

    [Fact]
    public async Task UserSignUp_WhenCalled_InvokesCryptoServiceExactlyOnceWithExpectedArgs()
    {
        var user = UsersFixture.GetTestUser();
        var rawPassword = user.Password;

        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Add(It.IsAny<User>()))
            .Callback<User>(user => user.Id = 1);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        await sut.UserSignUp(user);

        mockCryptoService.Verify(service => service.Hash(rawPassword), Times.Once);
    }

    [Fact]
    public async Task UserSignUp_WhenCalled_InvokesUserRepository()
    {
        var user = UsersFixture.GetTestUser();

        var mockUserRepo = new Mock<UserRepository>(null);
        mockUserRepo
            .Setup(repository => repository.Add(It.IsAny<User>()))
            .Callback<User>(user => user.Id = 1);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();
        mockCryptoService
            .Setup(service => service.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var ctx = new HttpContextAccessor();

        var sut = new AuthService(mockUow.Object, ctx, config, mockCryptoService.Object);

        await sut.UserSignUp(user);

        mockUserRepo.Verify(repository => repository.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void GetAuthUser_OnAuthenticated_ReturnsAuthUser()
    {
        var mockUserRepo = new Mock<UserRepository>(null);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var defaultContext = new DefaultHttpContext();
        var identity = new ClaimsIdentity(AuthFixture.GetTestClaims(), "Custom");
        var user = new ClaimsPrincipal(identity);
        defaultContext.User = user;

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor
            .Setup(accessor => accessor.HttpContext)
            .Returns(defaultContext);

        var sut = new AuthService(mockUow.Object, mockHttpContextAccessor.Object, config, mockCryptoService.Object);

        var result = sut.GetAuthUser();

        result.Should().BeOfType<UserDTO>();
    }

    [Fact]
    public void GetAuthUser_OnUnauthenticated_ReturnsNull()
    {
        var mockUserRepo = new Mock<UserRepository>(null);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.UserRepository)
            .Returns(mockUserRepo.Object);

        var mockCryptoService = new Mock<ICryptoService>();

        var config = Options.Create(
            new JwtConfig
            {
                Key = "sDI7OkPjq2dySdtwsxb0LDyAFQSx9af2CVKSvFzieM3aBnkhCs",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpireIn = 30
            });

        var defaultContext = new DefaultHttpContext();
        var identity = new ClaimsIdentity(AuthFixture.GetTestClaims());
        var user = new ClaimsPrincipal(identity);
        defaultContext.User = user;

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor
            .Setup(accessor => accessor.HttpContext)
            .Returns(defaultContext);

        var sut = new AuthService(mockUow.Object, mockHttpContextAccessor.Object, config, mockCryptoService.Object);

        var result = sut.GetAuthUser();

        result.Should().BeNull();
    }
}