using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly AppDbContext _context;

        public MetodoPagoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MetodoPago>> GetAllAsync()
        {
            return await _context.MetodosPago.ToListAsync();
        }

        public async Task<MetodoPago?> GetByIdAsync(Guid id)
        {
            return await _context.MetodosPago.FirstOrDefaultAsync(m => m.Id.Value == id);
        }

        public async Task AddAsync(MetodoPago metodo)
        {
            await _context.MetodosPago.AddAsync(metodo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetodoPago metodo)
        {
            _context.MetodosPago.Update(metodo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.MetodosPago.FirstOrDefaultAsync(m => m.Id.Value == id);
            if (entity != null)
            {
                _context.MetodosPago.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}