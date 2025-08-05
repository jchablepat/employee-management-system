namespace ClientLibrary.Services.Contracts
{
    public interface IExcelService
    {
        Task Export<T>(IEnumerable<T> data, string fileName);
        Task<List<T>> Read<T>(MemoryStream ms);
    }
}
