using System.ComponentModel.DataAnnotations;

namespace SnippetManagement.Api.Model;

public class UserViewModel
{
    public UserViewModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}