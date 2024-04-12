using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DTO
{
    public class SesionDTO
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Rol { get; set; } = null!;
    }
}
