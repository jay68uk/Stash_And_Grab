using System.Diagnostics;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;

namespace Stash_And_Grab.Application.ApiServices;

public sealed class ApplicationServices : IApplicationServices
{
    private readonly ILoggerAdaptor<ApplicationServices> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<StashCreateDtoModel> _validator;

    public ApplicationServices(IValidator<StashCreateDtoModel> validator, IMediator mediator,
        ILoggerAdaptor<ApplicationServices> logger)
    {
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<ResponseStashItemModel>> GetItem(Guid id)
    {
        _logger.LogInformation("Attempting to get stash for {id}", id);
        var stopWatch = Stopwatch.StartNew();

        try
        {
            return await _mediator.Send(new GetStashItemByIdQuery(id));
        }
        catch (ArgumentNullException)
        {
            return new Result<ResponseStashItemModel>().WithError("Id not found");
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("Finished attempt to get stash for {id} after {time}", id,
                stopWatch.Elapsed.TotalMilliseconds);
        }
    }

    public async Task<Result<ResponseStashItemStatusModel>> CreateStashItem(StashCreateDtoModel item)
    {
        _logger.LogInformation("Attempting to create stash for {@item}", item);
        var stopWatch = Stopwatch.StartNew();

        try
        {
            var validationResult = await _validator.ValidateAsync(item);

            if (validationResult.IsValid)
                return await _mediator.Send(new CreateStashItemCommand(item.ToRequestStashItemCreateModel()));

            return ToErrorResult(validationResult);
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("Finished attempt to create stash after {time}",
                stopWatch.Elapsed.TotalMilliseconds);
        }
    }

    private static Result<ResponseStashItemStatusModel> ToErrorResult(ValidationResult validationResult)
    {
        var errorMessages = validationResult.Errors.Select(e => new Error(e.ErrorMessage)).ToList();

        return Result.Fail<ResponseStashItemStatusModel>(errorMessages);
    }
}