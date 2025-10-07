using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RepuestoRepository : IRepuestoRepository
    {
        private readonly AppDbContext _context;

        public RepuestoRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task UpdateAsync(Repuesto repuesto)
        {
            _context.Repuestos.Update(repuesto);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Repuesto>> GetAllAsync()
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .ToListAsync();
        }

        public async Task<Repuesto?> GetByIdAsync(Guid id)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedor)
                .FirstOrDefaultAsync(r => r.Id.Value == id);
        }

        public async Task AddAsync(Repuesto repuesto)
        {
            await _context.Repuestos.AddAsync(repuesto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repuesto = await _context.Repuestos.FirstOrDefaultAsync(r => r.Id.Value == id);
            if (repuesto != null)
            {
                _context.Repuestos.Remove(repuesto);
                await _context.SaveChangesAsync();
            }
        }


    }
}