using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese su nombre completo")]
        public string NombreCompleto { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su correo")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su contraseña")]
        public string Clave { get; set; } = null!;

        [Required(ErrorMessage = "Recuerde confirmar su contraseña")]
        public string ConfirmarClave { get; set; } = null!;

        public string Rol { get; set; } = null!;

    }
}
