using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Repositories;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Services.Implementation;

public class JwtConfiguration
{
    public JwtConfiguration()
    {
    }
    [Required]
    public string IssuerSigningKey { get; set; }
    [Required]
    public string ValidAudience { get; set; }
    [Required]
    public string ValidIssuer { get; set; }
    [Required]
    public int ExpiringDays { get; set; }
}

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(IOptions<JwtConfiguration> jwtConfiguration, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public async Task<Result<string>> GetToken(UserDto userDto, bool isExternal)
    {
        var user = await _unitOfWork.UserRepository.Get(userDto.Email); //test user: a@a.vn/a
        if (isExternal)
        {
            if (user is null)
            {
                user = await _unitOfWork.UserRepository.Create(new CreateUserRequest(userDto.Email, null,
                    userDto.SocialProvider));
            }
        }
        else
        {
            if (user is null)
                return Result.Fail("User not existed.");

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                return Result.Fail("Incorrect username or password");
        }

        SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return Result.Ok(tokenHandler.WriteToken(securityToken));
    }

    private SecurityTokenDescriptor GetTokenDescriptor(UserDto user)
    {
        byte[] securityKey =
            Encoding.UTF8.GetBytes(_jwtConfiguration.IssuerSigningKey ?? throw new InvalidOperationException());
        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(_jwtConfiguration.ExpiringDays),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtConfiguration.ValidAudience,
            Issuer = _jwtConfiguration.ValidIssuer,
            Claims = new Dictionary<string, object>()
            {
                { "UserId", user.Id }
            }
        };

        return tokenDescriptor;
    }
}