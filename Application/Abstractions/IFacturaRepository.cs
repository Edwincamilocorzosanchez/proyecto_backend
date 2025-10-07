using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IFacturaRepository
    {
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<Factura?> GetByIdAsync(Guid id);
        Task AddAsync(Factura factura);
        Task UpdateAsync(Factura factura);
        Task DeleteAsync(Guid id);
    }
}