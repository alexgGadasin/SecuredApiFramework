using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Identity.Data;
using Domain.Identity.Business;
using Microsoft.Extensions.Options;
using Domain.Api;
using System.Collections.Generic;
using System.Security.Claims;
using Domain.Identity.Oauth;

namespace Domain.Identity.Web
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;
        public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }
        public async Task Invoke(HttpContext context)
        {
            var key = context.Request.Headers.ContainsKey("Authorization") ? "Authorization" : "X-Authorization";
            var auth = context.Request.Headers[key].FirstOrDefault();

            if (auth != null)
            {
                var method = auth.Split(" ").First();
                var token = auth.Split(" ").Last();

                AttachUserToContext(context, token, method);
            }

            await _next(context);
        }
        private void AttachUserToContext(HttpContext context, string token, string method)
        {
            try
            {
                if(method == "Bearer")
                {
                    context.User = _tokenService.Validate(token);
                    context.Items["Sub"] = context.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).First().Value;
                }
                else if (method == "Basic")
                {
                    var client = ClientIdentifier.Get(token);
                    context.Items["Sub"] = client.client_id;
                }
            }
            catch (Exception ex) 
            {
                if(ex.Message.StartsWith("IDX10223"))
                {
                    context.Items["Err"] = "IDX10223";
                    context.Items["ErrMsg"] = "Lifetime validation failed. The token is expired.";
                }
            }
        }
    }
}
