using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services
{
    public class TipoMovimientoService
    {
        private readonly ITipoMovimientoRepository _repository;

        public TipoMovimientoService(ITipoMovimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoMovimientoResponseDto>> GetAllAsync()
        {
            var tipos = await _repository.GetAllAsync();
            return tipos.Select(t => new TipoMovimientoResponseDto
            {
                Id = t.Id.Value,
                Nombre = t.Nombre.Value
            });
        }

        public async Task<TipoMovimientoResponseDto?> GetByIdAsync(Guid id)
        {
            var tipo = await _repository.GetByIdAsync(id);
            if (tipo == null) return null;

            return new TipoMovimientoResponseDto
            {
                Id = tipo.Id.Value,
                Nombre = tipo.Nombre.Value
            };
        }

        public async Task<bool> UpdateAsync(Guid id, TipoMovimientoRequestDto dto)
        {
            var tipo = await _repository.GetByIdAsync(id);
            if (tipo == null) return false;

            tipo.Nombre = new NombreVO(dto.Nombre);
            await _repository.UpdateAsync(tipo);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tipo = await _repository.GetByIdAsync(id);
            if (tipo == null) return false;

            await _repository.DeleteAsync(tipo.Id.Value);
            return true;
        }

        public async Task<TipoMovimientoResponseDto> CreateAsync(DTOs.TipoMovimientoRequestDto dto)
        {
            var tipo = new TipoMovimiento(
            new IdVO(Guid.NewGuid()),
            new NombreVO(dto.Nombre)
            );

            await _repository.AddAsync(tipo);

            return new TipoMovimientoResponseDto
            {
                Id = tipo.Id.Value,
                Nombre = tipo.Nombre.Value
            };
        }
    }
}