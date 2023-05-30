using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SnippetManagement.Common.Enum;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Repositories;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Services.Implementation;

public class JwtConfiguration
{
    public string IssuerSigningKey { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
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

    public async Task<Result> GetToken(UserDto userDto)
    {
        var user = await _unitOfWork.UserRepository.Get(userDto.Email); //test user: a@a.vn/a
        if (user != null && BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return Result.Ok().WithSuccess(tokenHandler.WriteToken(securityToken));
        }

        return Result.Fail("Wrong username or password");
    }

    public async Task<Result> GetTokenForExternalProvider(string externalToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(externalToken);
        var email = jwtToken.Payload["email"].ToString();

        if (string.IsNullOrEmpty(email)) return Result.Fail("Wrong username or password");

        var userDto = await _unitOfWork.UserRepository.Get(email);
        if (userDto is null)
        {
            userDto = await _unitOfWork.UserRepository.Create(new CreateUserRequest()
            {
                Email = email,
                SocialProvider = SocialProvider.Google
            });
        }

        SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userDto);
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return Result.Ok().WithSuccess(tokenHandler.WriteToken(securityToken));
    }

    private SecurityTokenDescriptor GetTokenDescriptor(UserDto user)
    {
        byte[] securityKey = Encoding.UTF8.GetBytes(_jwtConfiguration.IssuerSigningKey);
        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddSeconds(30),
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