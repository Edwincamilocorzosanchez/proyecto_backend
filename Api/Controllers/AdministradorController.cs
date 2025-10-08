using System;
using AutoMapper;
using Api.DTOs.Administradores;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AdministradoresController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AdministradorDto>>> GetAll(CancellationToken ct)
        {
            var admins = await _unitOfWork.Admins.GetAllAsync(ct);
            var dto = _mapper.Map<IEnumerable<AdministradorDto>>(admins);
            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdministradorDto>> GetById(int id, CancellationToken ct)
        {
            var admin = await _unitOfWork.Admins.GetByIdAsync(new IdVO(id), ct);
            if (admin is null) return NotFound();

            return Ok(_mapper.Map<AdministradorDto>(admin));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministradorDto body, CancellationToken ct)
        {
            // Validación de unicidad por nombre
            var nombreVo = new NombreVO(body.Nombre);
            if (await _unitOfWork.Admins.ExistsByNombreAsync(nombreVo, ct))
                return Conflict(new { message = "Administrador con este nombre ya existe" });

            var admin = _mapper.Map<Administrador>(body);
            await _unitOfWork.Admins.AddAsync(admin, ct);

            var dto = _mapper.Map<AdministradorDto>(admin);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAdministradorDto body, CancellationToken ct)
        {
            var admin = await _unitOfWork.Admins.GetByIdAsync(new IdVO(id), ct);
            if (admin is null) return NotFound();

            // Map parcial
            if (body.Nombre != null) admin.Nombre = new NombreVO(body.Nombre);
            if (body.Telefono != null) admin.Telefono = new TelefonoVO(body.Telefono);
            if (body.NivelAcceso != null) admin.NivelAcceso = new NivelAccesoVO(body.NivelAcceso);
            if (body.AreaResponsabilidad != null) admin.AreaResponsabilidad = new DescripcionVO(body.AreaResponsabilidad);
            if (body.IsActive.HasValue) admin.IsActive = new EstadoVO(body.IsActive.Value);

            var updated = await _unitOfWork.Admins.UpdateAsync(admin, ct);
            if (!updated) return StatusCode(500, new { message = "Error actualizando administrador" });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _unitOfWork.Admins.DeleteAsync(new IdVO(id), ct);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpGet("nivel/{nivel}")]
        public async Task<ActionResult<IEnumerable<AdministradorDto>>> GetByNivelAcceso(string nivel, CancellationToken ct)
        {
            var nivelVo = new NivelAccesoVO(nivel);
            var admins = await _unitOfWork.Admins.GetByNivelAccesoAsync(nivelVo, ct);
            var dto = _mapper.Map<IEnumerable<AdministradorDto>>(admins);
            return Ok(dto);
        }
    }
}
