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
            builder.HasOne(u => u.Passanger).WithOne(p => p.user).HasForeignKey<User>(u => u.PassangerId);
            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
        }
    }
}
