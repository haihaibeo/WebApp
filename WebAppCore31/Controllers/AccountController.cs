using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCore31.Controllers
{
    //[Route("api/[controller]")]
    // Controller : MVC controller with view support
    // ControllerBase : MVC controller without view support
    //[ApiController]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM reg)
        {
            if (ModelState.IsValid)
            {
                User user;
                if (reg.Role == "Student")
                    user = new Student();
                else if (reg.Role == "Author")
                    user = new Author();
                else
                {
                    var msgg = new
                    {
                        message = "Unsuccessfully register !",
                        error = "Invalid role !"
                    };
                    return Ok(msgg);
                }
                user.Name = reg.FullName;
                user.Email = reg.Email;
                user.DateOfBirth = reg.DateOfBirth;
                user.UserName = reg.Email;

                var result = await _userManager.CreateAsync(user, reg.Password);
                if (result.Succeeded)
                {
                    var resultRole = await _userManager.AddToRoleAsync(user, reg.Role);
                    await _signInManager.SignInAsync(user, reg.RememberMe);
                    var msg = new
                    {
                        message = $"User {reg.FullName} registered!"
                    };
                    return Ok(msg);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    var errorMsg = new
                    {
                        message = "User not added!",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return Ok(errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Неверные входные данные.",
                    error = ModelState.Values.SelectMany(e =>
                    e.Errors.Select(er => er.ErrorMessage))
                };
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginVM login)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: login.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var msg = new
                    {
                        message = "Logged in successfully!"
                    };
                    return Ok(msg);
                }
                else
                {
                    ModelState.AddModelError("", "Wrong combination of login and password!");
                    var errorMsg = new
                    {
                        message = "Failed logging in",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage))
                    };
                    return Ok(errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var msg = new
            {
                message = "Signed out!"
            };
            return Ok(msg);
        }
    }
}