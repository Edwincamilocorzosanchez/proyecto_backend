using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class HistorialInventarioRepository : IHistorialInventarioRepository
    {
        private readonly AppDbContext _context;

        public HistorialInventarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialInventario>> GetAllAsync()
        {
            return await _context.HistorialesInventario
                .Include(h => h.Repuesto)
                .Include(h => h.Administrador)
                .Include(h => h.TipoMovimiento)
                .ToListAsync();
        }

        public async Task<HistorialInventario?> GetByIdAsync(Guid id)
        {
            return await _context.HistorialesInventario
                .Include(h => h.Repuesto)
                .Include(h => h.Administrador)
                .Include(h => h.TipoMovimiento)
                .FirstOrDefaultAsync(h => h.Id.Value == id);
        }

        public async Task AddAsync(HistorialInventario historial)
        {
            await _context.HistorialesInventario.AddAsync(historial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HistorialInventario historial)
        {
            _context.HistorialesInventario.Update(historial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var historial = await _context.HistorialesInventario.FirstOrDefaultAsync(h => h.Id.Value == id);
            if (historial != null)
            {
                _context.HistorialesInventario.Remove(historial);
                await _context.SaveChangesAsync();
            }
        }
    }
}