using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class DataBaseContext : DbContext
    {

        /// <summary> Roles </summary>
        public DbSet<Role> Roles => Set<Role>();

        /// <summary> Employees </summary>
        public DbSet<Employee> Employees => Set<Employee>();

        /// <summary> Customers </summary>
        public DbSet<Customer> Customers => Set<Customer>();

        /// <summary> Preferences </summary>
        public DbSet<Preference> Preferences => Set<Preference>();

        /// <summary> PromoCodes </summary>
        public DbSet<PromoCode> PromoCodes => Set<PromoCode>();

        public DataBaseContext()
        {
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PromoCodeFactory.db");
            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                        .HasMany(e => e.PromoCodes)
                        .WithOne(c => c.Customer)
                        .HasForeignKey(k => k.CustomerId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

            modelBuilder.Entity<Customer>()
                        .HasMany(e => e.Preferences)
                        .WithMany()
                        .UsingEntity(
                            nameof(CustomerPreference),
                            r => r.HasOne(typeof(Customer)).WithMany().HasForeignKey(nameof(CustomerPreference.CustomerId)).HasPrincipalKey(nameof(Customer.Id)),
                            l => l.HasOne(typeof(Preference)).WithMany().HasForeignKey(nameof(CustomerPreference.PreferenceId)).HasPrincipalKey(nameof(Preference.Id)),
                            j => j.HasKey(nameof(CustomerPreference.CustomerId), nameof(CustomerPreference.PreferenceId)));

        }

        public static void InitDb()
        {
            using var context = new DataBaseContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(FakeDataFactory.Employees);
            context.SaveChanges();

            context.AddRange(FakeDataFactory.Preferences);
            context.SaveChanges();

            context.AddRange(FakeDataFactory.Customers);
            context.SaveChanges();
        }
    }
}
