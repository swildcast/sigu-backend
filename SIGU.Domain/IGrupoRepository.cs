namespace SIGU.Domain;

public interface IGrupoRepository
{
    Task<Grupo?> GetByIdAsync(int id);
    Task<IEnumerable<Grupo>> GetAllAsync();
    Task AddAsync(Grupo grupo);
    Task UpdateAsync(Grupo grupo);
    Task DeleteAsync(int id);
}