using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Programa
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La facultad es obligatoria")]
    [MaxLength(100, ErrorMessage = "La facultad no puede exceder 100 caracteres")]
    public string Facultad { get; set; } = string.Empty;
    
    [Range(1, 500, ErrorMessage = "Los créditos totales deben estar entre 1 y 500")]
    public int CreditosTotales { get; set; }
}