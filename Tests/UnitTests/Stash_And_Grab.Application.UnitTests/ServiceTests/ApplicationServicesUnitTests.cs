using FluentAssertions;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Stash_And_Grab.Application.ApiServices;
using Stash_And_Grab.Application.Commands;
using Stash_And_Grab.Application.DtoModels;
using Stash_And_Grab.Application.Logging;
using Stash_And_Grab.Application.Queries;
using Stash_And_Grab.Application.ResponseModels;

namespace Stash_And_Grab.Application.UnitTests.ServiceTests;

public class ApplicationServicesUnitTests
{
    private readonly ILoggerAdaptor<ApplicationServices>
        _logger = Substitute.For<ILoggerAdaptor<ApplicationServices>>();

    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IApplicationServices _sut;
    private readonly IValidator<StashCreateDtoModel> _validator = Substitute.For<IValidator<StashCreateDtoModel>>();

    public ApplicationServicesUnitTests()
    {
        _sut = new ApplicationServices(_validator, _mediator, _logger);
    }

    [Fact]
    public async Task GetItem_ShouldReturnSuccessResultStashItem_WhenIdIsFound()
    {
        //Arrange
        var expected = new Result<ResponseStashItemModel>().WithValue(new ResponseStashItemModel("json_string"));

        //Act
        _mediator.Send(Arg.Any<GetStashItemByIdQuery>()).Returns(new ResponseStashItemModel("json_string"));

        var result = await _sut.GetItem(Guid.NewGuid());

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetItem_ShouldReturnFailResultStashItem_WhenIdIsNotFound()
    {
        //Arrange
        var expected = new Result<ResponseStashItemModel>().WithError("Id not found");

        //Act
        _mediator.Send(Arg.Any<GetStashItemByIdQuery>()).Throws(new ArgumentNullException());

        var result = await _sut.GetItem(Guid.NewGuid());

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeEquivalentTo(expected.Reasons[0]);
    }

    [Fact]
    public async Task GetItem_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetStashItemByIdQuery>()).Returns(new ResponseStashItemModel(string.Empty));

        //Act
        await _sut.GetItem(Guid.NewGuid());

        //Assert
        _logger.Received(1).LogInformation(Arg.Is("Attempting to get stash for {id}"), Arg.Any<Guid>());
        _logger.Received(1).LogInformation(Arg.Is("Finished attempt to get stash for {id} after {time}"),
            Arg.Any<Guid>(), Arg.Any<double>());
    }

    [Fact]
    public async Task CreateStashItem_ShouldReturnSuccessResultId_WhenStashItemIsCreated()
    {
        //Arrange
        var id = Guid.NewGuid();
        var requestModel = new StashCreateDtoModel("stash01", "json_string", "string");
        var expected = new Result<ResponseStashItemStatusModel>().WithValue(new ResponseStashItemStatusModel(id));

        //Act
        _validator.ValidateAsync(Arg.Any<StashCreateDtoModel>()).Returns(new ValidationResult());
        _mediator.Send(Arg.Any<CreateStashItemCommand>()).Returns(new ResponseStashItemStatusModel(id));

        var result = await _sut.CreateStashItem(requestModel);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CreateStashItem_ShouldReturnFailResult_WhenStashCreateDtoModelIsNotValid()
    {
        //Arrange
        var requestModel = Arg.Any<StashCreateDtoModel>();

        //Act
        _validator.ValidateAsync(requestModel)
            .Returns(new ValidationResult(new ValidationFailure[1] { new(string.Empty, string.Empty) }));
        _mediator.Send(Arg.Any<CreateStashItemCommand>()).ReturnsNullForAnyArgs();

        var result = await _sut.CreateStashItem(requestModel);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task CreateStashItem_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        var requestModel = new StashCreateDtoModel("stash01", "json_string", "string");

        //Act
        _validator.ValidateAsync(Arg.Any<StashCreateDtoModel>()).Returns(new ValidationResult());
        _mediator.Send(Arg.Any<CreateStashItemCommand>()).Returns(new ResponseStashItemStatusModel(Guid.NewGuid()));
        await _sut.CreateStashItem(requestModel);

        //Assert
        _logger.Received(1)
            .LogInformation(Arg.Is("Attempting to create stash for {@item}"), Arg.Any<StashCreateDtoModel>());
        _logger.Received(1).LogInformation(Arg.Is("Finished attempt to create stash after {time}"), Arg.Any<double>());
    }

    [Fact]
    public async Task GetStashItemByIdHandler_ShouldThrowExceptionAndLogError_WhenExceptionIsThrown()
    {
        //Arrange
        _validator.ValidateAsync(Arg.Any<StashCreateDtoModel>()).Returns(new ValidationResult());
        _mediator.Send(Arg.Any<CreateStashItemCommand>()).ThrowsAsync(new Exception());

        //Act
        var requestAction = async () =>
            await _sut.CreateStashItem(new StashCreateDtoModel(string.Empty, string.Empty, string.Empty));

        //Assert
        await requestAction.Should().ThrowAsync<Exception>();
    }
}