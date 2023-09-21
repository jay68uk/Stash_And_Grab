using Stash_And_Grab.Application.ApiServices;
using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.Handlers;

internal sealed record GetStashItemByIdHandler : IRequestHandler<GetStashItemByIdQuery, ResponseStashItemModel>
{
    private readonly IDataHandler _dataHandler;
    private readonly ILoggerAdaptor<GetStashItemByIdHandler> _logger;

    public GetStashItemByIdHandler(IDataHandler dataHandler, ILoggerAdaptor<GetStashItemByIdHandler> logger)
    {
        _dataHandler = dataHandler;
        _logger = logger;
    }

    public async Task<ResponseStashItemModel> Handle(GetStashItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var item = await _dataHandler.GetStashItem(request.StashItemId);

            item.ThrowIfNull();

            return item.ToResponseStashItemModel();
        }
        catch (ArgumentNullException)
        {
            _logger.LogInformation(
                "Expected to find item for {id} but no item was found. Either expired or doesn't exist!",
                request.StashItemId);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting stash for item {id}", request.StashItemId);
            throw;
        }
    }
}