using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTTokens.Models
{
    public static class JwtHandler
    {
        private static string plainTextSecurityKey = "ncmdkdkdfkgjutsS";
        private static string Issuer = "AzureSend";
        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
        }
        public static SigningCredentials GetSecret()
        {
            return new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature);
        }

        public static string GetToken(List<Claim> claims, string baseUrl)
        {
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.AddHours(5);

            var claimsList = new List<Claim> {
                new Claim("iss",Issuer),
                new Claim("iat",notBefore.ToString()),
                new Claim("exp",expires.ToString()),
                new Claim("aud",baseUrl),
            };

            claimsList.AddRange(claims);

            var securityToken = new JwtSecurityToken(new JwtHeader(GetSecret()), new JwtPayload(Issuer, baseUrl, claimsList, notBefore, expires));
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public static DecodeReturnModel DecodeToken(string token, string baseUrl)
        {
            SecurityToken validatedToken;
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new[] { baseUrl },
                ValidIssuers = new[] { Issuer },
                IssuerSigningKey = GetKey()
            };

            var claims = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out validatedToken);

            return new DecodeReturnModel
            {
                ClaimsPrincipal = claims,
                SecurityToken = validatedToken
            };
        }
    }

    public class DecodeReturnModel
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public SecurityToken SecurityToken { get; set; }
    }
}
