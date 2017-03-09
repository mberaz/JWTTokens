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
        public const string Name = "name";

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var token = actionContext.Header(Authorization); //HttpContext.Current.Request.Headers[Authorization];
                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "No token!" };
                    return;
                }

                token = token.Replace(Bearer, "");

                var result = JwtHandler.DecodeToken(token, actionContext.BaseUrl());
                var name = result.ClaimsPrincipal.FindFirst(c => c.Type == Name);
                var userId = result.ClaimsPrincipal.FindFirst(c => c.Type == UserId);

                //get user from Db by user id


                actionContext.ActionArguments.Add(Name, $"{name.Value}:{userId.Value}");

                var issuer = result.SecurityToken.Issuer;
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Wrong token or userId!" };
                return;
            }
        }


    }
}
