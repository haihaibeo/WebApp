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
using WebAppCore31.Interfaces;
using WebAppCore31.Logic;
using WebAppCore31.ModelsDTO;
using System.Net;
using Microsoft.AspNetCore.Razor.Language;
using System.Net.Http;

namespace WebAppCore31.Controllers
{
    [Route("course")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private const string author = "Author";
        private const string student = "Student";

        private ICourseLogic logic;
        public CourseController(ICourseLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public async Task<List<CourseDTO>> GetAllCourses()
        {
            return await logic.GetAllCourse();
        }

        //[Route("course/{id}")]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            try
            {
                var result = await logic.GetCourseById(courseId);
                if (result == null) return Ok(new ReturnMessage(msg: "Unsuccessful!", error: "Course does not exist!"));
                return Ok(result);
            }
            catch
            {
                return Ok(new ReturnMessage(msg: "Unsuccessful!", error: "Unexpected error!"));
            }
        }

        //[HttpGet("/Account/{accId}")]
        //public async Task<IActionResult> GetAllCourseByAuthorId([FromRoute] string accId)
        //{
        //    if (ModelState.IsValid == false)
        //        return BadRequest();

        //    var user = await _userManager.FindByIdAsync(accId);
        //    var courses = new List<Course>();
        //    foreach (var item in _context.Courses)
        //    {
        //        if (item.AuthorId == accId)
        //            courses.Add(item);
        //    }

        //    return Ok(courses);
        //}

        [HttpGet("GetAuthor/{courseId}")]
        public async Task<IActionResult> GetAuthorByCourseId([FromRoute]string courseId)
        {
            var result = await logic.GetAuthorByCourseId(courseId);
            if(result == null) 
                return Ok(new ReturnMessage(msg: null, error: "Course doesn't exist"));
            
            return Ok(result);
        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> DeleteCourse([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
                return BadRequest();

            var result = await logic.DeleteCourse(courseId);
            if (result == -1) return NotFound();

            return Ok(new ReturnMessage("Deleted", null));
        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> EditCourse([FromRoute]string courseId, [FromBody] PublishDTO new_course)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(await logic.EditCourse(courseId, new_course, HttpContext.User));
        }

        [HttpPost]
        [Authorize(Roles = author)]
        public async Task<IActionResult> CreateCourse([FromBody]PublishDTO course)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            return Ok(await logic.Publish(course, HttpContext.User));
        }

        [HttpPost("subscribe/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> Subscribe([FromRoute]string courseId)
        {
            if (ModelState.IsValid != true)
                return BadRequest(ModelState);

            return Ok(await logic.Subscribe(courseId, HttpContext.User));
        }

        [HttpDelete("Unsubscribe/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> Unsubscribe([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            return Ok(await logic.Unsubscribe(courseId, HttpContext.User));
        }

        [HttpGet("IsSubscribed/{courseId}")]
        [Authorize(Roles = student)]
        public async Task<IActionResult> IsSubscribed([FromRoute]string courseId)
        {
            return Ok(await logic.IsSubscribed(courseId, HttpContext.User));
        }

        [HttpGet("CanAuthorEditCourse/{courseId}")]
        [Authorize(Roles = author)]
        public async Task<IActionResult> CanAuthorEditCourseById([FromRoute]string courseId)
        {
            return Ok(await logic.CanAuthorEditCourseById(courseId, HttpContext.User));
        }
    }
}