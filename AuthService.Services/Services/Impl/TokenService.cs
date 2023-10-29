using AuthService.Repositories.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services.Impl
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptionsModel _jwtOptionsModel;

        public TokenService(IOptions<JwtOptionsModel> jwtOptionsModel)
        {
            _jwtOptionsModel = jwtOptionsModel.Value;
        }

        public string GenerateToken(AccountModel accountModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptionsModel.SecretKey);

            var claims = new List<Claim>
            {
                new Claim("PhoneNumber", accountModel.PhoneNumber),
                new Claim("UserId", accountModel.AccountId.ToString()),
                new Claim(ClaimTypes.Role, accountModel.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtOptionsModel.Issuer,
                Audience = _jwtOptionsModel.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptionsModel.AccessTokenExpiration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
