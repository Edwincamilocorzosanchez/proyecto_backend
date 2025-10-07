using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IOrdenServicioRepository
    {
        Task AddAsync(OrdenServicio orden);
        Task<IEnumerable<OrdenServicio>> GetAllAsync();
        Task<OrdenServicio?> GetByIdAsync(Guid id);
        Task UpdateAsync(OrdenServicio orden);
        Task DeleteAsync(Guid id);
    }
}