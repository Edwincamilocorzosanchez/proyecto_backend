using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Api.DTOs.Administradores;
using Api.Services.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AdministradoresController : BaseApiController
{
    private readonly IAdministradorService _service;
    private readonly IMapper _mapper;

    public AdministradoresController(IAdministradorService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AdministradorDto>>> GetAll(CancellationToken ct)
    {
        var admins = await _service.GetAllAsync(ct);
        var dto = _mapper.Map<IEnumerable<AdministradorDto>>(admins);
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdministradorDto>> GetById(int id, CancellationToken ct)
    {
        try 
        {
            var admin = await _service.GetByIdAsync(new IdVO(id), ct);
            if (admin is null) return NotFound();
            return Ok(_mapper.Map<AdministradorDto>(admin));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdministradorDto body, CancellationToken ct)
    {
        try
        {
            var admin = _mapper.Map<Administrador>(body);
            var id = await _service.AddAsync(admin, ct);

            var dto = _mapper.Map<AdministradorDto>(admin);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAdministradorDto body, CancellationToken ct)
    {
        var existing = await _service.GetByIdAsync(new IdVO(id), ct);
        if (existing is null) return NotFound();

        // Map parcial con value objects
        if (body.Nombre != null) existing.Nombre = new NombreVO(body.Nombre);
        if (body.Telefono != null) existing.Telefono = new TelefonoVO(body.Telefono);
        if (body.NivelAcceso != null) existing.NivelAcceso = new NivelAccesoVO(body.NivelAcceso);
        if (body.AreaResponsabilidad != null) existing.AreaResponsabilidad = new DescripcionVO(body.AreaResponsabilidad);
        if (body.IsActive.HasValue) existing.IsActive = new EstadoVO(body.IsActive.Value);

        try
        {
            var updated = await _service.UpdateAsync(existing, ct);
            if (!updated) return StatusCode(500, new { message = "Error actualizando administrador" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(new IdVO(id), ct);
        if (!deleted) return NotFound();

        return NoContent();
    }

    [HttpGet("nivel/{nivel}")]
    public async Task<ActionResult<IEnumerable<AdministradorDto>>> GetByNivelAcceso(string nivel, CancellationToken ct)
    {
        var all = await _service.GetAllAsync(ct);
        var filtered = all.Where(a => a.NivelAcceso.Value.Equals(nivel, StringComparison.OrdinalIgnoreCase));

        var dto = _mapper.Map<IEnumerable<AdministradorDto>>(filtered);
        return Ok(dto);
    }
}
