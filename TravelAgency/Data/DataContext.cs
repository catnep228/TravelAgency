using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Data
{
    public class DataContext : DbContext

    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Passanger> Passangers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TypeOfHoliday> TypeOfHolidays { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}
