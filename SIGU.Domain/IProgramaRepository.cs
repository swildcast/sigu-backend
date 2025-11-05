namespace SIGU.Domain;

public interface IProgramaRepository
{
    Task<Programa?> GetByIdAsync(int id);
    Task<IEnumerable<Programa>> GetAllAsync();
    Task AddAsync(Programa programa);
    Task UpdateAsync(Programa programa);
    Task DeleteAsync(int id);
}