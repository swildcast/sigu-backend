namespace SIGU.Domain;

public class ProyectoInv
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int InvestigadorLiderId { get; set; }
    public int ConvocatoriaId { get; set; }
    public decimal Presupuesto { get; set; }
    public string Estado { get; set; } = string.Empty;
}