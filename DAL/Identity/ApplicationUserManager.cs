using DAL.DataContext;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Identity
{
    public class ApplicationUserManager : UserManager<User>
    {
        //public ApplicationUserManager(IUserStore<User> store, IdentityFactoryOptions<ApplicationUserManager> options) : base(store)
        //{
            
        //}
    }
}
