using DigitekShop.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitekShop.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.HasData(
            //    new ApplicationUser
            //    {
            //        Id = "05446344-f9cc-4566-bd2c-36791b4e28ed",
            //        Email = "admin@localhost.com",
            //        NormalizedEmail = "ADMIN@LOCALHOST.COM",
            //        FullName = "Admin",
            //        UserName = "admin@localhost.com",
            //        NormalizedUserName = "ADMIN@LOCALHOST.COM",
            //        PasswordHash = "AQAAAAEAACcQAAAAELbXp1JgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQ==",
            //        EmailConfirmed = true,
            //        ConcurrencyStamp = "f3601366-fe65-4523-938a-85ffc2447df1",
            //        SecurityStamp = "a46a3e2d-dee3-4a4b-a010-44a554a8f9dc",
            //        LockoutEnabled = false,
            //        AccessFailedCount = 0,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false
            //    },
            //    new ApplicationUser
            //    {
            //        Id = "2ec9f480-7288-4d0f-a1cd-53cc89968b45",
            //        Email = "user@localhost.com",
            //        NormalizedEmail = "USER@LOCALHOST.COM",
            //        FullName = "System",
            //        UserName = "user@localhost.com",
            //        NormalizedUserName = "USER@LOCALHOST.COM",
            //        PasswordHash = "AQAAAAEAACcQAAAAELbXp1JgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQJgH+6VQpQ==",
            //        EmailConfirmed = true,
            //        ConcurrencyStamp = "4681f251-226d-4cd1-b5e2-86ac85d1e7a6",
            //        SecurityStamp = "41185186-9e47-4d35-8fa4-c34c9bffc3e4",
            //        LockoutEnabled = false,
            //        AccessFailedCount = 0,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false
            //    }
            //);
        }
    }
}
