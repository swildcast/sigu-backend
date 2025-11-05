namespace SIGU_Backend.Models
{
    public class Matricula
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; } // FK a Usuario.Id (rol = Estudiante)
        public int PeriodoId { get; set; }
        public string Estado { get; set; } = "Activa"; // Activa / Cerrada
    }
}