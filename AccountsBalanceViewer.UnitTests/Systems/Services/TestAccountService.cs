using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories;
using AccountsViewer.API.Repositories.Interfaces;
using AccountsViewer.API.Services;
using FluentAssertions;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Services;

public class TestAccountService
{
    [Fact]
    public async Task AddAccount_OnSuccess_MutatesAccountId()
    {
        var account = new Account { Name = "R&D" };

        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repo => repo.Add(It.IsAny<Account>()))
            .Callback<Account>(a => a.Id = 1);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        await sut.AddAccount(account);

        account.Id.Should().Be(1);
    }

    [Fact]
    public async Task AddAccount_WhenCalled_InvokesAccountRepository()
    {
        var account = new Account { Name = "R&D" };

        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repo => repo.Add(It.IsAny<Account>()));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        await sut.AddAccount(account);

        mockAccountRepo.Verify(repository => repository.Add(account), Times.Once);
    }

    [Fact]
    public async Task GetAccountsWithBalancesWithinRange_OnSuccess_ReturnsAccountsListOfExpectedSize()
    {
        var accounts = AccountsFixture.GetTestAccountDTOs();

        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repo =>
                repo.FindAllWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(accounts);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        var result = await sut.GetAccountsWithBalancesWithinRange(It.IsAny<DateOnly>(), It.IsAny<DateOnly>());

        result.Count.Should().Be(accounts.Count);
    }

    [Fact]
    public async Task UpdateAccount_WhenCalled_InvokesFindFromAccountRepo()
    {
        var account = new Account { Name = "R&D" };

        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repo =>
                repo.Find(It.IsAny<long>()))
            .ReturnsAsync(new Account());

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        await sut.UpdateAccount(It.IsAny<long>(), account);

        mockAccountRepo.Verify(repository => repository.Find(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAccount_WhenCalled_DeletesExpectedAccount()
    {
        var account = new Account { Id = 100, Name = "R&D" };

        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repository =>
                repository.Find(It.IsAny<long>()))
            .ReturnsAsync(account);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        await sut.DeleteAccount(It.IsAny<long>());

        mockAccountRepo.Verify(repository => repository.Delete(account), Times.Once);
    }

    [Fact]
    public async Task DeleteAccount_WhenAccountDoesntExist_ThrowsException()
    {
        var mockAccountRepo = new Mock<AccountRepository>(null);
        mockAccountRepo
            .Setup(repository =>
                repository.Find(It.IsAny<long>()))
            .ReturnsAsync(() => null);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.AccountRepository)
            .Returns(mockAccountRepo.Object);

        var sut = new AccountService(mockUow.Object);

        var act = () => sut.DeleteAccount(It.IsAny<long>());

        await act.Should().ThrowAsync<Exception>();
    }
}