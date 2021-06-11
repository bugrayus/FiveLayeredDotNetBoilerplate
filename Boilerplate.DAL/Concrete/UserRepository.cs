using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.DAL.Abstract;
using Boilerplate.DAL.Context;
using Boilerplate.DAL.Utilities;
using Boilerplate.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Boilerplate.DAL.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(BoilerplateContext context, ILogger<UserRepository> logger,
            IHttpContextAccessor httpContextAccessor) : base(context, logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region GetByMail

        public async Task<User> GetByMail(string mail)
        {
            return await Context.Users.SingleOrDefaultAsync(u => u.Email == mail && u.IsActive);
        }

        #endregion

        #region GetById

        public async Task<User> GetById(int id)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == id && u.IsActive);
            return user;
        }

        #endregion

        #region GetAll

        public async Task<List<User>> GetAll()
        {
            return await Context.Users.Where(x => x.IsActive).ToListAsync();
        }

        #endregion

        #region GetUserByToken

        public User GetUserByToken()
        {
            return _httpContextAccessor.HttpContext.GetThisUser(Context);
        }

        #endregion
    }
}