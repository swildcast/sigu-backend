namespace SIGU.Domain;

public interface IPQRSRepository
{
    Task<PQRS?> GetByIdAsync(int id);
    Task<IEnumerable<PQRS>> GetAllAsync();
    Task AddAsync(PQRS pqrs);
    Task UpdateAsync(PQRS pqrs);
    Task DeleteAsync(int id);
}