using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Materia
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El código es obligatorio")]
    [MaxLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
    public string Codigo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
    public int Creditos { get; set; }
    
    // Propiedades de navegación
    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    public virtual ICollection<Prerrequisito> Prerrequisitos { get; set; } = new List<Prerrequisito>();
}