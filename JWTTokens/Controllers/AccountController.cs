﻿using JWTTokens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using JWTTokens.Filters;

namespace JWTTokens.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        [Route("")]
        [HttpPost]
        public IHttpActionResult Register(UserModel model)
        {
            var now = DateTime.UtcNow;
            var expire = now.AddHours(5);
            var header = new JwtHeader(Config.GetSecret());
            //iss (issuer), exp (expiration time), sub (subject), aud (audience),
            //https://jwt.io/
            https://scotch.io/tutorials/the-anatomy-of-a-json-web-token


            // register user in DB, get user ID
            //var payload = new JwtPayload
            //{
            //    {"iss", "AzureSend"},//issuer
            //    {"exp ", now.AddHours(5)},//expiration time
            //    {"iat" ,now},//The time the JWT was issued,
            //    { "aud","http://localhost:62661"},
            //    { "name",model.UserName},
            //    { "userId",5}
            //};

            var calimList = new List<Claim> {
                new Claim("iss", "AzureSend"),
                new Claim("iat" ,now.ToString()),
                new Claim("exp ",expire.ToString()),
                new Claim("aud","http://localhost:62661"),
                new Claim("name",model.UserName),
                new Claim("userId",5.ToString())
            };

            var payload = new JwtPayload("AzureSend", "http://localhost:62661", calimList, now, expire);
            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);

            return Ok(ApiAuthorizationFilterAttribute.Bearer + tokenString);
        }




    }
}
