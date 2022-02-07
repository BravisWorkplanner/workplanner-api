using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityConfigurations
{
    public class TimeRegistrationEntityConfiguration : IEntityTypeConfiguration<TimeRegistration>
    {
        public void Configure(EntityTypeBuilder<TimeRegistration> builder)
        {
            builder.ToTable("TimeRegistrations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("TimeRegistrationId");

            builder.HasOne(x => x.Order).WithMany(x => x.TimeRegistrations).OnDelete(DeleteBehavior.Cascade);
        }
    }
}