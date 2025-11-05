namespace SIGU.Domain;

public class ProductoInv
{
    public int Id { get; set; }
    public int IdProyecto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}