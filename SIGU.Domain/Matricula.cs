using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Matricula
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El ID del estudiante es obligatorio")]
    public int EstudianteId { get; set; }
    
    [Required(ErrorMessage = "El ID del periodo es obligatorio")]
    public int IdPeriodo { get; set; }
    
    [MaxLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = "Activa";
    
    // Propiedades de navegación
    public virtual Usuario Estudiante { get; set; } = null!;
    public virtual Periodo Periodo { get; set; } = null!;
    public virtual ICollection<DetalleMatricula> DetallesMatricula { get; set; } = new List<DetalleMatricula>();
}