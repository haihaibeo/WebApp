using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public partial class RegisterContext : IdentityDbContext<User>
    {
        public RegisterContext(DbContextOptions<RegisterContext> options) : base(options)
        { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);       
        }
    }
}
