using FluentValidation;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Model.Validator;

public class FilterSnippetRequestValidator : AbstractValidator<SearchSnippetRequest>
{
    public FilterSnippetRequestValidator()
    {
        RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate).When(x => x.FromDate.HasValue)
            .WithMessage("To Date must greater than From Date");
    }
}