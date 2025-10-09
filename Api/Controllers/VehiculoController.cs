using Api.DTOs.Vehiculos;
using AutoMapper;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class VehiculosController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VehiculosController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    // GET: api/vehiculos/all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetAll(CancellationToken ct)
    {
        var vehiculos = await _unitOfWork.Vehiculos.GetAllAsync(ct);
        var dto = _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        return Ok(dto);
    }

    // GET: api/vehiculos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehiculoDetailDto>> GetById(int id, CancellationToken ct)
    {
        var vehiculo = await _unitOfWork.Vehiculos.GetByIdAsync(new IdVO(id), ct);
        if (vehiculo is null) return NotFound(new { message = $"No se encontró un vehículo con ID {id}" });

        var dto = _mapper.Map<VehiculoDetailDto>(vehiculo);
        return Ok(dto);
    }

    // GET: api/vehiculos/vin/{vin}
    [HttpGet("vin/{vin}")]
    public async Task<ActionResult<VehiculoDetailDto>> GetByVin(string vin, CancellationToken ct)
    {
        var vinVo = new VinVO(vin);
        var vehiculo = await _unitOfWork.Vehiculos.GetByVinAsync(vinVo, ct);
        if (vehiculo is null) return NotFound(new { message = $"No se encontró un vehículo con VIN {vin}" });

        var dto = _mapper.Map<VehiculoDetailDto>(vehiculo);
        return Ok(dto);
    }

    // GET: api/vehiculos/cliente/{clienteId}
    [HttpGet("cliente/{clienteId:int}")]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetByClienteId(int clienteId, CancellationToken ct)
    {
        var vehiculos = await _unitOfWork.Vehiculos.GetByClienteIdAsync(new IdVO(clienteId), ct);
        var dto = _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        return Ok(dto);
    }

    // POST: api/vehiculos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehiculoDto body, CancellationToken ct)
    {
        var vinVo = new VinVO(body.Vin);
        if (await _unitOfWork.Vehiculos.ExistsByVinAsync(vinVo, ct))
            return Conflict(new { message = "Ya existe un vehículo con este VIN" });

        var vehiculo = new Vehiculo
        {
            ClienteId = new IdVO(body.ClienteId),
            Marca = new NombreVO(body.Marca),
            Modelo = new NombreVO(body.Modelo),
            Anio = new AnioVehiculoVO(body.Anio),
            Vin = vinVo,
            Kilometraje = new KilometrajeVO(body.Kilometraje)
        };

        await _unitOfWork.Vehiculos.AddAsync(vehiculo, ct);

        var dto = _mapper.Map<VehiculoDto>(vehiculo);
        return CreatedAtAction(nameof(GetById), new { id = vehiculo.Id.Value }, dto);
    }

    // PUT: api/vehiculos/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehiculoDto body, CancellationToken ct)
    {
        var vehiculo = await _unitOfWork.Vehiculos.GetByIdAsync(new IdVO(id), ct);
        if (vehiculo is null)
            return NotFound(new { message = $"No se encontró un vehículo con ID {id}" });

        // Mapeo manual para evitar sobrescribir relaciones
        vehiculo.Marca = new NombreVO(body.Marca);
        vehiculo.Modelo = new NombreVO(body.Modelo);
        vehiculo.Anio = new AnioVehiculoVO(body.Anio);
        vehiculo.Vin = new VinVO(body.Vin);
        vehiculo.Kilometraje = new KilometrajeVO(body.Kilometraje);

        var updated = await _unitOfWork.Vehiculos.UpdateAsync(vehiculo, ct);
        if (!updated)
            return StatusCode(500, new { message = "Error actualizando el vehículo" });

        return NoContent();
    }

    // DELETE: api/vehiculos/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.Vehiculos.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound(new { message = $"No se encontró un vehículo con ID {id}" });

        return NoContent();
    }
}
