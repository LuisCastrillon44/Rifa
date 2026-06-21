using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TalonarioConfiguration : IEntityTypeConfiguration<Talonario>
{
    public void Configure(EntityTypeBuilder<Talonario> builder)
    {
        builder.ToTable("talonarios");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(t => t.UserId).HasColumnName("user_id");
        builder.Property(t => t.Title).HasColumnName("title").IsRequired();
        builder.Property(t => t.Description).HasColumnName("description");
        builder.Property(t => t.BoletasNumber).HasColumnName("boletas_number");
        builder.Property(t => t.BoletasValue).HasColumnName("boletas_value").HasPrecision(18, 2);
        builder.Property(t => t.LotteryDate).HasColumnName("lottery_date");
        builder.Property(t => t.LotteryNumber).HasColumnName("lottery_number");
        builder.Property(t => t.Jackpot).HasColumnName("jackpot").HasMaxLength(255).IsRequired();

        builder.HasIndex(t => t.UserId);

        builder.HasMany(t => t.Boletas)
            .WithOne(b => b.Talonario)
            .HasForeignKey(b => b.TalonarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
