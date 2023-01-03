using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SnippetManagement.Api.Model;

namespace SnippetManagement.Api.Services.Implementation;

public class AuthenticationService : IAuthenticationService
{
    
    public string GetToken(UserCredentials userCredentials)
    {
        if (userCredentials.UserName == "a" && userCredentials.Password == "a")
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
        //TODO: setup key into appsettings.json
        byte[] securityKey = Encoding.UTF8.GetBytes("KEY");
        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(expiringDays),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenDescriptor;
    }
}