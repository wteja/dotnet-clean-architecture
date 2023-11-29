using Application.Features.AssetType.Commands;
using Application.Features.AssetType.Queries;
using Application.Features.AssetType.Queries.GetAssetTypeByIdQuery;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("asset-types")]
public class AssetTypeController : BaseCRUDController<GetAllAssetTypesQuery, GetAssetTypeByIdQuery, AddAssetTypeCommand, UpdateAssetTypeCommand, DeleteAssetTypeCommand>
{
    public AssetTypeController()
    {
    }
}
