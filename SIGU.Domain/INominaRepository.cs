namespace SIGU.Domain;

public interface INominaRepository
{
    Task<Nomina?> GetByIdAsync(int id);
    Task<IEnumerable<Nomina>> GetAllAsync();
    Task AddAsync(Nomina nomina);
    Task UpdateAsync(Nomina nomina);
    Task DeleteAsync(int id);
}