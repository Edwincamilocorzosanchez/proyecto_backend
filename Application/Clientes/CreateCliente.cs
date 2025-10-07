
using Domain.ValueObjects;
using MediatR;

namespace Application.Clientes;

public sealed record CreateCliente(
    TelefonoVO Telefono,
    DireccionVO Direccion,
    EstadoVO IsActive
) : IRequest<IdVO>;
