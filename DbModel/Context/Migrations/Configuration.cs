namespace DbModel.Context.Migrations
{
    using DomainClasses.Entities;
    using DomainClasses.Enums;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using Utilities.Security;
    public sealed class Configuration : DbMigrationsConfiguration<MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MyDbContext context)
        {
            context.Roles.AddOrUpdate(x => new { x.Name, x.Description },
                 new Role { Name = "admin", Description = "Admin" },
                 new Role { Name = "student", Description = "Student" },
                 new Role { Name = "professor", Description = "Professor" });

            context.SiteOptions.AddOrUpdate(op => new { op.Name, op.Value },
                new SiteOption { Name = "Department_Name", Value = "" },
                new SiteOption { Name = "Term_Name", Value = "" },
                new SiteOption { Name = "Start_Election_Date", Value = "11/28/2017" },
                new SiteOption { Name = "Start_Election_Time", Value = "09:50 AM" },
                new SiteOption { Name = "End_Election_Date", Value = "11/29/2017" },
                new SiteOption { Name = "End_Election_Time", Value = "10:50 AM" },

                new SiteOption { Name = "Start_Remove_Date", Value = "11/29/2017" },
                new SiteOption { Name = "Start_Remove_Time", Value = "10:50 AM" },
                new SiteOption { Name = "End_Remove_Date", Value = "11/29/2017" },
                new SiteOption { Name = "End_Remove_Time", Value = "10:50 AM" },
                new SiteOption { Name = "Student_max_Unit", Value = "15" });

            context.SaveChanges();

            context.Users.AddOrUpdate(u => u.UserName, new User
            {
                RegisterDate = DateTime.Now,
                IsBaned = false,
                RegisterType = UserRegisterType.Active,
                Password = Encryption.EncryptingPassword("123456"),
                Role = context.Roles.Find(1),
                UserName = "admin"
            });
            base.Seed(context);
        }
    }
}
