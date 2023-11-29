using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.AssetType.Commands;

public class AddAssetTypeCommandValidator : AbstractValidator<AddAssetTypeCommand>
{
    private readonly IAssetTypeRepositoryAsync _repository;

    public AddAssetTypeCommandValidator(IAssetTypeRepositoryAsync repository)
    {
        _repository = repository;
        

        RuleFor(p => p.Name)
        .NotEmpty().WithMessage("{PropertyName} is required.")
        .NotNull().WithMessage("{PropertyName} cannot be null.")
        .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
        .MustAsync(IsUniqueName).WithMessage("{PropertyName} must be unique.");
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _repository.IsUniqueName(name, null, cancellationToken);
    }
}