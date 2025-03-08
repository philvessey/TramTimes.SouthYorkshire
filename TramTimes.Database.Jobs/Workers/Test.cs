using Azure.Storage.Blobs;
using Quartz;

namespace TramTimes.Database.Jobs.Workers;

public class Test(
    BlobServiceClient blobService,
    ILogger<Build> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: Guid.NewGuid().ToString()));
        
        try
        {
            #region Get Active Blobs
            
            var activeBlobs = blobService.GetBlobContainerClient(blobContainerName: "database")
                .GetBlobsAsync(prefix: context.FireTimeUtc.Date.ToString(format: "yyyyMMdd") + "/gtfs/");
            
            await foreach (var blob in activeBlobs)
            {
                await blobService.GetBlobContainerClient(blobContainerName: "database")
                    .GetBlobClient(blob.Name)
                    .DownloadToAsync(path: Path.Combine(
                        path1: storage.FullName,
                        path2: Path.GetFileName(path: blob.Name)));
            }
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception thrown: {exception}",
                args: e.Message);
        }
        finally
        {
            storage.Delete(recursive: true);
        }
    }
}