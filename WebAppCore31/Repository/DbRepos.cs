using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class DbRepos
    {
        private readonly RegisterContext context;
        public DbRepos(RegisterContext context)
        {
            this.context = context;
        }

        private AuthorRepository authorRepos;
        private UserRepository userRepos;
        private StudentRepository studentRepos;
        private CourseRepository courseRepos;

        public IRepository<Author> Authors
        {
            get
            {
                if (authorRepos == null) return authorRepos = new AuthorRepository();
                return authorRepos;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepos == null) return userRepos = new UserRepository();
                return userRepos;
            }
        }

        public IRepository<Student> Students
        {
            get
            {
                if (studentRepos == null) return studentRepos = new StudentRepository();
                return studentRepos;
            }
        }

        public IRepository<Course> Courses
        {
            get
            {
                if (courseRepos == null) return courseRepos = new CourseRepository();
                return courseRepos;
            }
        }

        public async Task<int> SaveAsync()
        {
            var result = await context.SaveChangesAsync();
            return result;
        }
    }
}
