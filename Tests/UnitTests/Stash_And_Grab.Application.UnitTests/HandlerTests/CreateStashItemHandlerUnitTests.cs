using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Stash_And_Grab.Application.Commands;
using Stash_And_Grab.Application.DtoModels;
using Stash_And_Grab.Application.Handlers;
using Stash_And_Grab.Application.Interfaces;
using Stash_And_Grab.Application.Logging;
using Stash_And_Grab.Application.RequestModels;
using Stash_And_Grab.Application.ResponseModels;

namespace Stash_And_Grab.Application.UnitTests.HandlerTests;

public class CreateStashItemHandlerUnitTests
{
    private readonly IDataHandler _dataHandler = Substitute.For<IDataHandler>();

    private readonly ILoggerAdaptor<CreateStashItemHandler> _logger =
        Substitute.For<ILoggerAdaptor<CreateStashItemHandler>>();

    private readonly CreateStashItemHandler _sut;

    public CreateStashItemHandlerUnitTests()
    {
        _sut = new CreateStashItemHandler(_dataHandler, _logger);
    }

    [Fact]
    public async Task CreateStashItemHandler_ShouldReturnResponseStashItemStatusModel_WhenStashIsCreated()
    {
        //Arrange
        var id = Guid.NewGuid();
        var expectedValue = new ResponseStashItemStatusModel(id);

        var requestModel = new RequestStashItemCreateModel("stash01", "json_string", "string");
        var dataModel = new DataModel
            { Id = id, JsonContent = "json_string", EntityType = "string", LastModified = DateTime.Now };

        var command = new CreateStashItemCommand(requestModel);
        _dataHandler.InsertStashItem(Arg.Do<DataModel>(x => dataModel = x)).Returns(id);

        //Act
        var result = await _sut.Handle(command, default);

        //Assert
        result.Should().BeOfType<ResponseStashItemStatusModel>();
        result.Should().BeEquivalentTo(expectedValue);
    }

    [Fact]
    public async Task CreateStashItemHandler_ShouldThrowExceptionAndLogError_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new Exception();
        var mockModel = Arg.Any<DataModel>();

        var command =
            new CreateStashItemCommand(new RequestStashItemCreateModel(string.Empty, string.Empty, string.Empty));
        _dataHandler.InsertStashItem(mockModel).Throws(exception);


        //Act
        var requestAction = async () => await _sut.Handle(command, default);

        //Assert
        await requestAction.Should().ThrowAsync<Exception>();

        _logger.Received(1).LogError(Arg.Is(exception),
            Arg.Is("Error creating stash for item {@item}"),
            Arg.Any<RequestStashItemCreateModel>());
    }
}