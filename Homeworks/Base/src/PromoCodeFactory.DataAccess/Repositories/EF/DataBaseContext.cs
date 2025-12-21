using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class DataBaseContext : DbContext
    {

        /// <summary>
        /// Roles
        /// </summary>
        public DbSet<Role> Roles => Set<Role>();

        /// <summary>
        /// Employees
        /// </summary>
        public DbSet<Employee> Employees => Set<Employee>();

        public DataBaseContext() => Database.EnsureCreated();

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PromoCodeFactory.db");
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.();
        }
    }
}
