
using FluentValidation;

namespace Application.Clientes;

public class CreateClienteValidator : AbstractValidator<CreateCliente>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.Telefono).NotNull();
        RuleFor(x => x.Direccion).NotNull();
        RuleFor(x => x.IsActive).NotNull();
    }
}
