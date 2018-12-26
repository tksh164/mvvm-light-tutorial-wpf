using Microsoft.EntityFrameworkCore;

namespace WpfMvvmLightTutorial.Model
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Data Source=.\DEV;Initial Catalog=CompanyDB;Integrated Security=true";
            optionsBuilder.UseSqlServer(connectionString, (providerOptions) => {
                providerOptions.CommandTimeout(60);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        public DbSet<Employee> Employees { get; set; }
    }
}
