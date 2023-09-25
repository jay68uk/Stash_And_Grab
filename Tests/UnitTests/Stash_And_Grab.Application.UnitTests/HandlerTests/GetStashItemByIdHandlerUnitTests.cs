using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Stash_And_Grab.Application.DtoModels;
using Stash_And_Grab.Application.Handlers;
using Stash_And_Grab.Application.Interfaces;
using Stash_And_Grab.Application.Logging;
using Stash_And_Grab.Application.Queries;
using Stash_And_Grab.Application.ResponseModels;

namespace Stash_And_Grab.Application.UnitTests.HandlerTests;

public class GetStashItemByIdHandlerUnitTests
{
    private readonly IDataHandler _dataHandler = Substitute.For<IDataHandler>();

    private readonly ILoggerAdaptor<GetStashItemByIdHandler> _logger =
        Substitute.For<ILoggerAdaptor<GetStashItemByIdHandler>>();

    private readonly GetStashItemByIdHandler _sut;

    public GetStashItemByIdHandlerUnitTests()
    {
        _sut = new GetStashItemByIdHandler(_dataHandler, _logger);
    }

    [Fact]
    public async Task GetStashItemByIdHandler_ShouldReturnResponseStashItemModel_WhenStashIdIsFound()
    {
        //Arrange
        var expectedValue = new ResponseStashItemModel("json_string");

        var dataModel = new DataModel
        {
            Id = Guid.NewGuid(),
            JsonContent = "json_string",
            EntityType = "string",
            LastModified = DateTime.Now
        };

        var command = new GetStashItemByIdQuery(Guid.NewGuid());
        _dataHandler.GetStashItem(Arg.Any<Guid>()).Returns(dataModel);

        //Act
        var result = await _sut.Handle(command, default);

        //Assert
        result.Should().BeOfType<ResponseStashItemModel>();
        result.Should().BeEquivalentTo(expectedValue);
    }

    [Fact]
    public async Task GetStashItemByIdHandler_ShouldReturnNullAndLogInfo_WhenStashIdIsNotFound()
    {
        //Arrange
        var command = new GetStashItemByIdQuery(Guid.NewGuid());
        _dataHandler.GetStashItem(Arg.Any<Guid>()).ReturnsNullForAnyArgs();

        //Act
        var result = await _sut.Handle(command, default);

        //Assert
        result.Should().BeNull();
        _logger.Received(1).LogInformation(
            Arg.Is("Expected to find item for {id} but no item was found. Either expired or doesn't exist!"),
            Arg.Any<Guid>());
    }

    [Fact]
    public async Task GetStashItemByIdHandler_ShouldThrowExceptionAndLogError_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new Exception();
        var command = new GetStashItemByIdQuery(Guid.NewGuid());
        _dataHandler.GetStashItem(Arg.Any<Guid>()).Throws(exception);

        //Act
        var requestAction = async () => await _sut.Handle(command, default);

        //Assert
        await requestAction.Should().ThrowAsync<Exception>();

        _logger.Received(1).LogError(Arg.Is(exception),
            Arg.Is("Error getting stash for item {id}"),
            Arg.Any<Guid>());
    }
}