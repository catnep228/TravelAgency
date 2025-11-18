using Microsoft.EntityFrameworkCore;
using TravelAgency.Configurations;
using TravelAgency.Models;

namespace TravelAgency.Data
{
    public class DataContext : DbContext


    {
        private readonly MySqlServerVersion _mySqlServerVersion;
        private readonly IConfiguration _configuration;
        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlServerVersion = new MySqlServerVersion(new Version(8, 0));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Passanger> Passangers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TypeOfHoliday> TypeOfHolidays { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseMySql(_configuration.GetConnectionString("DefaultConnection"), _mySqlServerVersion); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

    }
}