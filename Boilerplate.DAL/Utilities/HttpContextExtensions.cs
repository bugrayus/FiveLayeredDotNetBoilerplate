using Boilerplate.DAL.Context;
using Boilerplate.Entity.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Boilerplate.DAL.Utilities
{
    public static class HttpContextExtensions
    {
        public static User GetThisUser(this HttpContext context, BoilerplateContext dbContext)
        {
            if (!context.User.Claims.Any())
            {
                return null;
            }
            var claims = context.User.Claims.ToDictionary(u => u.Type, u => u.Value);
            var userId = claims[ClaimTypes.NameIdentifier];
            var user = dbContext.Users.FirstOrDefault(s => s.Id == int.Parse(userId) && s.IsActive);
            return user;
        }
    }
}