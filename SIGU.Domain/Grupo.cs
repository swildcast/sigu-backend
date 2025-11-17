using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Grupo
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El ID de la materia es obligatorio")]
    public int IdMateria { get; set; }
    
    [Required(ErrorMessage = "El ID del periodo es obligatorio")]
    public int IdPeriodo { get; set; }
    
    [Required(ErrorMessage = "El ID del docente es obligatorio")]
    public int DocenteId { get; set; }
    
    [Range(1, 100, ErrorMessage = "El cupo debe estar entre 1 y 100")]
    public int Cupo { get; set; }
    
    [MaxLength(100, ErrorMessage = "El horario no puede exceder 100 caracteres")]
    public string Horario { get; set; } = string.Empty;
    
    // Propiedades de navegación
    public virtual Materia Materia { get; set; } = null!;
    public virtual Periodo Periodo { get; set; } = null!;
    public virtual Usuario Docente { get; set; } = null!;
    public virtual ICollection<DetalleMatricula> DetallesMatricula { get; set; } = new List<DetalleMatricula>();
}