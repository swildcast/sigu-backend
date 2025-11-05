namespace SIGU.Domain;

public interface IMateriaRepository
{
    Task<Materia?> GetByIdAsync(int id);
    Task<IEnumerable<Materia>> GetAllAsync();
    Task AddAsync(Materia materia);
    Task UpdateAsync(Materia materia);
    Task DeleteAsync(int id);
}