using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IAdministradorRepository
    {
        Task AddAsync(Administrador administrador);
        Task<IEnumerable<Administrador>> GetAllAsync();
        Task<Administrador?> GetByIdAsync(Guid id);
        Task UpdateAsync(Administrador administrador);
        Task DeleteAsync(Guid id);
    }
}