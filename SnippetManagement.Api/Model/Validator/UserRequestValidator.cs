using FluentValidation;
using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Api.Model.Validator;

public class UserRequestValidator: AbstractValidator<UserViewModel>
{
    public UserRequestValidator(SnippetManagementDbContext context)
    {
        RuleFor(x => x.Email).Must(email =>
        {
            var user = context.Set<User>().FirstOrDefault(x => x.Email == email);
            return user == null;
        }).WithMessage(model => $"Sorry, this email {model.Email} is already taken");
    }
}