using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace WebAppCore31.Controllers
{
    [Route("api")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RegisterContext _context;

        public CourseController(UserManager<User> userManager, RegisterContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (ModelState.IsValid == true)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var role = await _userManager.GetRolesAsync(user);
                if (user != null)
                {
                    course.AuthorId = user.Id;
                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            else return BadRequest();
        }
    }
}