using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.AssetType.Commands;

public class AddAssetTypeCommand : IRequest<Response<int>>
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}

public class AddAssetTypeCommandHandler : IRequestHandler<AddAssetTypeCommand, Response<int>>
{
    private readonly IAssetTypeRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public AddAssetTypeCommandHandler(IAssetTypeRepositoryAsync repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.AssetType>(request);
        await _repository.AddAsync(entity, cancellationToken);
        return new Response<int>(entity.Id);
    }
}