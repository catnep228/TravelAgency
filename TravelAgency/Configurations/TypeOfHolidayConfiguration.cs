using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class TypeOfHolidayConfiguration : IEntityTypeConfiguration<TypeOfHoliday>
    {
        public void Configure(EntityTypeBuilder<TypeOfHoliday> builder)
        {
            builder.HasKey(th => th.Id);
            builder.HasMany(th => th.Trips).WithOne(t => t.typeOfHoliday).HasForeignKey(t => t.typeofHoldayId);
        }
    }
}
