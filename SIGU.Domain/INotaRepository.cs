namespace SIGU.Domain;

public interface INotaRepository
{
    Task<Nota?> GetByIdAsync(int id);
    Task<IEnumerable<Nota>> GetAllAsync();
    Task AddAsync(Nota nota);
    Task UpdateAsync(Nota nota);
    Task DeleteAsync(int id);
}