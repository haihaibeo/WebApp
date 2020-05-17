using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebAppCore31.Controllers
{
    [Route("course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RegisterContext _context;

        private const string author = "Author";
        private const string student = "Student";

        public CourseController(UserManager<User> userManager, RegisterContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        //[Route("course/{id}")]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var result = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            CourseModel course = new CourseModel(result);
            return Ok(course);
        }

        [HttpGet("/Account/{accId}")]
        public async Task<IActionResult> GetAllCourseByAuthorId([FromRoute] string accId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(accId);
            var courses = new List<Course>();
            foreach (var item in _context.Courses)
            {
                if (item.AuthorId == accId)
                    courses.Add(item);
            }

            return Ok(courses);
        }

        [HttpGet("GetAuthor/{courseId}")]
        public async Task<IActionResult> GetAuthorByCourseId([FromRoute]string courseId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            if (course != null)
            {
                var author = new UserModel(await _context.Users.SingleOrDefaultAsync(u => u.Id == course.AuthorId));
                return Ok(author);
            }
            else return Ok(new ReturnMessage(msg: null, error: "Course doesn't exist"));
        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> DeleteCourse([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
                return BadRequest();

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return NotFound();

            //--Delete Comment--//
            var comments = _context.Comments.ToList().FindAll(c => c.CourseId == course.Id);
            _context.Comments.RemoveRange(comments);

            //--Delete StudentCourse--//
            var studCourses = _context.StudentCourses.ToList().FindAll(sc => sc.CourseId == course.Id);
            _context.StudentCourses.RemoveRange(studCourses);

            _context.Courses.Remove(course);

            var result = await _context.SaveChangesAsync();
            return Ok(new ReturnMessage("Deleted", null));
        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> EditCourse([FromRoute]string courseId, [FromBody] Course new_course)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);

            if (course != null)
            {
                var author = await _userManager.GetUserAsync(HttpContext.User);
                if (course.AuthorId != author.Id)
                    return BadRequest();

                course.Title = new_course.Title;
                course.Subject = new_course.Subject;
                course.ContentCourse = new_course.ContentCourse;
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = author)]
        public async Task<IActionResult> CreateCourse([FromBody]Course course)
        {
            if (ModelState.IsValid == true)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var role = await _userManager.GetRolesAsync(user);
                if (user != null)
                {
                    course.AuthorId = user.Id;
                    await _context.Courses.AddAsync(course);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            else return BadRequest();
        }

        [HttpPost("subscribe/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> Subscribe([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var studcourse = new StudentCourse();
            studcourse.CourseId = courseId;
            studcourse.StudentId = user.Id;

            foreach (var item in _context.StudentCourses)
            {
                if(item.CourseId == courseId && item.StudentId == user.Id)
                    return Ok(new ReturnMessage(msg: null, error: "Course is already registered by the user!"));
            }

            await _context.StudentCourses.AddAsync(studcourse);
            await _context.SaveChangesAsync();
            return Ok(new ReturnMessage(msg: "Subscribed successfully !", error:null));
        }

        [HttpDelete("Unsubscribe/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> Unsubscribe([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var stud = await _userManager.GetUserAsync(HttpContext.User);
            if(stud != null)
            {
                var studCourse = await _context.StudentCourses.SingleOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == stud.Id);
                if (studCourse != null)
                {
                    _context.StudentCourses.Remove(studCourse);
                    await _context.SaveChangesAsync();
                    return Ok(new ReturnMessage("Unscubscribed!", null));
                }
                else return Ok(new ReturnMessage(null, "Error"));
            }
            return Ok(new ReturnMessage(null, "User not found"));
        }

        [HttpGet("IsSubscribed/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> IsSubscribed([FromRoute]string courseId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var course = await _context.Courses.FindAsync(courseId);
            if(course != null)
            {
                var studCourse = await _context.StudentCourses.SingleOrDefaultAsync(sc => sc.StudentId == user.Id && sc.CourseId == course.Id);
                if(studCourse != null)
                {
                    var msg = new ReturnMessage(msg: true, error: null);
                    return Ok(msg);
                }
                else
                {
                    var msg = new ReturnMessage(msg: false, error: null);
                    return Ok(msg);
                }
            }
            else
            {
                var msg = new ReturnMessage(null, "Course not existed");
                return Ok(msg);
            }
        }

        [HttpGet("CanAuthorEditCourse/{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> CanAuthorEditCourseById([FromRoute]string courseId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            if (user != null && course != null)
            {
                if (course.AuthorId == user.Id)
                    return Ok(new ReturnMessage(msg: true, error: null));
                else return Ok(new ReturnMessage(msg: false, error: null));
            }
            else if (user == null) return Ok(new ReturnMessage(msg: null, error: "User not found"));
            else if (course == null) return Ok(new ReturnMessage(msg: null, error: "Course not found"));
            return Ok(new ReturnMessage(null, "Unexpected error"));
        }
    }
}