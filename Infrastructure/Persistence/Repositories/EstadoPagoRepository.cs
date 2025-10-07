using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class EstadoPagoRepository : IEstadoPagoRepository
    {
        private readonly AppDbContext _context;
    
        public EstadoPagoRepository(AppDbContext context)
        {
            _context = context;
        }
    
        public async Task AddAsync(EstadoPago estadoPago)
        {
            await _context.EstadosPago.AddAsync(estadoPago);
            await _context.SaveChangesAsync();
        }
    
        public async Task<IEnumerable<EstadoPago>> GetAllAsync()
        {
            return await _context.EstadosPago.ToListAsync();
        }
    
        public async Task<EstadoPago?> GetByIdAsync(Guid id)
        {
            return await _context.EstadosPago
                .FirstOrDefaultAsync(e => e.Id.Value == id);
        }
    
        public async Task UpdateAsync(EstadoPago estadoPago)
        {
            _context.EstadosPago.Update(estadoPago);
            await _context.SaveChangesAsync();
        }
    
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.EstadosPago.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}