using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AplicacionPersonas.Models;

public partial class Persona
{
    public int Id { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string NumeroIdentificacion { get; set; } = null!;
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = null!;
    public string TipoIdentificacion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }
}
