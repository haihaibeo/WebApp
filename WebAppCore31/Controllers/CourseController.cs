using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebAppCore31.Controllers
{
    [Route("course")]
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

        [HttpGet]
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.Include(c => c.Author).ToList();
        }

        //[Route("course/{id}")]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            return Ok(result);
        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> DeleteCourse([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var course = await _context.Courses.FindAsync(courseId);
            if (course.AuthorId == user.Id)
            {
                _context.Courses.Remove(course);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllCourses));
            }
            return BadRequest();
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

        [HttpPost("subscribe/{courseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Subscribe([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
                return BadRequest();

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var studcourse = new StudentCourse();
            studcourse.CourseId = courseId;
            studcourse.StudentId = user.Id;

            foreach (var item in _context.StudentCourses)
            {
                if(item.CourseId == courseId && item.StudentId == user.Id)
                {
                    var msg = new
                    {
                        error = "Course is already registered by the user!"
                    };
                    return Ok(msg);
                }
            }
            await _context.StudentCourses.AddAsync(studcourse);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}