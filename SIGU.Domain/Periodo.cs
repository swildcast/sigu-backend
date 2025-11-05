namespace SIGU.Domain;

public class Periodo
{
    public int Id { get; set; }
    public int Anio { get; set; }
    public int Semestre { get; set; }
    public string Estado { get; set; } = string.Empty;
}