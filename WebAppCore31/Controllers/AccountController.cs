using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

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
        private readonly RegisterContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RegisterContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                else return Ok(new ReturnMessage(msg: "Unsuccessfully register!", error: "Invalid role!"));

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
                    return Ok(new ReturnMessage(msg: "User not added!", error: ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))));
                }
            }
            else return Ok(new ReturnMessage(msg: "Неверные входные данные.", error: ModelState.Values.SelectMany(e =>
                    e.Errors.Select(er => er.ErrorMessage))));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: login.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                    return Ok(new ReturnMessage(msg: "Logged in successfully", error: null));
                else
                {
                    ModelState.AddModelError("", "Wrong combination of login and password!");
                    return Ok(new ReturnMessage("Error logging in", error: ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage))));
                }
            }
            else return Ok(new ReturnMessage(
                msg: "Error logging in",
                error: ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)))
                );
        }

        [HttpPost]
        [Authorize]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ReturnMessage("Signed out!", null));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/IsAuthenticated")]
        public async Task<IActionResult> GetCurrentUserRoleAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return Ok(new ReturnMessage(null, "User not logged in!"));
            else
            {
                var role = await _userManager.GetRolesAsync(user);
                return Ok(new ReturnMessage(msg: role, null));
            }
        }

        [HttpGet("Account/GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
                return Ok(new UserModel(user));
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/GetUser/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute]string userId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                return Ok(new UserModel(user));
            }
            else return Ok(new ReturnMessage(null, "User not found"));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/{userId}")]
        public async Task<IActionResult> GetUserByFromRouteId([FromRoute]string userId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();
            var result = await GetUserById(userId);
            return Ok(result);
        }
    }
}