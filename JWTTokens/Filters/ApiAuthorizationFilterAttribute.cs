using JWTTokens.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace JWTTokens.Filters
{
    public class ApiAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public const string Authorization = "Authorization";
        public const string Bearer = "bearer ";
        public const string UserId = "userId";

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers[Authorization];
                token = token.Replace(Bearer, "");

                var handler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudiences = new string[]
                    {
                        "http://xxx.azurewebsites.net",
                        "http://localhost:62661"
                    },
                    ValidIssuers = new string[]
                    {
                        "AzureSend",
                    },
                   // RequireExpirationTime = false,
                    IssuerSigningKey = Config.GetKey()
                };

                SecurityToken validatedToken;

                var claims = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                var name = claims.FindFirst(c => c.Type == "name");
                var userId = claims.FindFirst(c => c.Type == UserId);

                //get user from Db by user id

                actionContext.ActionArguments.Add("name", $"{name.Value}:{userId.Value}");

                var issuer = validatedToken.Issuer;
            }
            catch (Exception)
            {

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Wrong token or userId!" };
                return;
            }
        }


    }
}
