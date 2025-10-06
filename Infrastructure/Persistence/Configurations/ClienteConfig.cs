using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(v => v.Value, v => new IdVO(v))
            .HasColumnName("id");

        builder.Property(c => c.Telefono)
            .HasConversion(v => v.Value, v => new TelefonoVO(v))
            .HasColumnName("telefono")
            .HasMaxLength(20);

        builder.Property(c => c.Direccion)
            .HasConversion(v => v.Value, v => new DireccionVO(v))
            .HasColumnName("direccion")
            .HasMaxLength(255);

        builder.Property(c => c.IsActive)
            .HasConversion(v => v.Value, v => new EstadoVO(v))
            .HasColumnName("is_active")
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Cliente>(c => c.Id)
            .HasConstraintName("fk_cliente_user")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
