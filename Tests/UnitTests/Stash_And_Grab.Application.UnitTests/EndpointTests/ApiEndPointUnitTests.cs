using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Stash_And_Grab.Api;
using Stash_And_Grab.Application.ApplicationServices;
using Stash_And_Grab.Application.Logging;
using Stash_And_Grab.Application.ResponseModels;

namespace Stash_And_Grab.Application.UnitTests.EndpointTests;

public class ApiEndPointUnitTests : IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _application;
    private readonly IApplicationServices _applicationServices;
    private readonly IConfiguration _configuration;
    private readonly ILoggerAdaptor<Program> _logger;

    public ApiEndPointUnitTests()
    {
        _applicationServices = Substitute.For<IApplicationServices>();
        _configuration = Substitute.For<IConfiguration>();
        _logger = Substitute.For<ILoggerAdaptor<Program>>();

        _application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_applicationServices);
                    services.AddSingleton(_configuration);
                    services.AddSingleton(_logger);
                });
            });
    }

    public ValueTask DisposeAsync()
    {
        return _application.DisposeAsync();
    }

    [Fact]
    public async Task GetStashItem_ShouldReturnOKAndStashValue_WhenStashIdIsFound()
    {
        //Arrange
        using var client = _application.CreateClient();
        _applicationServices.GetItem(Arg.Any<Guid>()).Returns(new Result<ResponseStashItemModel>().WithSuccess("")
            .WithValue(new ResponseStashItemModel("json_string")));

        //Act
        var response = await client.GetAsync("/stash/" + Guid.NewGuid());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetStashItem_ShouldReturnBadRequest_WhenStashIdIsNotGuid()
    {
        //Arrange
        using var client = _application.CreateClient();
        _applicationServices.GetItem(Arg.Any<Guid>()).Returns(new Result<ResponseStashItemModel>().WithSuccess("")
            .WithValue(new ResponseStashItemModel("json_string")));

        //Act
        var response = await client.GetAsync("/stash/" + "x");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetStashItem_ShouldReturnNotFound_WhenStashIdIsNotFound()
    {
        //Arrange
        using var client = _application.CreateClient();
        var expected = new List<Reason> { new("Id not found", new Dictionary<string, object>()) };
        _applicationServices.GetItem(Arg.Any<Guid>())
            .Returns(new Result<ResponseStashItemModel>().WithError("Id not found"));

        //Act
        var response = await client.GetAsync("/stash/" + Guid.NewGuid());
        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<List<Reason>>(content);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseObject.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetStashItem_ShouldLogError_WhenExceptionIsThrown()
    {
        //Arrange
        using var client = _application.CreateClient();
        var exception = new Exception();
        _applicationServices.GetItem(Arg.Any<Guid>()).Throws(exception);

        //Act
        var response = await client.GetAsync("/stash/" + Guid.NewGuid());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        _logger.Received(1).LogError(Arg.Is(exception),
            Arg.Is("Error getting result for {Id}"), Arg.Any<Guid>());
    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record Reason(string? message, Dictionary<string, object> metadata);