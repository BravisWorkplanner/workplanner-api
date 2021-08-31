using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityConfigurations
{
    public class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.ToTable("Workers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("WorkerId");
        }
    }
}