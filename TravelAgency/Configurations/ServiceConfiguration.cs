using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.Trips).WithMany(t => t.Services);
            builder.HasMany(s => s.TripsAdditional).WithMany(t => t.AdditionalServices);
            builder.HasMany(s => s.Orders).WithMany(o => o.AdditionalServices);
        }
    }
}
