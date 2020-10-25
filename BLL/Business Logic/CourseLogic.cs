using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebAppCore31.Interfaces;
using WebAppCore31.ModelsDTO;

namespace WebAppCore31.Logic
{
    public class CourseLogic : ICourseLogic
    {
        private IDatabaseRepository dbrepo;
        private readonly UserManager<User> userManager;
        public CourseLogic(IDatabaseRepository dbrepo, UserManager<User> userManager)
        {
            this.dbrepo = dbrepo;
            this.userManager = userManager;
        }

        public async Task<List<CourseDTO>> GetAllCourse()
        {
            var courses = await dbrepo.Courses.GetAllAsync();
            var res = new List<CourseDTO>();
            foreach (var c in courses)
            {
                var user = await dbrepo.Users.GetUserByIdAsync(c.AuthorId);
                res.Add(new CourseDTO(c, user.Name));
            }
            return res;
        }

        public async Task<CourseDTO> GetCourseById(string Id)
        {
            var course = await dbrepo.Courses.GetByIdAsync(Id);
            if (course != null)
            {
                var user = await dbrepo.Users.GetUserByIdAsync(course.AuthorId);
                return new CourseDTO(course, user.Name);
            }
            else return null;
        }

        public async Task<UserDTO> GetAuthorByCourseId(string courseId)
        {
            var course = await dbrepo.Courses.GetByIdAsync(courseId);
            if (course == null)
                return null;
            var author = await dbrepo.Users.GetUserByIdAsync(course.AuthorId);
            return new UserDTO(author);
        }

        public async Task<int> DeleteCourse(string courseId)
        {
            var course = await dbrepo.Courses.GetByIdAsync(courseId);
            if (course == null) return -1;

            // Find and delete all comments first
            var comments = (await dbrepo.Comments.GetAllAsync()).Where(c => c.CourseId == courseId).ToList();
            foreach (var cmt in comments)
            {
                dbrepo.Comments.Delete(cmt.Id);
            }

            // Then delete all student-course related to the course 
            var studCourses = (await dbrepo.StudentCourses.GetAllAsync()).Where(sc => sc.CourseId == courseId).ToList();
            foreach (var sc in studCourses)
            {
                dbrepo.StudentCourses.Delete(sc.Id);
            }

            // finally delete course
            dbrepo.Courses.Delete(course.Id);

            return await dbrepo.SaveChangesAsync();
        }

        public async Task<ReturnMessage> EditCourse(string courseId, PublishDTO newCourse, ClaimsPrincipal claims)
        {
            var course = await dbrepo.Courses.GetByIdAsync(courseId);
            if (course == null) return new ReturnMessage(null, "Course does not exist!");

            // check if current user is the owner of the course
            var author = await userManager.GetUserAsync(claims);
            if (author.Id != course.AuthorId) return new ReturnMessage(null, "You are not the owner!");

            course.Title = newCourse.Title;
            course.Subject = newCourse.Subject;
            course.ContentCourse = newCourse.ContentCourse;

            dbrepo.Courses.Update(course);
            await dbrepo.SaveChangesAsync();
            return new ReturnMessage("Successfull", null);
        }

        public async Task<ReturnMessage> Publish(PublishDTO newCourse, ClaimsPrincipal claims)
        {
            var user = await userManager.GetUserAsync(claims);
            var course = new Course();
            course.AuthorId = user.Id;
            course.Title = newCourse.Title;
            course.Subject = newCourse.Subject;
            course.ContentCourse = newCourse.ContentCourse;

            dbrepo.Courses.Create(course);
            var result = await dbrepo.SaveChangesAsync();
            if (result > 0) return new ReturnMessage("Successful!", null);
            else return new ReturnMessage("Unsuccessful", "Error occured");
        }

        public async Task<ReturnMessage> Subscribe(string courseId, ClaimsPrincipal claims) 
        {
            var user = await userManager.GetUserAsync(claims);
            var course = await dbrepo.Courses.GetByIdAsync(courseId);
            if (course == null) return new ReturnMessage("Unsuccessful!", "Course does not exist!");

            var studCourse = new StudentCourse();
            studCourse.CourseId = courseId;
            studCourse.StudentId = user.Id;

            foreach (var sc in await dbrepo.StudentCourses.GetAllAsync())
            {
                if (sc.StudentId == user.Id && sc.CourseId == courseId)
                    return new ReturnMessage("Unsuccesful!", error: "Course is already register by user!");
            }
            dbrepo.StudentCourses.Create(studCourse);
            var result = await dbrepo.SaveChangesAsync();
            if (result > 0) return new ReturnMessage("Registered successfully", null);
            return new ReturnMessage(null, "Something went wrong!");
        }

        public async Task<ReturnMessage> Unsubscribe(string courseId, ClaimsPrincipal claims)
        {
            try
            {
                var stud = await userManager.GetUserAsync(claims);

                // first remove stud-course related to this stud and course
                var studCourse = (await dbrepo.StudentCourses.GetAllAsync()).SingleOrDefault(sc => sc.CourseId == courseId && sc.StudentId == stud.Id);
                // if user did not subs to this course
                if (studCourse == null) return new ReturnMessage(null, "You did not subscribe to this course!");
                // else
                dbrepo.StudentCourses.Delete(studCourse.Id);
                await dbrepo.SaveChangesAsync();
                return new ReturnMessage("Unsubscribed!", null);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(null, ex.Message);
            }
            
        }

        public async Task<ReturnMessage> IsSubscribed(string courseId, ClaimsPrincipal claims)
        {
            try
            {
                var user = await userManager.GetUserAsync(claims);
                var course = await dbrepo.Courses.GetByIdAsync(courseId);
                if (course == null) return new ReturnMessage(null, "Course does not exist!");

                var studCourse = (await dbrepo.StudentCourses.GetAllAsync()).SingleOrDefault(sc => sc.CourseId == courseId && sc.StudentId == user.Id);
                if (studCourse == null) return new ReturnMessage(false, null);
                else return new ReturnMessage(true, null);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(null, ex.Message);
            }
            
        }

        public async Task<ReturnMessage> CanAuthorEditCourseById(string courseId, ClaimsPrincipal claims)
        {
            try
            {
                var author = await userManager.GetUserAsync(claims);
                var course = await dbrepo.Courses.GetByIdAsync(courseId);

                if (course == null) return new ReturnMessage(null, "Course does not exist!");
                if (course.AuthorId == author.Id) return new ReturnMessage(true, null);
                else return new ReturnMessage(false, null);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(null, ex.Message);
            }
            
        }
    }
}
