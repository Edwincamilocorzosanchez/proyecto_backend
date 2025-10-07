using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly AppDbContext _context;

        public AdministradorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Administrador administrador)
        {
            await _context.Administradores.AddAsync(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Administrador>> GetAllAsync()
        {
            return await _context.Administradores
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<Administrador?> GetByIdAsync(Guid id)
        {
            return await _context.Administradores
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id.Value == id);
        }

        public async Task UpdateAsync(Administrador administrador)
        {
            _context.Administradores.Update(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var admin = await GetByIdAsync(id);
            if (admin != null)
            {
                _context.Administradores.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}