using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories.Interfaces;
using AccountsViewer.API.Services;
using AccountsViewer.API.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Services;

public class TestEntryService
{
    [Fact]
    public async Task AddEntries_WhenUnauthorized_ReturnsException()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.GetAuthUser());

        var mockEntryRepo = new Mock<IEntryRepository>();
        mockEntryRepo.Setup(repository => repository.AddRange(It.IsAny<IEnumerable<Entry>>()));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.EntryRepository)
            .Returns(mockEntryRepo.Object);

        var sut = new EntryService(mockAuthService.Object, mockUow.Object);

        var act = () => sut.AddEntries(new List<Entry>());

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task AddEntries_WhenCalled_InvokesAuthServiceExactlyOnce()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(UsersFixture.GetTestUserDTO());

        var mockEntryRepo = new Mock<IEntryRepository>();
        mockEntryRepo.Setup(repository => repository.AddRange(It.IsAny<IEnumerable<Entry>>()));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.EntryRepository)
            .Returns(mockEntryRepo.Object);

        var sut = new EntryService(mockAuthService.Object, mockUow.Object);

        await sut.AddEntries(new List<Entry>());

        mockAuthService.Verify(service => service.GetAuthUser(), Times.Once);
    }

    [Fact]
    public async Task AddEntries_WhenCalled_InvokesEntryRepositoryExactlyOnce()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(UsersFixture.GetTestUserDTO());

        var mockEntryRepo = new Mock<IEntryRepository>();
        mockEntryRepo.Setup(repository => repository.AddRange(It.IsAny<IEnumerable<Entry>>()));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.EntryRepository)
            .Returns(mockEntryRepo.Object);

        var sut = new EntryService(mockAuthService.Object, mockUow.Object);

        await sut.AddEntries(EntriesFixture.GetTestEntries());

        mockEntryRepo.Verify(repository => repository.AddRange(It.IsAny<IEnumerable<Entry>>()), Times.Once);
    }

    [Fact]
    public async Task AddEntries_WhenCalled_InvokesEntryRepositoryWithExpectedSizeOfEntries()
    {
        var entries = EntriesFixture.GetTestEntries();

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService
            .Setup(service => service.GetAuthUser())
            .Returns(UsersFixture.GetTestUserDTO());

        var mockEntryRepo = new Mock<IEntryRepository>();
        mockEntryRepo.Setup(repository => repository.AddRange(It.IsAny<IEnumerable<Entry>>()));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow
            .Setup(uow => uow.EntryRepository)
            .Returns(mockEntryRepo.Object);

        var sut = new EntryService(mockAuthService.Object, mockUow.Object);

        await sut.AddEntries(entries);

        mockEntryRepo.Verify(
            repository => repository.AddRange(It.Is<Entry[]>(list => list.Length == entries.Count)),
            Times.Once);
    }
}