using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore31.ModelsDTO;

namespace WebAppCore31.Logic
{
    public interface ICourseLogic
    {
        Task<List<CourseDTO>> GetAllCourse();
        Task<CourseDTO> GetCourseById(string Id);
        Task<UserDTO> GetAuthorByCourseId(string courseId);
        Task<int> DeleteCourse(string courseId);
        Task<ReturnMessage> EditCourse(string courseId, PublishDTO newCourse, ClaimsPrincipal claims);
        Task<ReturnMessage> Publish(PublishDTO newCourse, ClaimsPrincipal claims);
        Task<ReturnMessage> Subscribe(string courseId, ClaimsPrincipal claims);
        Task<ReturnMessage> Unsubscribe(string courseId, ClaimsPrincipal claims);
        Task<ReturnMessage> IsSubscribed(string courseId, ClaimsPrincipal claims);
        Task<ReturnMessage> CanAuthorEditCourseById(string courseId, ClaimsPrincipal claims);
    }
}