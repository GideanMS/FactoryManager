using FactoryManager.Application.DTOs.Machines;
using FluentValidation;

namespace FactoryManager.Application.Validators;

public class UpdateMachineValidator : AbstractValidator<UpdateMachineRequest>
{
    public UpdateMachineValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Machine name cannot be empty.");

        RuleFor(x => x.ProductionPerMinute)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Production per minute cannot be negative.");
    }
}
