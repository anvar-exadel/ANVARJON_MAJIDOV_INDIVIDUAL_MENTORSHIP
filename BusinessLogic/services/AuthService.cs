using BusinessLogic.interfaces;
using DatabaseAccess;
using Shared.apiResponse.serviceResponse;
using Shared.dtos.auth;
using Shared.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public ServiceResponse<UserDto> Register(RegisterDto registerDto)
        {
            if (UserExists(registerDto.UserName)) return new ServiceResponse<UserDto>(null, false, "Username is already taken", ResponseType.Failed);
            if(_context.AppUsers.Any(u => u.Email == registerDto.Email)) return new ServiceResponse<UserDto>(null, false, "Email is already in use", ResponseType.Failed);

            AppUser newUser = new AppUser 
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                UserRole = UserRole.User
            };

            //make first user admin
            if (!_context.AppUsers.Any()) newUser.UserRole = UserRole.Admin;

            _context.AppUsers.Add(newUser);
            _context.SaveChanges();

            return new ServiceResponse<UserDto>(GetUserDtoFromAppUser(newUser));
        }

        public ServiceResponse<UserDto> Login(LoginDto loginDto)
        {
            AppUser user = _context.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == loginDto.UserName.ToLower());
            if (user == null) return new ServiceResponse<UserDto>(null, false, "Username is not found", ResponseType.Failed);

            return new ServiceResponse<UserDto>(GetUserDtoFromAppUser(user));
        }

        private bool UserExists(string username)
        {
            return _context.AppUsers.Any(u => u.UserName.ToLower() == username.ToLower());
        }

        private UserDto GetUserDtoFromAppUser(AppUser appUser)
        {
            return new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Token = _tokenService.GetToken(appUser)
            };
        }
    }
}
