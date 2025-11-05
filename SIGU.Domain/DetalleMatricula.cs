namespace SIGU.Domain;

public class DetalleMatricula
{
    public int Id { get; set; }
    public int IdMatricula { get; set; }
    public int IdGrupo { get; set; }
    public string Estado { get; set; } = string.Empty;
}