namespace SIGU.Domain;

public class Programa
{
	public int Id { get; set; }
	public string Nombre { get; set; } = string.Empty;
	public string Facultad { get; set; } = string.Empty;
	public int CreditosTotales { get; set; }
}