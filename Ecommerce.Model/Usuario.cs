using System;
using System.Collections.Generic;

namespace Ecommerce.Modelo;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
