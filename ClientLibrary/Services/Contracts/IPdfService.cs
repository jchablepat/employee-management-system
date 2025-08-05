namespace ClientLibrary.Services.Contracts
{
    public interface IPdfService
    {
        Task Export<T>(IEnumerable<T> data, string fileName, bool landscape = true);
    }
}
