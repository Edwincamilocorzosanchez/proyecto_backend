using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MecanicoConfig : IEntityTypeConfiguration<Mecanico>
{
    public void Configure(EntityTypeBuilder<Mecanico> builder)
    {
        builder.ToTable("mecanicos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(id => id.Value, value => new(value))
            .HasColumnName("id");

        builder.Property(m => m.Nombre)
            .HasConversion(n => n.Value, value => new(value))
            .HasColumnName("nombre")
            .HasMaxLength(100);

        builder.Property(m => m.Telefono)
            .HasConversion(t => t == null ? null : t.Value,
                            value => value == null ? null : new(value))
            .HasColumnName("telefono")
            .HasMaxLength(20);

        builder.Property(m => m.Especialidad)
            .HasConversion(e => e == null ? null : e.Value,
                            value => value == null ? null : new(value))
            .HasColumnName("especialidad")
            .HasMaxLength(60);

        builder.Property(m => m.IsActive)
            .HasConversion(a => a.Value, value => new(value))
            .HasColumnName("is_active");

        builder.HasOne(m => m.User)
            .WithOne()
            .HasForeignKey<Mecanico>(m => m.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
    }
}
