using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EstadoOrdenConfig : IEntityTypeConfiguration<EstadoOrden>
{
    public void Configure(EntityTypeBuilder<EstadoOrden> builder)
    {
        builder.ToTable("estados_orden");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new(value))
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nombre)
            .HasConversion(n => n.Value, value => new(value))
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();
    }
}
