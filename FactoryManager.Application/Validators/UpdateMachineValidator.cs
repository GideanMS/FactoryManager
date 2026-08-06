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
            .LessThanOrEqualTo(x => x.MaxProductionPerMinute)
            .WithMessage("Production per minute cannot exceed max production per minute.");
        RuleFor(x => x.ProductionPerMinute)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Production per minute cannot be negative.");
        RuleFor(x => x.MaxProductionPerMinute)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Max production per minute cannot be negative.");
        RuleFor(x => x.EnergyConsumptionPerMinute)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Energy consumption per minute cannot be negative.");
        RuleFor(x => x.MaintenanceIntervalInDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance interval in days cannot be negative.");
    }
}
