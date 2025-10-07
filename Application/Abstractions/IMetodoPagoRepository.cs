using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application
{
    public interface IMetodoPagoRepository
    {
        Task<IEnumerable<MetodoPago>> GetAllAsync();
        Task<MetodoPago?> GetByIdAsync(Guid id);
        Task AddAsync(MetodoPago metodo);
        Task UpdateAsync(MetodoPago metodo);
        Task DeleteAsync(Guid id);
    }
}