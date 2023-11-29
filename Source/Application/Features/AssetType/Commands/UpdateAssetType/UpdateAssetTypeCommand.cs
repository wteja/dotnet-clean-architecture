using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.AssetType.Commands;

public class UpdateAssetTypeCommand : IRequest<Response<int>>, IHasNumericId
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateAssetTypeCommandHandler : IRequestHandler<UpdateAssetTypeCommand, Response<int>>
{
    private readonly IAssetTypeRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public UpdateAssetTypeCommandHandler(IAssetTypeRepositoryAsync repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(UpdateAssetTypeCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (data == null)
        {
            throw new KeyNotFoundException("Budget Type not found.");
        }
        else
        {
            data = _mapper.Map<Domain.Entities.AssetType>(request);
            await _repository.UpdateAsync(data, cancellationToken);
            return new Response<int>(data.Id);
        }
    }
}