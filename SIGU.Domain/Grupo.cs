namespace SIGU.Domain;

public class Grupo
{
    public int Id { get; set; }
    public int IdMateria { get; set; }
    public int IdPeriodo { get; set; }
    public int DocenteId { get; set; }
    public int Cupo { get; set; }
    public string Horario { get; set; } = string.Empty;
}