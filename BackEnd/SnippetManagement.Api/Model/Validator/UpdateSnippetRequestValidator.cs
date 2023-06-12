using FluentValidation;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Model.Validator;

public class UpdateSnippetRequestValidator : AbstractValidator<UpdateSnippetRequest>
{
    public UpdateSnippetRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please inter Name");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Please inter Content");
    }
    
}