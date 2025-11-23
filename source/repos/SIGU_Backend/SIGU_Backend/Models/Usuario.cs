namespace SIGU_Backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Rol { get; set; } = "Estudiante"; // Estudiante, Docente, Coordinador, etc.
        public string Estado { get; set; } = "Activo";
    }
}   