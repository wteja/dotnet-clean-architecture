using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.AssetType.Commands;

public class UpdateAssetTypeCommandValidator : AbstractValidator<UpdateAssetTypeCommand>
{
    private readonly IAssetTypeRepositoryAsync _repository;

    public UpdateAssetTypeCommandValidator(IAssetTypeRepositoryAsync repository)
    {
        _repository = repository;

        RuleFor(p => p.Name)
        .NotEmpty().WithMessage("{PropertyName} is required.")
        .NotNull().WithMessage("{PropertyName} cannot be null.")
        .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        RuleFor(p => p)
        .MustAsync((obj, cancel) => IsUniqueName(obj.Name!, obj.Id, cancel)).WithMessage("{PropertyName} must be unique.");
        
    }

    private async Task<bool> IsUniqueName(string name, int? skipId, CancellationToken cancellationToken)
    {

        return await _repository.IsUniqueName(name, skipId, cancellationToken);
    }
}