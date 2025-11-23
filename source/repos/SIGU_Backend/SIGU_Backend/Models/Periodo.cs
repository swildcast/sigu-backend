namespace SIGU_Backend.Models
{
    public class Periodo
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int Semestre { get; set; } // 1 o 2
        public string Estado { get; set; } = "Abierto"; // Abierto / Cerrado
    }
}