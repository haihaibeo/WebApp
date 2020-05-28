using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore31.ModelsDTO;

namespace WebAppCore31.Logic
{
    public interface ICommentLogic
    {
        Task<List<CommentModel>> GetCommentByCourseId(string Id);
        Task<ReturnMessage> AddComment(CommentDTO comment, ClaimsPrincipal userclaims);
    }
}