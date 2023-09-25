using System.Diagnostics;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;

namespace Stash_And_Grab.Application.ApplicationServices;

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
        return await MeasureExecutionTime(async () =>
        {
            _logger.LogInformation("Attempting to get stash for {id}", id);

            try
            {
                var result = await _mediator.Send(new GetStashItemByIdQuery(id));

                return result ?? Result.Fail<ResponseStashItemModel>(new Error("Id not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stash item for {Id}.", id);
                throw; // You can decide whether to rethrow, or handle and return a Result.
            }
        }, "Finished attempt to get stash for {id}", id);
    }

    public async Task<Result<ResponseStashItemStatusModel>> CreateStashItem(StashCreateDtoModel item)
    {
        return await MeasureExecutionTime(async () =>
        {
            _logger.LogInformation("Attempting to create stash for {@item}", item);
            var stopWatch = Stopwatch.StartNew();


            var validationResult = await _validator.ValidateAsync(item);

            if (validationResult.IsValid)
                return await _mediator.Send(new CreateStashItemCommand(item.ToRequestStashItemCreateModel()));

            return ToErrorResult(validationResult);
        }, "Finished attempt to create stash");
    }

    internal async Task<T> MeasureExecutionTime<T>(Func<Task<T>> action, string logMessage, params object[] logArgs)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            return await action();
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation(logMessage + " after {time}ms",
                logArgs.Append(stopWatch.Elapsed.TotalMilliseconds).ToArray());
        }
    }

    private static Result<ResponseStashItemStatusModel> ToErrorResult(ValidationResult validationResult)
    {
        var errorMessages = validationResult.Errors.Select(e => new Error($"{e.PropertyName}: {e.ErrorMessage}"))
            .ToList();

        return Result.Fail<ResponseStashItemStatusModel>(errorMessages);
    }
}