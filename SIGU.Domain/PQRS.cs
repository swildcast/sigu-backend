namespace SIGU.Domain;

public class PQRS
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime SlaFecha { get; set; }
}