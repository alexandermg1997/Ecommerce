using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Ingrese su correo")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su contraseña")]
        public string Clave { get; set; } = null!;
    }
}
