using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Models;

namespace TravelAgency.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>

    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u  => u.Id);
            builder.HasOne(u => u.passanger).WithOne(p => p.user);
            builder.HasOne(u => u.role).WithMany(r => r.Users).HasForeignKey(u => u.roleId);
        }
    }
}
