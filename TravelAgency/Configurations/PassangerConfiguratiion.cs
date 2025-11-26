using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class PassangerConfiguratiion : IEntityTypeConfiguration<Passanger>
    {
        public void Configure(EntityTypeBuilder<Passanger> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.Orders).WithMany(o => o.Passangers);
            builder.HasOne(p => p.user).WithOne(u => u.passanger).HasForeignKey<Passanger>(p => p.userId);

        }
    }
}
