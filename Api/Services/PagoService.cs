using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Application.DTOs;
using Api.DTOs;

namespace Application.Services
{
    public class PagoService
    {
        private readonly IPagoRepository _repository;
        private readonly IMapper _mapper;

        public PagoService(IPagoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PagoResponseDto>> GetAllAsync()
        {
            var pagos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PagoResponseDto>>(pagos);
        }

        public async Task<PagoResponseDto?> GetByIdAsync(Guid id)
        {
            var pago = await _repository.GetByIdAsync(id);
            return pago is null ? null : _mapper.Map<PagoResponseDto>(pago);
        }

        public async Task AddAsync(PagoRequestDto dto)
        {
            var pago = _mapper.Map<Pago>(dto);
            await _repository.AddAsync(pago);
        }

        public async Task UpdateAsync(Guid id, PagoRequestDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Pago no encontrado.");

            existing.Monto = new DineroVO(dto.Monto);
            existing.FechaPago = new FechaHistoricaVO(dto.FechaPago);
            existing.MetodoPagoId = new IdVO(dto.MetodoPagoId);
            existing.EstadoPagoId = new IdVO(dto.EstadoPagoId);

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}