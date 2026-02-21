using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TramTimes.Web.Site.Models;
using TramTimes.Web.Utilities.Tools;

namespace TramTimes.Web.Site.Services;

public class StorageService(ProtectedLocalStorage storage)
{
    public async Task<TelerikStorage<T>> SetAsync<T>(
        string key,
        T value) where T : class {

        try
        {
            #region Serialize value

            var serialized = JsonSerializer.Serialize(value: value);

            #endregion

            #region Compress value

            var compressed = StorageTools.Compress(input: serialized);

            #endregion

            #region Check value

            if (string.IsNullOrEmpty(value: compressed))
                return new TelerikStorage<T> { Success = false };

            #endregion

            #region Set value

            await storage.SetAsync(
                value: compressed,
                key: key);

            #endregion

            return new TelerikStorage<T>
            {
                Success = true,
                Value = value
            };
        }
        catch (Exception e)
        {
            throw new Exception(
                message: $"Failed to set value for key: {key}",
                innerException: e);
        }
    }

    public async Task<TelerikStorage<T>> GetAsync<T>(string key) where T : class
    {
        try
        {
            #region Get value

            var result = await storage.GetAsync<string>(key: key);

            #endregion

            #region Check value

            if (!result.Success || result.Value is null)
                return new TelerikStorage<T> { Success = false };

            #endregion

            #region Decompress value

            var decompressed = StorageTools.Decompress(input: result.Value);

            #endregion

            #region Deserialize value

            var value = JsonSerializer.Deserialize<T>(json: decompressed);

            #endregion

            return new TelerikStorage<T>
            {
                Success = true,
                Value = value
            };
        }
        catch (Exception e)
        {
            throw new Exception(
                message: $"Failed to get value for key: {key}",
                innerException: e);
        }
    }

    public async Task<TelerikStorage<bool>> DeleteAsync(string key)
    {
        try
        {
            #region Delete value

            await storage.DeleteAsync(key: key);

            #endregion

            return new TelerikStorage<bool>
            {
                Success = true,
                Value = true
            };
        }
        catch (Exception e)
        {
            throw new Exception(
                message: $"Failed to delete value for key: {key}",
                innerException: e);
        }
    }
}