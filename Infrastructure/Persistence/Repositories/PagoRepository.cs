using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly AppDbContext _context;

        public PagoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pago>> GetAllAsync()
        {
            return await _context.Pagos
                .Include(p => p.MetodoPago)
                .Include(p => p.EstadoPago)
                .Include(p => p.Factura)
                .ToListAsync();
        }

        public async Task<Pago?> GetByIdAsync(Guid id)
        {
            return await _context.Pagos
                .Include(p => p.MetodoPago)
                .Include(p => p.EstadoPago)
                .Include(p => p.Factura)
                .FirstOrDefaultAsync(p => p.Id.Value == id);
        }

        public async Task AddAsync(Pago pago)
        {
            await _context.Pagos.AddAsync(pago);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Pagos.FirstOrDefaultAsync(p => p.Id.Value == id);
            if (entity != null)
            {
                _context.Pagos.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}