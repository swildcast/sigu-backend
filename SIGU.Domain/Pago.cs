namespace SIGU.Domain;

public class Pago
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int PeriodoId { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string RefPago { get; set; } = string.Empty;
}