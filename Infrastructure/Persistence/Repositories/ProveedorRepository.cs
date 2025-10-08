using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _context;

        public ProveedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Proveedor proveedor, CancellationToken ct = default)
        {
            await _context.Proveedores.AddAsync(proveedor, ct);
            await _context.SaveChangesAsync(ct);
            return proveedor.Id.Value.GetHashCode();
        }

        public async Task<bool> UpdateAsync(Proveedor proveedor, CancellationToken ct = default)
        {
            _context.Proveedores.Update(proveedor);
            var updated = await _context.SaveChangesAsync(ct);
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id.Value == id.Value, ct);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            var deleted = await _context.SaveChangesAsync(ct);
            return deleted > 0;
        }
        
        public async Task<bool> ExistsByNombreAsync(NombreVO nombre, CancellationToken ct = default)
        {
            return await _context.Proveedores
                .AnyAsync(p => p.Nombre == nombre, ct);
        }

        public async Task<IReadOnlyList<Proveedor>> GetActivosAsync(CancellationToken ct = default)
        {
            return await _context.Proveedores
                .Where(p => p.IsActive.Value)
                .Include(p => p.Repuestos)
                .ToListAsync(ct);
        }

        public async Task<Proveedor?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        {
            return await _context.Proveedores
                .Include(p => p.Repuestos)
                .FirstOrDefaultAsync(p => p.Id.Value == id.Value, ct);
        }

        public async Task<Proveedor?> GetByNombreAsync(NombreVO nombre, CancellationToken ct = default)
        {
            return await _context.Proveedores
                .Include(p => p.Repuestos)
                .FirstOrDefaultAsync(p => p.Nombre == nombre, ct);
        }

        public async Task<IReadOnlyList<Proveedor>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Proveedores
                .Include(p => p.Repuestos)
                .ToListAsync(ct);
        }
    }
}
