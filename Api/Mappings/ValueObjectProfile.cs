using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.ValueObjects;

namespace Api.Mappings
{
public sealed class ValueObjectProfile : Profile
{
    public ValueObjectProfile()
    {
        // Conversión simple de VO → primitivo
        CreateMap<IdVO, int>().ConvertUsing(vo => vo.Value);
        CreateMap<CantidadVO, int>().ConvertUsing(vo => vo.Value);
        CreateMap<DineroVO, decimal>().ConvertUsing(vo => vo.Value);
        CreateMap<DescripcionVO, string>().ConvertUsing(vo => vo.Value);
        CreateMap<FechaCitaVO, DateTime>().ConvertUsing(vo => vo.Value);

        // Si tienes más VO, agrégalos aquí
    }
}
}