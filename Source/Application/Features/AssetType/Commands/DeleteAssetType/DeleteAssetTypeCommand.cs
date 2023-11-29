using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.AssetType.Commands;

public class DeleteAssetTypeCommand : IRequest<Response<int>>, IHasNumericId
{
    public int Id { get; set; }
}

public class DeleteAssetTypeCommandHandler : IRequestHandler<DeleteAssetTypeCommand, Response<int>>
{
    private readonly IAssetTypeRepositoryAsync _repository;

    public DeleteAssetTypeCommandHandler(IAssetTypeRepositoryAsync repository)
    {
        _repository = repository;
    }
    public async Task<Response<int>> Handle(DeleteAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (data == null)
        {
            throw new KeyNotFoundException("Budget Type not found.");
        }
        else
        {
            await _repository.DeleteAsync(data, cancellationToken);
            return new Response<int>(data.Id);
        }
    }
}