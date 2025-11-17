using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Periodo
{
    public int Id { get; set; }
    
    [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100")]
    public int Anio { get; set; }
    
    [Range(1, 2, ErrorMessage = "El semestre debe ser 1 o 2")]
    public int Semestre { get; set; }
    
    [MaxLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = "Activo";
    
    // Propiedades de navegación
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}