using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebAppCore31.Interfaces;
using WebAppCore31.Logic;
using WebAppCore31.Repositories;

namespace WebAppCore31
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RegisterContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<RegisterContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IDatabaseRepository, DbRepo>();
            services.AddScoped<ICourseLogic, CourseLogic>();
            services.AddScoped<ICommentLogic, CommentLogic>();
            services.AddScoped<IUserLogic, UserLogic>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            });

            // Avoid object loop when called by API
            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // setup cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SimpleWebApp";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/";
                options.LogoutPath = "/Account/Logout";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            CreateRoles(service, "Student").Wait();
            CreateRoles(service, "Author").Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider, string newRole)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (await roleManager.FindByNameAsync(newRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(newRole));
            }
        }
    }
}
