namespace SIGU.Domain;

public interface IProductoInvRepository
{
    Task<ProductoInv?> GetByIdAsync(int id);
    Task<IEnumerable<ProductoInv>> GetAllAsync();
    Task AddAsync(ProductoInv producto);
    Task UpdateAsync(ProductoInv producto);
    Task DeleteAsync(int id);
}