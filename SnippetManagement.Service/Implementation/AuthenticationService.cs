using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Implementation;

public class JwtConfiguration
{
    public string? IssuerSigningKey { get; set; }
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
}

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(IOptions<JwtConfiguration> jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public async Task<string> GetToken(UserCredentials userCredentials)
    {
        //TODO: Get user to create token
        if (userCredentials is { UserName: "a", Password: "a" })
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
        const int expiringDays = 1;
        byte[] securityKey = Encoding.UTF8.GetBytes(_jwtConfiguration.IssuerSigningKey ?? string.Empty);
        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(expiringDays),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtConfiguration.ValidAudience,
            Issuer = _jwtConfiguration.ValidIssuer
        };

        return tokenDescriptor;
    }
}