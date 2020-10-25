using DAL.DataContext;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DAL.Repositories
{
    public class DbRepo : IDatabaseRepository
    {
        private readonly RegisterContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        private AuthorRepo authorRepo;
        private StudentRepo studentRepo;
        private UserRepo userRepo;
        private CourseRepo courseRepo;
        private CommentRepo commentRepo;
        private StudentCourseRepo studentCourseRepo;

        public DbRepo(RegisterContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        public IRepository<Course> Courses => throw new NotImplementedException();

        public IRepository<Comment> Comments => throw new NotImplementedException();

        public IRepository<StudentCourse> StudentCourse => throw new NotImplementedException();

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
