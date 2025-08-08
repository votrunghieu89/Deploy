using Azure.Core;
using GraphQLandEF.DTO.UserDTo;
using GraphQLandEF.DTO.Users;
using GraphQLandEF.Model.Users;
using GraphQLandEF.Repositories;
using GraphQLandEF.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLandEF.Services.Users
{
    public class AuthService
    {
        private readonly IUsersRepository _authRepository;
        private readonly AccesToken _accessToken;

        public AuthService(IUsersRepository authRepository, AccesToken accessToken)
        {
            _authRepository = authRepository;
            _accessToken = accessToken;
        }
        public async Task<string> Register(UserDTO user)
        { 
            if(user == null)
            {
                return "User data is null.";
            }
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
            {
                return "Username, password, and email are required.";
            }
            bool isUserExists = await _authRepository.IsUserExists(user.UserName);
            if (isUserExists)
            {
                return "Username already exists.";
            }
            UserModel? isEmailExists = await _authRepository.IsEmailExists(user.Email);
           
            if (isEmailExists != null)
            {
                Console.WriteLine("aaa");
                return "Email already exists.";
            }
            bool registrationResult = await _authRepository.Register(user);
            if (registrationResult)
            {
                return "Registration successful.";
            }
            else
            {
                return "Registration failed.";
            }
        }

        public async Task<LoginResponseDto> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            bool loginResult = await _authRepository.Login(username, password);
            UserModel user = await _authRepository.GetUserByUserName(username);
            if (loginResult)
            {
                var accessToken = await _accessToken.GenerateAccessToken(user);
                LoginResponseDto loginResponseDto = new LoginResponseDto(
                    accessToken,
                    user.UserId,
                    user.UserName,
                    user.Email
                    );
                return loginResponseDto;
            }
            else
            {
                return null;
            }
        }
    }
}
