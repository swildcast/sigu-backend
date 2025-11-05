namespace SIGU.Domain;

public class Usuario
{
    public int Id { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}