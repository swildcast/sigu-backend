namespace SIGU.Domain;

public class Nota
{
    public int Id { get; set; }
    public int IdDetalleMatricula { get; set; }
    public string Corte { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Fecha { get; set; }
}