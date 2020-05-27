using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DataContext
{
    public class RegisterContext : IdentityDbContext<User>
    { 
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                optionsBuilder = new DbContextOptionsBuilder<RegisterContext>();
                optionsBuilder.UseSqlServer(settings.SqlConnectionStr);
                options = optionsBuilder.Options;
            }
            public DbContextOptionsBuilder<RegisterContext> optionsBuilder { get; set; }
            public DbContextOptions options { get; set; }
            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild optionsBuild = new OptionsBuild();

        public RegisterContext(DbContextOptions<RegisterContext> options) : base(options) { }
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
