using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebAppCore31.Interfaces;
using WebAppCore31.Logic;
using WebAppCore31.ModelsDTO;

namespace WebAppCore31
{
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentLogic logic;
        public CommentController(ICommentLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        [Route("Comment/GetComment/{courseId}")]
        public async Task<IActionResult> GetCommentByCourseId([FromRoute]string courseId)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var comments = await logic.GetCommentByCourseId(courseId);
            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        [Route("Comment/AddComment")]
        public async Task<IActionResult> AddComment([FromBody]CommentDTO comment)
        {
            if (ModelState.IsValid == false)
                return BadRequest(new ReturnMessage(msg: null, error: "Something went wrong!"));

            return Ok(await logic.AddComment(comment, HttpContext.User));
        }
    }
}
