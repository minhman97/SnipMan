using FluentValidation;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Model.Validator;

public class CreateSnippetRequestValidator : AbstractValidator<CreateSnippetRequest>
{
    public CreateSnippetRequestValidator()
    {
        RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage("Please inter Content");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Please inter Name").MaximumLength(255).WithMessage("Length not over 255 characters");
    }
}