using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Business.Abstract;
using Boilerplate.Business.Core;
using Boilerplate.Core.Helpers.Api;
using Boilerplate.Core.Helpers.Generators;
using Boilerplate.DAL.Abstract;
using Boilerplate.Entity.Models;
using Boilerplate.Entity.RequestModels.User;
using Boilerplate.Entity.ResponseModels.User;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly Token _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, Token tokenGenerator)
        {
            _userRepository = userRepository;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
        }

        #region Create

        public async Task<bool> Create(CreateUserRequest request)
        {
            var user = await _userRepository.GetByMail(request.Email);
            if (user != null)
            {
                _logger.LogWarning("User already exists");
                throw new ApiException(new Error
                {
                    Message = "User already exists"
                });
            }

            var passTuple = PasswordHasher.HashPassword(request.Password, null);
            var newUser = new User(request, passTuple);
            if (await _userRepository.Create(newUser))
            {
                _logger.LogInformation($"User created successfully with mail = {request.Email}");
                return true;
            }

            _logger.LogError("Error while updating user");
            throw new ApiException(new Error
            {
                Message = "Error while updating user"
            });
        }

        #endregion

        #region GetById

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        #endregion

        #region Delete

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                _logger.LogWarning("User does not exist");
                throw new ApiException(new Error
                {
                    Message = "Kullanıcı bulunamadı"
                });
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;
            if (await _userRepository.Update(user))
            {
                _logger.LogInformation($"User updated successfully = {user.Name} + {user.Surname}");
                return true;
            }

            _logger.LogError("Error while updating user");
            throw new ApiException(new Error
            {
                Message = "Kullanıcı güncellenirken hata"
            });
        }

        #endregion

        #region GetAll

        public async Task<List<User>> GetAll()
        {
            var users = await _userRepository.GetAll();
            if (users.Count == 0)
            {
                _logger.LogWarning("Couldn't find any active user");
                throw new ApiException(new Error
                {
                    Message = "Hergangi bir kullanıcı bulunamadı"
                });
            }

            _logger.LogInformation($"Users fetched = {users}");
            return users;
        }

        #endregion

        #region Update

        public async Task<bool> Update(UpdateUserRequest request)
        {
            var user = _userRepository.GetUserByToken();
            if (user == null)
            {
                _logger.LogWarning("User does not exist");
                throw new ApiException(new Error
                {
                    Message = "User does not exist"
                });
            }

            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Surname))
                user.Surname = request.Surname;
            if (await _userRepository.Update(user))
            {
                _logger.LogInformation($"User updated successfully = {user.Name} + {user.Surname}");
                return true;
            }

            _logger.LogError("Error while updating user");
            throw new ApiException(new Error
            {
                Message = "Error while updating user"
            });
        }

        #endregion

        #region Login

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByMail(request.Email);
            if (user == null)
            {
                _logger.LogInformation($"No user found by mail {request.Email}");
                throw new ApiException(new Error
                {
                    Message = "Mail ile eşleşen kullanıcı bulunamadı"
                });
            }

            var hashedPassword = PasswordHasher.HashPassword(request.Password, user.Salt).Item2;
            var token = _tokenGenerator.GenerateToken(user);
            if (hashedPassword == user.HashedPassword)
            {
                _logger.LogInformation($"User logged in successfully {request.Email}");
                return new LoginResponse(user, token);
            }

            _logger.LogInformation($"Wrong password {request.Email}");
            throw new ApiException(new Error
            {
                Message = "Girilen parola doğru değil"
            });
        }

        #endregion

        #region GetUserByToken

        public User GetUserByToken()
        {
            var user = _userRepository.GetUserByToken();
            if (user != null) return user;
            _logger.LogWarning("User does not exist");
            throw new ApiException(new Error
            {
                Message = "User does not exist"
            });
        }

        #endregion
    }
}