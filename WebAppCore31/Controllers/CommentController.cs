using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebAppCore31
{
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly RegisterContext context;
        public CommentController(RegisterContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("Comment/GetComment/{courseId}")]
        public async Task<IActionResult> GetCommentByCourseId([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var result = await context.Comments.Where(c => c.CourseId == courseId).ToListAsync();
            var comments = new List<CommentModel>();
            foreach(var item in result)
            {
                comments.Add(new CommentModel(item));
            }
            return Ok(comments);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("Comment/AddComment")]
        public async Task<IActionResult> AddComment([FromBody]Comment comment)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            await context.Comments.AddAsync(
                new Comment()
                {
                    CourseId = comment.CourseId,
                    CommentString = comment.CommentString,
                    StudentId = comment.StudentId,
                    DateTime = DateTime.Now
                });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
