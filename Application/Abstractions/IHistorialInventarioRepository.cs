using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IHistorialInventarioRepository
    {
        Task<IEnumerable<HistorialInventario>> GetAllAsync();
        Task<HistorialInventario?> GetByIdAsync(Guid id);
        Task AddAsync(HistorialInventario historial);
        Task UpdateAsync(HistorialInventario historial);
        Task DeleteAsync(Guid id);
    }
}