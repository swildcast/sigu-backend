using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Prerrequisito
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El ID de la materia es obligatorio")]
    public int IdMateria { get; set; }
    
    [Required(ErrorMessage = "El ID de la materia prerrequisito es obligatorio")]
    public int IdMateriaReq { get; set; }
    
    // Propiedades de navegación
    public virtual Materia Materia { get; set; } = null!;
    public virtual Materia MateriaRequisito { get; set; } = null!;
}