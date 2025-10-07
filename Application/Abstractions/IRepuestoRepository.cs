using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IRepuestoRepository
    {
        Task<IEnumerable<Repuesto>> GetAllAsync();
        Task<Repuesto?> GetByIdAsync(Guid id);
        Task AddAsync(Repuesto repuesto);
        Task UpdateAsync(Repuesto repuesto);
        Task DeleteAsync(Guid id);
    }
}