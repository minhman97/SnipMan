using FluentValidation;

namespace SnippetManagement.Api.Model.Validator;

public class SnippetViewModelValidator : AbstractValidator<SnippetViewModel>
{
    public SnippetViewModelValidator()
    {
        RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage("Please inter Content");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Please inter Name").MaximumLength(255).WithMessage("Length not over 255 characters");
    }
}