using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasMany(t => t.Services).WithMany(s => s.Trips);
            builder.HasMany(t => t.AdditionalServices).WithMany(s => s.TripsAdditional);
            builder.HasOne(t => t.typeOfHoliday).WithMany(th => th.Trips).HasForeignKey(t => t.typeofHoldayId);
            builder.HasMany(t => t.Orders).WithOne(o => o.trip).HasForeignKey(o => o.tripId);
        }
    }
}

