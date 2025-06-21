using Azure.Storage.Blobs;
using Quartz;

namespace TramTimes.Web.Jobs.Workers;

public class Clean(
    BlobContainerClient blobService,
    ILogger<Clean> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region delete expired blobs
            
            var expiredBlobs = blobService.GetBlobsAsync();
            
            await foreach (var item in expiredBlobs)
                if (item.Properties.LastModified < context.FireTimeUtc.Date.AddDays(value: -28))
                    await blobService
                        .GetBlobClient(blobName: item.Name)
                        .DeleteAsync();
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}