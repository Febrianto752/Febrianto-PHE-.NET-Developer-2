using SupplyManagementSystem.Models;
using SupplyManagementSystem.Utilities;
using SupplyManagementSystem.Utilities.Enums;
using System;
using System.Data.Entity;

namespace SupplyManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationDbContext")
        {
            if (!Database.Exists())
            {
                Database.SetInitializer(new Seeder());
                Database.Initialize(true);
            }
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTender> ProjectTenders { get; set; }


    }

    public class Seeder : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Code to seed data
            context.Accounts.Add(new Account { Guid = Guid.NewGuid(), Name = "Admin", Password = HashingHandler.HashPassword("Admin123"), Email = "admin@gmail.com", NoTelp = "081236767632", Role = nameof(RoleEnum.Admin), CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now });

            context.Accounts.Add(new Account { Guid = Guid.NewGuid(), Name = "Ria Sutrani", Password = HashingHandler.HashPassword("Manager123"), Email = "manager@gmail.com", NoTelp = "081236733332", Role = nameof(RoleEnum.Manager), CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now });

            context.SaveChanges();
        }
    }

    //public class AccountConfiguration : EntityTypeConfiguration<Account>
    //{
    //    public AccountConfiguration()
    //    {
    //        this.ToTable("Account")
    //            .HasKey(a => a.Guid);
    //    }
    //}
}