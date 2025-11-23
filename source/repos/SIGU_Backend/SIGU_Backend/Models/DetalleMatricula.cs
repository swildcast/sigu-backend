namespace SIGU_Backend.Models
{
    public class DetalleMatricula
    {
        public int Id { get; set; }
        public int MatriculaId { get; set; }
        public int GrupoId { get; set; }
        public string Estado { get; set; } = "Inscrito"; // Inscrito / Retirado
    }
}