using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.DTOs
{
    public class MetodoPagoService
    {
        private readonly IMetodoPagoRepository _repository;
        private readonly IMapper _mapper;

        public MetodoPagoService(IMetodoPagoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MetodoPagoResponseDto>> GetAllAsync()
        {
            var metodos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MetodoPagoResponseDto>>(metodos);
        }

        public async Task<MetodoPagoResponseDto?> GetByIdAsync(Guid id)
        {
            var metodo = await _repository.GetByIdAsync(id);
            return metodo is null ? null : _mapper.Map<MetodoPagoResponseDto>(metodo);
        }

        public async Task AddAsync(MetodoPagoRequestDto dto)
        {
            var metodo = _mapper.Map<MetodoPago>(dto);
            await _repository.AddAsync(metodo);
        }

        public async Task UpdateAsync(Guid id, MetodoPagoRequestDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Método de pago no encontrado.");

            existing.Nombre = new NombreVO(dto.Nombre);

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}