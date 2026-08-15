using FactoryManager.Application.DTOs.Products;
using FluentValidation;

namespace FactoryManager.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name cannot be empty.");
    }
}

