using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class UpdateRepuestoDto : CreateRepuestoDto
    {
         public Guid Id { get; set; }
    }
}