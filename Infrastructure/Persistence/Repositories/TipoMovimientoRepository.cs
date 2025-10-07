using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TipoMovimientoRepository : ITipoMovimientoRepository
    {
        private readonly AppDbContext _context;

        public TipoMovimientoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoMovimiento>> GetAllAsync()
        {
            return await _context.TiposMovimiento.ToListAsync();
        }

        public async Task<TipoMovimiento?> GetByIdAsync(Guid id)
        {
            return await _context.TiposMovimiento.FirstOrDefaultAsync(t => t.Id.Value == id);
        }

        public async Task AddAsync(TipoMovimiento entity)
        {
            await _context.TiposMovimiento.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TipoMovimiento entity)
        {
            _context.TiposMovimiento.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.TiposMovimiento.FirstOrDefaultAsync(t => t.Id.Value == id);
            if (entity != null)
            {
                _context.TiposMovimiento.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}