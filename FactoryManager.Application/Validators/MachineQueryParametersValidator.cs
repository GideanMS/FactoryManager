using FactoryManager.Application.Common.Sorting;
using FactoryManager.Application.Validators;
using FluentValidation;

namespace FactoryManager.Application.DTOs.Machines;

public class MachineQueryParametersValidator : QueryParametersValidator<MachineQueryParameters>
{
    public MachineQueryParametersValidator()
    {
        RuleFor(x => x.MinProduction)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinProduction.HasValue);

        RuleFor(x => x.MaxProduction)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxProduction.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinProduction.HasValue || !x.MaxProduction.HasValue || x.MinProduction <= x.MaxProduction)
            .WithMessage("MinProduction must be less than or equal to MaxProduction");

        RuleFor(x => x.SortBy)
            .Must(sortBy => string.IsNullOrWhiteSpace(sortBy) || MachineSortExpressions.SortExpressions.ContainsKey(sortBy.ToLowerInvariant()))
            .WithMessage("Invalid sorting field.");

        RuleFor(x => x.SortDirection)
            .IsInEnum()
            .WithMessage("Invalid sorting direction.");
    }
}