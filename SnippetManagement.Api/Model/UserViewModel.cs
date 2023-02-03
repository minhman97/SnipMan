using System.ComponentModel.DataAnnotations;

namespace SnippetManagement.Api.Model;

public class UserViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}