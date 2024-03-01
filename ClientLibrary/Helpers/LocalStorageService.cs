
using Blazored.LocalStorage;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStoreageService)
    {
        private const string StorageKey = "authentication-token";
        public async Task<string> GetToken() => await localStoreageService.GetItemAsStringAsync(StorageKey);
        public async Task SetToken(string item) => await localStoreageService.SetItemAsStringAsync(StorageKey, item);
        public async Task RemoveToken() => await localStoreageService.RemoveItemAsync(StorageKey);
    }
}
