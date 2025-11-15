using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.AdditionalServices).WithMany(s => s.Orders);
            builder.HasOne(o => o.trip).WithMany(t => t.Orders).HasForeignKey(o => o.tripId);
        }
    }
}
