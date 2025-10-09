using Api.DTOs.TiposServicio;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TipoServicioController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoServicioController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ============================================================
    // GET /api/tiposervicio
    // ============================================================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoServicioDto>>> GetAll(CancellationToken ct)
    {
        var tipos = await _unitOfWork.TipoServicio.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<TipoServicioDto>>(tipos);
        return Ok(result);
    }

    // ============================================================
    // GET /api/tiposervicio/{id}
    // ============================================================
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TipoServicioDetailDto>> GetById(int id, CancellationToken ct)
    {
        var tipo = await _unitOfWork.TipoServicio.GetByIdAsync(new IdVO(id), ct);
        if (tipo is null)
            return NotFound($"No se encontró el tipo de servicio con ID {id}");

        var dto = _mapper.Map<TipoServicioDetailDto>(tipo);
        return Ok(dto);
    }

    // ============================================================
    // GET /api/tiposervicio/nombre/{nombre}
    // ============================================================
    [HttpGet("nombre/{nombre}")]
    public async Task<ActionResult<TipoServicioDetailDto>> GetByNombre(string nombre, CancellationToken ct)
    {
        var tipo = await _unitOfWork.TipoServicio.GetByNombreAsync(new NombreVO(nombre), ct);
        if (tipo is null)
            return NotFound($"No se encontró el tipo de servicio con nombre '{nombre}'");

        var dto = _mapper.Map<TipoServicioDetailDto>(tipo);
        return Ok(dto);
    }

    // ============================================================
    // POST /api/tiposervicio
    // ============================================================
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTipoServicioDto dto, CancellationToken ct)
    {
        var tipo = _mapper.Map<TipoServicio>(dto);
        await _unitOfWork.TipoServicio.AddAsync(tipo, ct);
        await _unitOfWork.SaveChanges(ct);

        return CreatedAtAction(nameof(GetById),
            new { id = tipo.Id.Value },
            new { tipo.Id.Value });
    }

    // ============================================================
    // PUT /api/tiposervicio/{id}
    // ============================================================
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTipoServicioDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.TipoServicio.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el tipo de servicio con ID {id}");

        _mapper.Map(dto, existing);
        await _unitOfWork.TipoServicio.UpdateAsync(existing, ct);
        await _unitOfWork.SaveChanges(ct);

        return NoContent();
    }

    // ============================================================
    // DELETE /api/tiposervicio/{id}
    // ============================================================
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.TipoServicio.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el tipo de servicio con ID {id}");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
