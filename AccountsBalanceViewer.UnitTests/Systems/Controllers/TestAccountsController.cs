using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Controllers;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Services;
using Castle.Core.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Controllers;

public class TestAccountsController
{
    [Fact]
    public async Task GetAccounts_OnSuccess_ReturnsStatusCode200()
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService
            .Setup(service =>
                service.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(AccountsFixture.GetTestAccountDTOs());
        
        var sut = new AccountsController(mockAccountService.Object);

        var result = (await sut.GetAccounts(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Result as OkObjectResult;

        result!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetAccounts_OnSuccess_ReturnsListOfAccounts()
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService
            .Setup(service =>
                service.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(AccountsFixture.GetTestAccountDTOs());
        
        var sut = new AccountsController(mockAccountService.Object);

        var result = (await sut.GetAccounts(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Result;
        result.Should().BeOfType<OkObjectResult>();

        var accounts = (result as OkObjectResult)?.Value;
        accounts.Should().BeOfType<List<AccountDTO>>();
    }

    [Fact]
    public async Task GetAccounts_WhenCalled_InvokesAccountServiceExactlyOnce()
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService
            .Setup(service =>
                service.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(AccountsFixture.GetTestAccountDTOs());
        
        var sut = new AccountsController(mockAccountService.Object);

        var result = await sut.GetAccounts(It.IsAny<DateTime>(), It.IsAny<DateTime>());

        mockAccountService.Verify(
            service => service.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAccounts_WhenCalled_ParsesDateTimeIntoDateOnly()
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService
            .Setup(service =>
                service.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(AccountsFixture.GetTestAccountDTOs());
        
        var sut = new AccountsController(mockAccountService.Object);

        var balanceFrom = DateTime.Now;
        var balanceTo = DateTime.Now;

        var result = await sut.GetAccounts(balanceFrom, balanceTo);

        mockAccountService.Verify(
            service =>
                service.GetAccountsWithBalancesWithinRange(
                    DateOnly.FromDateTime(balanceFrom),
                    DateOnly.FromDateTime(balanceTo)
                ),
            Times.Once);
    }
}