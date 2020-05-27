using DAL.DataContext;
using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Identity
{
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore(RegisterContext context) : base(context) { }
    }
}
