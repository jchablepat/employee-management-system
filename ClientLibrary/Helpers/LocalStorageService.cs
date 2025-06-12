using Blazored.LocalStorage;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string StorageKey = "authentication-token";

        /// <summary>
        /// Retrieve the token from local storage as string with the specified key.
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetToken() => await localStorageService.GetItemAsStringAsync(StorageKey);

        /// <summary>
        /// Sets or updates the token in local storage with the specified key
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task SetToken(string item) => await localStorageService.SetItemAsStringAsync(StorageKey, item);

        /// <summary>
        /// Removes the token from local storage with the specified key
        /// </summary>
        /// <returns></returns>
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(StorageKey);
    }
}
