﻿using SnippetManagement.Common.Enum;

namespace SnippetManagement.Service.Model;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public SocialProvider? SocialProvider { get; set; }
}