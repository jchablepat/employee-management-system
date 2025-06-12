using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    /// <summary>
    /// Generic repository interface to handle CRUD operations for any entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task<GeneralResponse> Insert(T entity);
        Task<GeneralResponse> Update(T entity);
        Task<GeneralResponse> DeleteById(int id);
    }
}
