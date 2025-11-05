namespace SIGU.Domain;

public interface IPeriodoRepository
{
    Task<Periodo?> GetByIdAsync(int id);
    Task<IEnumerable<Periodo>> GetAllAsync();
    Task AddAsync(Periodo periodo);
    Task UpdateAsync(Periodo periodo);
    Task DeleteAsync(int id);
}