namespace SIGU.Domain;

public class Presupuesto
{
    public int Id { get; set; }
    public string CentroCosto { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal ValorAsignado { get; set; }
    public decimal ValorEjecutado { get; set; }
}