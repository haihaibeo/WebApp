using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppCore31.Interfaces;
using WebAppCore31.Logic;

namespace WebAppCore31.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly IUserLogic logic;
        public AccountController(IUserLogic logic)
        {
            this.logic = logic;
        }

        /// <summary>
        /// Register a new user with role
        /// </summary>
        /// <param name="reg"></param>
        /// <returns>ReturnMessage</returns>
        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO reg)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                if (reg.Role != "Student" && reg.Role != "Author")
                    return Ok(new ReturnMessage(msg: "Unsuccessfully register!", error: "Invalid role!"));

                var result = await logic.RegisterAsync(reg);
                if (result == true)
                    return Ok(new ReturnMessage($"User {reg.FullName} registered!", null));
                else
                    return Ok(new ReturnMessage(msg: "User not added!", error: ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))));
            }
            catch
            {
                return Ok(new ReturnMessage(null, "Unexpected error!"));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await logic.PasswordSignAsync(login);
            if (result.Succeeded)
                return Ok(new ReturnMessage(msg: "Logged in successfully", error: null));
            else
            {
                ModelState.AddModelError("", "Wrong combination of login and password!");
                return Ok(new ReturnMessage("Error logging in", error: ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage))));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await logic.SignOutAsync();
            return Ok(new ReturnMessage("Signed out!", null));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/IsAuthenticated")]
        public async Task<IActionResult> GetCurrentUserRoleAsync()
        {
            var role = await logic.GetCurrentUserRoleAsync(HttpContext.User);
            if (role == null)
                return Ok(new ReturnMessage(null, "User not logged in!"));
            else
            {
                return Ok(new ReturnMessage(msg: role, null));
            }
        }

        [HttpGet("Account/GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await logic.GetCurrentUserAsync(HttpContext.User);
            if (user != null)
                return Ok(user);
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/GetUser/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute]string userId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var user = await logic.GetUserByIdAsync(userId);
            if (user != null)
                return Ok(user);
            return Ok(new ReturnMessage(null, "User not found"));
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