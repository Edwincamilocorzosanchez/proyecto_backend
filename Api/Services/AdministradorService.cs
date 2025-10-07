using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services
{

    public class AdministradorService
    {
        private readonly IAdministradorRepository _repository;

        public AdministradorService(IAdministradorRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdministradorResponseDto> CrearAsync(AdministradorRequestDto dto)
        {
            var admin = new Administrador(
                new IdVO(Guid.NewGuid()),
                new NombreVO(dto.Nombre),
                string.IsNullOrEmpty(dto.Telefono) ? null : new TelefonoVO(dto.Telefono),
                string.IsNullOrEmpty(dto.NivelAcceso) ? null : new NivelAccesoVO(dto.NivelAcceso),
                string.IsNullOrEmpty(dto.AreaResponsabilidad) ? null : new DescripcionVO(dto.AreaResponsabilidad),
                new EstadoVO(dto.IsActive)
            );

            await _repository.AddAsync(admin);

            return new AdministradorResponseDto
            {
                Id = admin.Id.Value,
                Nombre = admin.Nombre.Value,
                Telefono = admin.Telefono?.Value,
                NivelAcceso = admin.NivelAcceso?.Value,
                AreaResponsabilidad = admin.AreaResponsabilidad?.Value,
                IsActive = admin.IsActive.Value
            };
        }

        public async Task<IEnumerable<AdministradorResponseDto>> ObtenerTodosAsync()
        {
            var admins = await _repository.GetAllAsync();

            return admins.Select(a => new AdministradorResponseDto
            {
                Id = a.Id.Value,
                Nombre = a.Nombre.Value,
                Telefono = a.Telefono?.Value,
                NivelAcceso = a.NivelAcceso?.Value,
                AreaResponsabilidad = a.AreaResponsabilidad?.Value,
                IsActive = a.IsActive.Value
            });
        }

        public async Task<AdministradorResponseDto?> ObtenerPorIdAsync(Guid id)
        {
            var admin = await _repository.GetByIdAsync(id);
            if (admin == null) return null;

            return new AdministradorResponseDto
            {
                Id = admin.Id.Value,
                Nombre = admin.Nombre.Value,
                Telefono = admin.Telefono?.Value,
                NivelAcceso = admin.NivelAcceso?.Value,
                AreaResponsabilidad = admin.AreaResponsabilidad?.Value,
                IsActive = admin.IsActive.Value
            };
        }

        public async Task ActualizarAsync(Guid id, AdministradorRequestDto dto)
        {
            var admin = await _repository.GetByIdAsync(id);
            if (admin == null)
                throw new Exception("Administrador no encontrado.");

            admin.Nombre = new NombreVO(dto.Nombre);
            admin.Telefono = string.IsNullOrEmpty(dto.Telefono) ? null : new TelefonoVO(dto.Telefono);
            admin.NivelAcceso = string.IsNullOrEmpty(dto.NivelAcceso) ? null : new NivelAccesoVO(dto.NivelAcceso);
            admin.AreaResponsabilidad = string.IsNullOrEmpty(dto.AreaResponsabilidad) ? null : new DescripcionVO(dto.AreaResponsabilidad);
            admin.IsActive = new EstadoVO(dto.IsActive);

            await _repository.UpdateAsync(admin);
        }

        public async Task EliminarAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}
