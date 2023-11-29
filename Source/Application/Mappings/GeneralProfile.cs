using Application.Features.AssetType.Commands;
using Application.Features.AssetType.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<AssetType, GetAllAssetTypesViewModel>().ReverseMap();
        CreateMap<AddAssetTypeCommand, AssetType>();
        CreateMap<UpdateAssetTypeCommand, AssetType>().ForAllMembers(src => src.Condition((src, dest, member) => member != null));
    }
}