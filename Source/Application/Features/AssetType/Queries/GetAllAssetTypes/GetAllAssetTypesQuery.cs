using AutoMapper;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Application.Parameters;

namespace Application.Features.AssetType.Queries;

public class GetAllAssetTypesQuery : RequestParameter, IRequest<PagedResponse<IEnumerable<GetAllAssetTypesViewModel>>>
{
    public string? name { get; set; }
}

public class GetAllAssetTypesQueryHandler : IRequestHandler<GetAllAssetTypesQuery, PagedResponse<IEnumerable<GetAllAssetTypesViewModel>>>
{
    private readonly IAssetTypeRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public GetAllAssetTypesQueryHandler(IAssetTypeRepositoryAsync repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllAssetTypesViewModel>>> Handle(GetAllAssetTypesQuery request, CancellationToken cancellationToken)
    {
        var rawData = await _repository.GetPagedResponseAsync(request.PageNumber, request.PageSize, cancellationToken);
        var data = _mapper.Map<IEnumerable<GetAllAssetTypesViewModel>>(rawData);
        return new PagedResponse<IEnumerable<GetAllAssetTypesViewModel>>(data, request.PageNumber, request.PageSize);
    }
}