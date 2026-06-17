using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BoletaConfiguration : IEntityTypeConfiguration<Boleta>
{
    public void Configure(EntityTypeBuilder<Boleta> builder)
    {
        builder.ToTable("boletas");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(b => b.TalonarioId).HasColumnName("talonario_id");
        builder.Property(b => b.Number).HasColumnName("number");
        builder.Property(b => b.BuyerName).HasColumnName("buyer_name");
        builder.Property(b => b.BuyerPhone).HasColumnName("buyer_phone");
        builder.Property(b => b.BuyerAddress).HasColumnName("buyer_address");
        builder.Property(b => b.Sold).HasColumnName("sold").HasDefaultValue(false);

        builder.HasIndex(b => new { b.TalonarioId, b.Number }).IsUnique();
        builder.HasIndex(b => b.TalonarioId);
        builder.HasIndex(b => b.Sold);
    }
}
