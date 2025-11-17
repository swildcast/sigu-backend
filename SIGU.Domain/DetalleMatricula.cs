using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class DetalleMatricula
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El ID de la matrícula es obligatorio")]
    public int IdMatricula { get; set; }
    
    [Required(ErrorMessage = "El ID del grupo es obligatorio")]
    public int IdGrupo { get; set; }
    
    [MaxLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = "Activo";
    
    // Propiedades de navegación
    public virtual Matricula Matricula { get; set; } = null!;
    public virtual Grupo Grupo { get; set; } = null!;
}