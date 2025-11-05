namespace SIGU.Domain;

public interface IMatriculaRepository
{
    Task<Matricula?> GetByIdAsync(int id);
    Task<IEnumerable<Matricula>> GetAllAsync();
    Task AddAsync(Matricula matricula);
    Task UpdateAsync(Matricula matricula);
    Task DeleteAsync(int id);
}