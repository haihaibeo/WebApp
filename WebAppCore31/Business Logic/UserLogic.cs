using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore31.Interfaces;

namespace WebAppCore31.Logic
{
    public class UserLogic : IUserLogic
    {
        private IDatabaseRepository dbrepo;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserLogic(IDatabaseRepository dbrepo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.dbrepo = dbrepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> RegisterAsync(RegisterDTO reg)
        {
            User user;
            if (reg.Role == "Author")
                user = new Student();
            else if (reg.Role == "Student")
                user = new Author();
            else return false;
            user.Name = reg.FullName;
            user.Email = reg.Email;
            user.DateOfBirth = reg.DateOfBirth;
            user.UserName = reg.Email;

            var result = await userManager.CreateAsync(user, reg.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, reg.Role);
                await SignInAsync(user, reg.RememberMe);
                return true;
            }
            else return false;
        }

        public Task SignInAsync(User user, bool rememberMe)
        {
            return signInManager.SignInAsync(user, rememberMe);
        }

        public Task<SignInResult> PasswordSignAsync(LoginDTO login)
        {
            return signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
        }

        public Task SignOutAsync()
        {
            return signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<string>> GetCurrentUserRoleAsync(ClaimsPrincipal claims)
        {
            var user = await userManager.GetUserAsync(claims);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                return role;
            }
            else return null;
        }

        public async Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            var res = await userManager.GetUserAsync(user);
            return new UserDTO(res);
        }

        public async Task<UserDTO> GetUserByIdAsync(string Id)
        {
            var user = await dbrepo.Users.GetUserByIdAsync(Id);
            return new UserDTO(user);
        }
    }
}
