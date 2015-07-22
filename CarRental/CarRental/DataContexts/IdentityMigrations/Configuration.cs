namespace CarRental.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<CarRental.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\IdentityMigrations";
        }

        protected override void Seed(CarRental.DataContexts.IdentityDb context)
        {
            context.Roles.AddOrUpdate(
                               p => p.Name,
                               new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin" },
                               new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = "CarFleet" },
                               new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = "ServiceCenter" }
                           );
            context.Users.AddOrUpdate(
                                p => p.UserName,
                                new Models.ApplicationUser { Id = Guid.NewGuid().ToString(), Email = "nimbik@gmail.com", UserName = "nimbik@gmail.com", PasswordHash = new PasswordHasher().HashPassword("!QAZ2wsx"), EmailConfirmed = true, LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString() },
                                new Models.ApplicationUser { Id = Guid.NewGuid().ToString(), Email = "bios@mail.ru", UserName = "bios@mail.ru", PasswordHash = new PasswordHasher().HashPassword("!QAZ2wsx"), EmailConfirmed = true, LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString() }
                                );


        }
    }
}
