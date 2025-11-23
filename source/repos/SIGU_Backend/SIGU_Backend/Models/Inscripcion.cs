namespace SIGU_Backend.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}