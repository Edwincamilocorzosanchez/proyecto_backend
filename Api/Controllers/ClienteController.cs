using Api.DTOs.Clientes;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClienteController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ============================================================
    // GET /api/cliente
    // ============================================================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll(CancellationToken ct)
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        return Ok(result);
    }

    // ============================================================
    // GET /api/cliente/{id}
    // ============================================================
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id, CancellationToken ct)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(new IdVO(id), ct);
        if (cliente is null)
            return NotFound($"No se encontró el cliente con ID {id}");

        var dto = _mapper.Map<ClienteDto>(cliente);
        return Ok(dto);
    }

    // ============================================================
    // GET /api/cliente/user/{userId}
    // ============================================================
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<ClienteDto>> GetByUserId(int userId, CancellationToken ct)
    {
        var cliente = await _unitOfWork.Clientes.GetByUserIdAsync(new IdVO(userId), ct);
        if (cliente is null)
            return NotFound($"No se encontró el cliente con UserId {userId}");

        var dto = _mapper.Map<ClienteDto>(cliente);
        return Ok(dto);
    }

    // ============================================================
    // POST /api/cliente
    // ============================================================
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateClienteDto dto, CancellationToken ct)
    {
        // Verificar existencia de correo
        var exists = await _unitOfWork.Clientes.ExistsByEmailAsync(new CorreoVO(dto.Correo), ct);
        if (exists)
            return Conflict("Ya existe un cliente registrado con ese correo.");

        var cliente = new Cliente(
            new IdVO(0),
            new NombreVO(dto.Nombre),
            new CorreoVO(dto.Correo),
            new TelefonoVO(dto.Telefono),
            new DireccionVO(dto.Direccion),
            new EstadoVO(dto.IsActive),
            dto.UserId
        );

        await _unitOfWork.Clientes.AddAsync(cliente, ct);
        await _unitOfWork.SaveChanges(ct);

        var createdDto = _mapper.Map<ClienteDto>(cliente);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id.Value }, createdDto);
    }

    // ============================================================
    // PUT /api/cliente/{id}
    // ============================================================
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateClienteDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.Clientes.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el cliente con ID {id}");

        // Actualización de propiedades
        existing.Nombre = new NombreVO(dto.Nombre);
        existing.Correo = new CorreoVO(dto.Correo);
        existing.Telefono = new TelefonoVO(dto.Telefono);
        existing.Direccion = new DireccionVO(dto.Direccion);
        existing.IsActive = new EstadoVO(dto.IsActive);

        var updated = await _unitOfWork.Clientes.UpdateAsync(existing, ct);
        if (!updated)
            return BadRequest("No se pudo actualizar el cliente.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }

    // ============================================================
    // DELETE /api/cliente/{id}
    // ============================================================
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.Clientes.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el cliente con ID {id}");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
