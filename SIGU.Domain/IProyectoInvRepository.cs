namespace SIGU.Domain;

public interface IProyectoInvRepository
{
    Task<ProyectoInv?> GetByIdAsync(int id);
    Task<IEnumerable<ProyectoInv>> GetAllAsync();
    Task AddAsync(ProyectoInv proyecto);
    Task UpdateAsync(ProyectoInv proyecto);
    Task DeleteAsync(int id);
}