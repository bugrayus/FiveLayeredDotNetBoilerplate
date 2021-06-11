using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Entity.Models;
using Boilerplate.Entity.RequestModels.User;
using Boilerplate.Entity.ResponseModels.User;

namespace Boilerplate.Business.Abstract
{
    public interface IUserService
    {
        Task<User> GetById(int id);
        Task<bool> Create(CreateUserRequest request);
        Task<List<User>> GetAll();
        Task<bool> Delete(int id);
        Task<bool> Update(UpdateUserRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        User GetUserByToken();
    }
}