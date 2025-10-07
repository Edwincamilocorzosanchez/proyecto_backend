using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class OrdenServicioRepository : IOrdenServicioRepository
    {
        private readonly AppDbContext _context;

        public OrdenServicioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrdenServicio orden)
        {
            await _context.OrdenesServicio.AddAsync(orden);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrdenServicio>> GetAllAsync()
        {
            return await _context.OrdenesServicio
                .Include(o => o.Vehiculo)
                .Include(o => o.Mecanico)
                .Include(o => o.TipoServicio)
                .Include(o => o.Estado)
                .ToListAsync();
        }

        public async Task<OrdenServicio?> GetByIdAsync(Guid id)
        {
            return await _context.OrdenesServicio
                .Include(o => o.Vehiculo)
                .Include(o => o.Mecanico)
                .Include(o => o.TipoServicio)
                .Include(o => o.Estado)
                .FirstOrDefaultAsync(o => o.Id.Value == id);
        }

        public async Task UpdateAsync(OrdenServicio orden)
        {
            _context.OrdenesServicio.Update(orden);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var orden = await GetByIdAsync(id);
            if (orden != null)
            {
                _context.OrdenesServicio.Remove(orden);
                await _context.SaveChangesAsync();
            }
        }
    }
}