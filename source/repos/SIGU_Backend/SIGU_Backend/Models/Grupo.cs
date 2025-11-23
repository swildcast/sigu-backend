namespace SIGU_Backend.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public int MateriaId { get; set; }
        public int PeriodoId { get; set; }
        public int DocenteId { get; set; } // FK a Usuario.Id (rol = Docente)
        public int Cupo { get; set; }
        public string Horario { get; set; } = ""; // Ej: "Lun 8-10, Mie 8-10"
    }
}