using System.Globalization;
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
        
        if (!customName.ContainsIgnoreCase(value: "|") || !customName.EndsWithIgnoreCase(value: ".png"))
            return Results.NotFound();
        
        #endregion
        
        #region check prefix
        
        var prefix = customName
            .Split(separator: '|')
            .FirstOrDefault();
        
        var name = customName
            .Split(separator: '|')
            .LastOrDefault();
        
        if (string.IsNullOrEmpty(value: prefix) || string.IsNullOrEmpty(value: name))
            return Results.NotFound();
        
        #endregion
        
        #region get prefix
        
        var valid = DateTime.TryParseExact(
            s: prefix,
            format: "yyyyMMddHHmm",
            provider: CultureInfo.InvariantCulture,
            style: DateTimeStyles.None,
            out var dateTime);
        
        if (!valid || dateTime == default)
            return Results.NotFound();
        
        #endregion
        
        #region upload file
        
        var blobClient = blobService.GetBlobClient(blobName: $"{prefix}/{name}");
        
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
            Prefix = prefix,
            Name = name,
            Endpoint = blobClient.Uri.ToString()
        };
        
        #endregion
        
        return Results.Json(data: result);
    }
}