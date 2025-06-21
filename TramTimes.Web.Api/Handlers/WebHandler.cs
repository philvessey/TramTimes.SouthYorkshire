using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TramTimes.Web.Api.Models;
using TramTimes.Web.Utilities.Extensions;

namespace TramTimes.Web.Api.Handlers;

public static class WebHandler
{
    public static async Task<IResult> PostScreenshotByFileAsync(
        BlobContainerClient blobService,
        HttpRequest request) {
        
        #region check headers
        
        var contentLength = request.Headers.ContentLength;
        var contentType = request.Headers.ContentType;
        
        if (contentLength is null || string.IsNullOrEmpty(value: contentType.FirstOrDefault()))
            return Results.BadRequest();
        
        if (contentLength is 0 || contentType.FirstOrDefault() is not "image/png")
            return Results.NotFound();
        
        #endregion
        
        #region get headers
        
        request.Headers.TryGetValue(key: "Custom-Name", out var customHeaders);
        var customName = customHeaders.FirstOrDefault();
        
        if (string.IsNullOrEmpty(value: customName))
            return Results.BadRequest();
        
        if (!customName.EndsWithIgnoreCase(value: ".png"))
            return Results.NotFound();
        
        #endregion
        
        #region upload file
        
        var blobClient = blobService.GetBlobClient(blobName: customName);
        
        var response = await blobClient.UploadAsync(
            content: request.Body,
            options: new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/png"
                }
            });
        
        if (response.GetRawResponse().IsError)
            return Results.NotFound();
        
        #endregion
        
        #region build result
        
        var result = new WebScreenshot
        {
            Endpoint = blobClient.Uri.ToString()
        };
        
        #endregion
        
        return Results.Json(data: result);
    }
}