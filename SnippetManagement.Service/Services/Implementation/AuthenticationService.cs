using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Repositories;

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

    public async Task<string> GetToken(UserCredentials userCredentials)
    {
        var user = await _unitOfWork.UserRepository.Get(userCredentials.Email); //test user: a@a.vn/a
        if (user != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.Password))
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor();
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        return string.Empty;
    }

    private SecurityTokenDescriptor GetTokenDescriptor()
    {
        byte[] securityKey = Encoding.UTF8.GetBytes(_jwtConfiguration.IssuerSigningKey);
        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(_jwtConfiguration.ExpiringDays),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtConfiguration.ValidAudience,
            Issuer = _jwtConfiguration.ValidIssuer
        };

        return tokenDescriptor;
    }
}