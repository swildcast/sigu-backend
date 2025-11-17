namespace SIGU.Domain;

public class Nomina
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public decimal SalarioBase { get; set; }
    public decimal Deducciones { get; set; }
    public decimal Neto { get; set; }
    // Ejemplo de cambio: agregar campo para tipo de contrato
    public string TipoContrato { get; set; } = string.Empty;
}