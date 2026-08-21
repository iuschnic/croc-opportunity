using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Models;
using Domain.Models;

namespace Storage.Context.Configurations;

public class OpportunityDbConfiguration : IEntityTypeConfiguration<OpportunityDb>
{
    public void Configure(EntityTypeBuilder<OpportunityDb> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .ValueGeneratedNever();
        builder.Property(o => o.Status)
            .HasConversion<int>();
        builder.Property(o => o.CreatedAt)
            .HasColumnType("timestamp with time zone");
        builder.Property(o => o.LossReason)
            .HasMaxLength(Opportunity.LossReasonMaxLength);
        builder.Property(o => o.Currency)
            .HasConversion<int>();
        builder.HasMany(c => c.Items)
            .WithOne(i => i.Opportunity)
            .HasForeignKey(i => i.OpportunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}