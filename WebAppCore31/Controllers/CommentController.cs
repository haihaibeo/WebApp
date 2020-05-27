using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;
        public CommentController(RegisterContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Comment/GetComment/{courseId}")]
        public async Task<IActionResult> GetCommentByCourseId([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var result = await context.Comments.Where(c => c.CourseId == courseId).ToListAsync();
            result = result.OrderByDescending(c => c.DateTime).ToList();
            var comments = new List<CommentModel>();
            foreach(var item in result)
                comments.Add(new CommentModel(item));

            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        [Route("Comment/AddComment")]
        public async Task<IActionResult> AddComment([FromBody]Comment comment)
        {
            if (ModelState.IsValid == false)
                return BadRequest(new ReturnMessage(msg: null, error: "Something went wrong!"));

            var user = await userManager.GetUserAsync(this.HttpContext.User);

            var course = context.Courses.SingleOrDefaultAsync(c => c.Id == comment.CourseId);
            if (course == null)
                return Ok(new ReturnMessage(msg: null, error: "Course doesn't exist"));

            await context.Comments.AddAsync(
                new Comment()
                {
                    CourseId = comment.CourseId,
                    CommentString = comment.CommentString,
                    UserId = user.Id,
                    DateTime = DateTime.Now
                });
            var result = await context.SaveChangesAsync();
            return Ok();
        }
    }
}
