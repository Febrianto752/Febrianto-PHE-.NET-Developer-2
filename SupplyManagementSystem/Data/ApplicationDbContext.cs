using SupplyManagementSystem.Models;
using System.Data.Entity;

namespace SupplyManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTender> ProjectTenders { get; set; }


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