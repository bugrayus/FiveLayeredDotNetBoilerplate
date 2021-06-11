using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Business.Abstract;
using Boilerplate.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Business.Core
{
    public class Jwt
    {
        private readonly AppSettings _appSettings;
        private readonly RequestDelegate _next;

        public Jwt(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) await AttachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
                var jwtToken = (JwtSecurityToken) validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
                context.Items["User"] = await userService.GetById(userId);
            }
            catch
            {
                // ignored
            }
        }
    }
}