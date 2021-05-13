using Boilerplate.Entity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boilerplate.DAL.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByMail(string mail);
        Task<User> GetById(int id);
        Task<List<User>> GetAll();
        User GetUserByToken();
    }
}
