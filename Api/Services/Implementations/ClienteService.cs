using Api.DTOs.Clientes;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto> GetByIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(new IdVO(id));
            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado");

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
        {
            // Aquí puedes poner validaciones de negocio:
            // Ej: correo único
            var exists = await _clienteRepository.ExistsByEmailAsync(new CorreoVO(dto.Correo));
            if (exists)
                throw new InvalidOperationException("Ya existe un cliente con este correo");

            var cliente = new Cliente
            {
                Id = new IdVO(0), // se asigna automáticamente en la DB
                Nombre = new NombreVO(dto.Nombre),
                Correo = new CorreoVO(dto.Correo),
                Telefono = new TelefonoVO(dto.Telefono),
                Direccion = new DireccionVO(dto.Direccion),
                IsActive = new EstadoVO(dto.IsActive),
                UserId = dto.UserId
            };

            await _clienteRepository.AddAsync(cliente);

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> UpdateAsync(int id, UpdateClienteDto dto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(new IdVO(id));
            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado");

            // Aplicar cambios
            cliente.Nombre = new NombreVO(dto.Nombre);
            cliente.Correo = new CorreoVO(dto.Correo);
            cliente.Telefono = new TelefonoVO(dto.Telefono);
            cliente.Direccion = new DireccionVO(dto.Direccion);
            cliente.IsActive = new EstadoVO(dto.IsActive);

            await _clienteRepository.UpdateAsync(cliente);

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(new IdVO(id));
            if (cliente == null)
                return false;

            await _clienteRepository.DeleteAsync(cliente.Id);
            return true;
        }
    }
}
