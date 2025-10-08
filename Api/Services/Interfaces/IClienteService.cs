using Api.DTOs.Clientes;
using Domain.Entities;

namespace Api.Services.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto> GetByIdAsync(int id);
        Task<ClienteDto> CreateAsync(CreateClienteDto dto);
        Task<ClienteDto> UpdateAsync(int id, UpdateClienteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
