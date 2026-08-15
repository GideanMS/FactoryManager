using FactoryManager.Application.DTOs.Products;
using FluentValidation;

namespace FactoryManager.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name cannot be empty.");
    }
}