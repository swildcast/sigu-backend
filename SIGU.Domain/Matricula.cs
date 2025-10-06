namespace SIGU.Domain;

public class Matricula
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int IdPeriodo { get; set; }
    public string Estado { get; set; } = string.Empty;
}