using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarRental.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.DataContexts
{
    public class AppDb: DbContext
    {
        // Сайт с инструкцией по настройке конфигурации для БД
        //http://pluralsight.com/training/Player?author=scott-allen&name=aspdotnet-mvc5-fundamentals-m6-ef6&mode=live&clip=2&course=aspdotnet-mvc5-fundamentals
        public AppDb()
            : base("DefaultConnection")
        {
        }

       public DbSet<Car> Cars { get; set; }
       public DbSet<Order> Orders { get; set;}
       public DbSet<Diagnostics> Diagnostic { get; set; }
       public DbSet<Picture> Pictures { get; set; }
       public DbSet<Email> Emails { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            ////modelBuilder.Entity<Order>()
            ////// Этот метод вернет ссылку на объект конфигурации Customer
            ////.HasRequired(p => p.CustomerOf)
            ////// Поэтому мы применяем метод WithRequiredDependent,
            ////// т.к. Customer является основной таблицей
            ////.WithRequiredDependent(c => c.Profile);

            //modelBuilder.Entity<Content>()
            //    .HasMany(c => c.Editors)
            //    .WithOptional()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Content>()
            //    .HasRequired(c => c.Owner)
            //    .WithOptional()
            //    .WillCascadeOnDelete(false);

            // modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
        }
    }
}

/*
Предварительно создаем папку DataContexts, там создаем DbContext наших талблиц и выносим туда Идентити контекст из IdentityModels

1. В Package Manager Console пишим enable-migrations. В результате будет ошибка, поскольку он не понимает какой контекст юзать. У нас их два
2. Пишем: enable-migrations -ContextTypeName IdentityDb -MigrationsDirectory DataContexts\IdentityMigrations
3. Пишем: enable-migrations -ContextTypeName AppDb -MigrationsDirectory DataContexts\AppMigrations
4. Пишем:  add-migration -ConfigurationTypeName CarRental.DataContexts.IdentityMigrations.Configuration "InitialCreate"
5. Пишем:  add-migration -ConfigurationTypeName CarRental.DataContexts.AppMigrations.Configuration "InitialCreate"
6. Пишем: PM> update-database -ConfigurationTypeName CarRental.DataContexts.IdentityMigrations.Configuration
7. Пишем: PM> update-database -ConfigurationTypeName CarRental.DataContexts.AppMigrations.Configuration
    



    */
