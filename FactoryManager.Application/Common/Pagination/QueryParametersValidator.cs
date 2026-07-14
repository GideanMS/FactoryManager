using FactoryManager.Application.Common.Pagination;
using FluentValidation;

namespace FactoryManager.Application.Validators;

public class QueryParametersValidator<T> : AbstractValidator<T> where T : QueryParameters
{
    public QueryParametersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}