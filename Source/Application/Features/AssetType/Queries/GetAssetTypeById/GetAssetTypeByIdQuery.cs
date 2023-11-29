using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.AssetType.Queries.GetAssetTypeByIdQuery;

public class GetAssetTypeByIdQuery : IRequest<Response<Domain.Entities.AssetType>>, IHasNumericId
{
    public int Id { get; set; }
}

public class GetAssetTypeByIdQueryHandler : IRequestHandler<GetAssetTypeByIdQuery, Response<Domain.Entities.AssetType>>
{
    private readonly IAssetTypeRepositoryAsync _repository;

    public GetAssetTypeByIdQueryHandler(IAssetTypeRepositoryAsync repository)
    {
        _repository = repository;
    }

    public async Task<Response<Domain.Entities.AssetType>> Handle(GetAssetTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (data == null)
        {
            throw new KeyNotFoundException("Asset Type not found.");
        }
        var response = new Response<Domain.Entities.AssetType>(data);
        return response;
    }
}