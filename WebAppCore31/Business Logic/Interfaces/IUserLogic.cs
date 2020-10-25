﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAppCore31.Logic
{
    public interface IUserLogic
    {
        Task<ReturnMessage> RegisterAsync(RegisterDTO reg, ModelStateDictionary modelState);
        Task SignInAsync(User user, bool rememberMe);
        Task<SignInResult> PasswordSignAsync(LoginDTO login);
        Task SignOutAsync();
        Task<IEnumerable<string>> GetCurrentUserRoleAsync(ClaimsPrincipal claims);
        Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<UserDTO> GetUserByIdAsync(string Id);
    }
}