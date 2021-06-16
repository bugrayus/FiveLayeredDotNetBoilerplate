using System.Threading.Tasks;
using Boilerplate.Business.Abstract;
using Boilerplate.Business.Core;
using Boilerplate.Entity.RequestModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Create

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var isCreated = await _userService.Create(request);
            return Success("User created successfully.", null, isCreated);
        }

        #endregion

        #region GetAll

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Success("Users fetched successfully.", null, users);
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            var isUpdated = await _userService.Update(request);
            return Success("User updated successfully.", null, isUpdated);
        }

        #endregion

        #region Delete

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _userService.Delete(id);
            return Success("User deleted successfully.", null, isDeleted);
        }

        #endregion

        #region Login

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userService.Login(request);
            return Success("Successfully logged in.", null, user);
        }

        #endregion
    }
}