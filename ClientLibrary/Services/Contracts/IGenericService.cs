using BaseLibrary.Responses;

namespace ClientLibrary.Services.Contracts
{
    /// <summary>
    /// Generic service interface for CRUD operations in a Blazor application.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericService<T>
    {
        Task<List<T>> GetAll(string baseUrl);
        Task<T> GetById(int id, string baseUrL);
        Task<GeneralResponse> Insert(T entity, string baseUrl);
        Task<GeneralResponse> Update(T entity, string baseUrl);
        Task<GeneralResponse> Delete(int id, string baseUrl);
    }
}
