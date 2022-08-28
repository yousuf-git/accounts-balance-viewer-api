using AccountsBalanceViewer.UnitTests.Fixtures;
using AccountsViewer.API.Controllers;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccountsBalanceViewer.UnitTests.Systems.Controllers;

public class TestEntriesController
{
    [Fact]
    public async Task AddEntries_OnSuccess_Returns204Response()
    {
        var request = EntriesFixture.GetTestAddEntryRequest();

        var mockEntryService = new Mock<IEntryService>();
        mockEntryService
            .Setup(service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()));

        var sut = new EntriesController(mockEntryService.Object);

        var result = (await sut.AddEntries(request)).Result as NoContentResult;

        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task AddEntries_OnEmptyRequest_Returns400Response()
    {
        var mockEntryService = new Mock<IEntryService>();
        mockEntryService
            .Setup(service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()));

        var sut = new EntriesController(mockEntryService.Object);

        var result = (await sut.AddEntries(new List<AddEntryRequest>())).Result as BadRequestResult;

        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task AddEntries_WhenCalledWithValidRequest_InvokesEntryServiceExactlyOnce()
    {
        var request = EntriesFixture.GetTestAddEntryRequest();

        var mockEntryService = new Mock<IEntryService>();
        mockEntryService
            .Setup(service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()));
        var sut = new EntriesController(mockEntryService.Object);

        var result = await sut.AddEntries(request);

        mockEntryService.Verify(
            service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()),
            Times.Once);
    }

    [Fact]
    public async Task AddEntries_WhenCalledWithEmptyRequest_DoesntInvokeEntryService()
    {
        var mockEntryService = new Mock<IEntryService>();
        mockEntryService
            .Setup(service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()));
        var sut = new EntriesController(mockEntryService.Object);

        var result = await sut.AddEntries(new List<AddEntryRequest>());

        mockEntryService.Verify(
            service => service.AddEntries(It.IsAny<IEnumerable<Entry>>()),
            Times.Never);
    }
}