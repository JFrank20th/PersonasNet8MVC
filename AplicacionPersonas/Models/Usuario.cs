using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AplicacionPersonas.Models;

public partial class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public string? Pass { get; set; }

    public DateTime FechaCreacion { get; set; }
}
