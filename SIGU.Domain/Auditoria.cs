namespace SIGU.Domain;

public class Auditoria
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Detalle { get; set; } = string.Empty;
}