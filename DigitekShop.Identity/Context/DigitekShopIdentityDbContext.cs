using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Identity.Configurations;
using DigitekShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitekShop.Identity.Context
{
    public class DigitekShopIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string, UserClaim, UserRole, UserLogin, IdentityRoleClaim<string>, UserToken>
    {
        public DigitekShopIdentityDbContext(DbContextOptions<DigitekShopIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
