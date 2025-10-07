using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface ITipoMovimientoRepository : IGenericRepository<TipoMovimiento>
    {
        Task<IEnumerable<TipoMovimiento>> GetAllAsync();
        Task<TipoMovimiento?> GetByIdAsync(Guid id);
        Task AddAsync(TipoMovimiento entity);
        Task UpdateAsync(TipoMovimiento entity);
        Task DeleteAsync(Guid id);
    }

    public interface IGenericRepository<T>
    {
    }
}