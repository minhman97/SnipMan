using FluentValidation;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Model.Validator;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x => x.TagName).NotNull().NotEmpty().WithMessage("Please inter Tag Name");
    }
}