using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Models;
using Domain.Models;

namespace Storage.Context.Configurations;

public class OpportunityItemDbConfiguration : IEntityTypeConfiguration<OpportunityItemDb>
{
    public void Configure(EntityTypeBuilder<OpportunityItemDb> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedNever();
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(OpportunityItem.NameMaxLength);
        builder.Property(i => i.PricePerUnit)
            .HasColumnType("numeric(18,2)");
        builder.Property(i => i.Discount)
            .HasColumnType("numeric(18,2)");
    }
}