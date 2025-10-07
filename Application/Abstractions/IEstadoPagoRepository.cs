using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IEstadoPagoRepository
    {
        Task AddAsync(EstadoPago estadoPago);
        Task<IEnumerable<EstadoPago>> GetAllAsync();
        Task<EstadoPago?> GetByIdAsync(Guid id);
        Task UpdateAsync(EstadoPago estadoPago);
        Task DeleteAsync(Guid id);
    }
}