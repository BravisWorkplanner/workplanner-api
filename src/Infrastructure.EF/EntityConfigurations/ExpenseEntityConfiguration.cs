using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityConfigurations
{
    public class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ExpenseId");

            builder.HasOne(x => x.Order).WithMany(x => x.Expenses).OnDelete(DeleteBehavior.Cascade);
        }
    }
}