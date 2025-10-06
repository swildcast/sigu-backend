namespace SIGU.Domain;

public interface IAuditoriaRepository
{
    Task<Auditoria?> GetByIdAsync(int id);
    Task<IEnumerable<Auditoria>> GetAllAsync();
    Task AddAsync(Auditoria auditoria);
    Task UpdateAsync(Auditoria auditoria);
    Task DeleteAsync(int id);
}