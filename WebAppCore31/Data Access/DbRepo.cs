using WebAppCore31.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WebAppCore31.Repositories
{
    public class DbRepo : IDatabaseRepository
    {
        private readonly RegisterContext context;
        private readonly UserManager<User> userManager;

        private AuthorRepo authorRepo;
        private StudentRepo studentRepo;
        private UserRepo userRepo;
        private CourseRepo courseRepo;
        private CommentRepo commentRepo;
        private StudentCourseRepo studentCourseRepo;

        public DbRepo(RegisterContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IUserRepository<User> Users
        {
            get
            {
                if (userRepo == null) userRepo = new UserRepo(context, userManager);
                return userRepo;
            }
        }

        public IUserRepository<Student> Students
        {
            get
            {
                if (studentRepo == null) studentRepo = new StudentRepo(context, userManager);
                return studentRepo;
            }
        }

        public IUserRepository<Author> Authors
        {
            get
            {
                if (authorRepo == null) authorRepo = new AuthorRepo(context, userManager);
                return authorRepo;
            }
        }

        public IRepository<Course> Courses
        {
            get
            {
                if (courseRepo == null) courseRepo = new CourseRepo(context);
                return courseRepo;
            }
        }

        public IRepository<Comment> Comments 
        {
            get
            {
                if (commentRepo == null) commentRepo = new CommentRepo(context);
                return commentRepo;
            }
        }

        public IRepository<StudentCourse> StudentCourses
        {
            get
            {
                if (studentCourseRepo == null) studentCourseRepo = new StudentCourseRepo(context);
                return studentCourseRepo;
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
