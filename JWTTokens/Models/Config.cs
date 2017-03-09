using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTTokens.Models
{
    public static class Config
    {
        private static string plainTextSecurityKey = "ncmdkdkdfkgjutsS";

        public static SymmetricSecurityKey GetKey()
        {
            var bytes = Encoding.UTF8.GetBytes(plainTextSecurityKey);
            var securityKey = new SymmetricSecurityKey(bytes);
            return securityKey;
        }
        public static SigningCredentials GetSecret()
        {
            var signingCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature);

            return signingCredentials;
        }
    }
}
