using Stash_And_Grab.Application.ApplicationServices;
using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.Handlers;

internal sealed record
    CreateStashItemHandler : IRequestHandler<CreateStashItemCommand, ResponseStashItemStatusModel>
{
    private readonly IDataHandler _dataHandler;
    private readonly ILoggerAdaptor<CreateStashItemHandler> _logger;

    public CreateStashItemHandler(IDataHandler dataHandler, ILoggerAdaptor<CreateStashItemHandler> logger)
    {
        _dataHandler = dataHandler;
        _logger = logger;
    }

    public async Task<ResponseStashItemStatusModel> Handle(CreateStashItemCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dataModel = request.ItemData.ToDataModel();
            var id = await _dataHandler.InsertStashItem(dataModel);

            return new ResponseStashItemStatusModel(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating stash for item {@item}", request.ItemData);
            throw;
        }
    }
}