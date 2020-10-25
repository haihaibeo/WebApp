using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore31.Interfaces;
using WebAppCore31.ModelsDTO;

namespace WebAppCore31.Logic
{
    public class CommentLogic : ICommentLogic
    {
        private IDatabaseRepository dbrepo;
        private readonly UserManager<User> userManager;

        public CommentLogic(IDatabaseRepository dbrepo, UserManager<User> userManager)
        {
            this.dbrepo = dbrepo;
            this.userManager = userManager;
        }

        public async Task<List<CommentModel>> GetCommentByCourseId(string Id)
        {
            var comments = (await dbrepo.Comments.GetAllAsync()).Where(cmt => cmt.CourseId == Id);
            comments = comments.OrderByDescending(cmt => cmt.DateTime).ToList();

            var cmtModels = new List<CommentModel>();
            foreach (var item in comments)
                cmtModels.Add(new CommentModel(item));

            return cmtModels;
        }

        public async Task<ReturnMessage> AddComment(CommentDTO comment, ClaimsPrincipal userclaims)
        {
            var user = await userManager.GetUserAsync(userclaims);
            var course = await dbrepo.Courses.GetByIdAsync(comment.CourseId);

            if(course == null) return new ReturnMessage(msg: null, error: "Course doesn't exist");

            dbrepo.Comments.Create(new Comment
            {
                UserId = user.Id,
                CommentString = comment.CommentString,
                CourseId = comment.CourseId,
                DateTime = DateTime.Now
            });

            await dbrepo.SaveChangesAsync();
            return new ReturnMessage(msg: "Comment added!", error: null);
        }
    }
}
