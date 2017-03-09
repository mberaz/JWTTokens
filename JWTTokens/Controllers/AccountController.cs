using JWTTokens.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace JWTTokens.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        // {
        // "UserName":"michael",
        // "Password":"5461661"
        // }

        //iss (issuer), exp (expiration time), sub (subject), aud (audience),
        //https://jwt.io/
        //https://scotch.io/tutorials/the-anatomy-of-a-json-web-token

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserModel model)
        {
            // register user in DB, get user ID
            var userId = 5;

            var claims = new List<Claim> {
                new Claim("name",model.UserName),
                new Claim("userId",userId.ToString())};

            var tokenString = JwtHandler.GetToken(claims, Request.BaseUrl());
            return Ok(tokenString);
            //return Ok(ApiAuthorizationFilterAttribute.Bearer + tokenString);
        }

        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> Login(UserModel model)
        {
            // validate user and password in DB
            var userId = 5;

            var claims = new List<Claim> {
                new Claim("name",model.UserName),
                new Claim("userId",userId.ToString())};

            var tokenString = JwtHandler.GetToken(claims, Request.BaseUrl());
            return Ok(tokenString);
            //return Ok(ApiAuthorizationFilterAttribute.Bearer + tokenString);
        }
    }
}
