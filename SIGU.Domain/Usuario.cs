using System.ComponentModel.DataAnnotations;

namespace SIGU.Domain;

public class Usuario
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El rol es obligatorio")]
    [MaxLength(50, ErrorMessage = "El rol no puede exceder 50 caracteres")]
    public string Rol { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [MaxLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string HashPassword { get; set; } = string.Empty;
    
    [MaxLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = "Activo";
    
    // Propiedades de navegación
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    public virtual ICollection<Grupo> GruposDocente { get; set; } = new List<Grupo>();
}