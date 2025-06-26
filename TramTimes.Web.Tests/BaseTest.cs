using Aspire.Hosting.Testing;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Playwright;
using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests;

public class BaseTest(AspireManager aspireManager) : IClassFixture<AspireManager>, IAsyncDisposable
{
	private AspireManager AspireManager { get; } = aspireManager ?? throw new ArgumentNullException(paramName: nameof(aspireManager));
	private PlaywrightManager PlaywrightManager => AspireManager.PlaywrightManager;
	
	protected async Task ConfigureTestAsync<TEntryPoint>() where TEntryPoint : class
	{
		#region configure aspire
		
		await AspireManager.ConfigureAsync<TEntryPoint>();
		
		#endregion
	}
	
	protected async Task RunTestDarkModeAsync(Func<IPage, Task> test)
	{
		#region check health
        
		if (AspireManager.Application is null)
			return;
		
		var tokenSource = new CancellationTokenSource(delay: TimeSpan.FromSeconds(seconds: 15));
		
		await AspireManager.Application.ResourceNotifications
			.WaitForResourceHealthyAsync(
				resourceName: "web-site",
				cancellationToken: tokenSource.Token)
			.WaitAsync(
				timeout: TimeSpan.FromSeconds(seconds: 15),
				cancellationToken: tokenSource.Token);
        
		#endregion
        
		#region build context
		
		if (PlaywrightManager.Browser is null)
			return;
		
		var context = await PlaywrightManager.Browser.NewContextAsync(options: new BrowserNewContextOptions
		{
			BaseURL = AspireManager.Application
				.GetEndpoint(resourceName: "web-site")
				.ToString(),
			
			ColorScheme = ColorScheme.Dark,
			IgnoreHTTPSErrors = true
		});
		
		#endregion
		
		#region build page
        
		var page = await context.NewPageAsync();
        
		#endregion
        
		#region run test
		
		try
		{
			await test(page);
		}
		finally
		{
			await page.CloseAsync();
		}
		
		await context.CloseAsync();
		await context.DisposeAsync();
		
		#endregion
	}
	
	protected async Task RunTestLightModeAsync(Func<IPage, Task> test)
	{
		#region check health
        
		if (AspireManager.Application is null)
			return;
		
		var tokenSource = new CancellationTokenSource(delay: TimeSpan.FromSeconds(seconds: 15));
		
		await AspireManager.Application.ResourceNotifications
			.WaitForResourceHealthyAsync(
				resourceName: "web-site",
				cancellationToken: tokenSource.Token)
			.WaitAsync(
				timeout: TimeSpan.FromSeconds(seconds: 15),
				cancellationToken: tokenSource.Token);
        
		#endregion
        
		#region build context
		
		if (PlaywrightManager.Browser is null)
			return;
		
		var context = await PlaywrightManager.Browser.NewContextAsync(options: new BrowserNewContextOptions
		{
			BaseURL = AspireManager.Application
				.GetEndpoint(resourceName: "web-site")
				.ToString(),
			
			ColorScheme = ColorScheme.Light,
			IgnoreHTTPSErrors = true
		});
		
		#endregion
		
		#region build page
        
		var page = await context.NewPageAsync();
        
		#endregion
        
		#region run test
		
		try
		{
			await test(page);
		}
		finally
		{
			await page.CloseAsync();
		}
		
		await context.CloseAsync();
		await context.DisposeAsync();
		
		#endregion
	}
	
	protected async Task RunTestSystemModeAsync(Func<IPage, Task> test)
	{
		#region check health
        
		if (AspireManager.Application is null)
			return;
		
		var tokenSource = new CancellationTokenSource(delay: TimeSpan.FromSeconds(seconds: 15));
		
		await AspireManager.Application.ResourceNotifications
			.WaitForResourceHealthyAsync(
				resourceName: "web-site",
				cancellationToken: tokenSource.Token)
			.WaitAsync(
				timeout: TimeSpan.FromSeconds(seconds: 15),
				cancellationToken: tokenSource.Token);
        
		#endregion
        
		#region build context
		
		if (PlaywrightManager.Browser is null)
			return;
		
		var context = await PlaywrightManager.Browser.NewContextAsync(options: new BrowserNewContextOptions
		{
			BaseURL = AspireManager.Application
				.GetEndpoint(resourceName: "web-site")
				.ToString(),
			
			ColorScheme = ColorScheme.NoPreference,
			IgnoreHTTPSErrors = true
		});
		
		#endregion
		
		#region build page
        
		var page = await context.NewPageAsync();
        
		#endregion
        
		#region run test
		
		try
		{
			await test(page);
		}
		finally
		{
			await page.CloseAsync();
		}
		
		await context.CloseAsync();
		await context.DisposeAsync();
		
		#endregion
	}
	
	protected static async Task CompleteTestAsync(string? error)
	{
		#region check pass
		
		if (string.IsNullOrEmpty(value: error))
			Assert.True(condition: true);
		
		#endregion
		
		#region check fail
		
		if (!string.IsNullOrEmpty(value: error))
			Assert.Fail(message: error);
		
		#endregion
		
		#region return result
		
		await Task.FromResult(result: true);
		
		#endregion
	}
	
	protected async Task UploadTestAsync()
	{
		#region check health
		
		if (AspireManager.Application is null)
			return;
		
		#endregion
		
		#region get environment
		
		var endpoint = AspireManager.Application.GetEndpoint(
			resourceName: "storage",
			endpointName: "blob");
		
		if (endpoint.Scheme is not "tcp")
		{
			AspireManager.Storage!.CreateSubdirectory(path: "production");
			
			foreach (var item in AspireManager.Storage!.GetFiles())
			{
				item.CopyTo(
					destFileName: Path.Combine(
						path1: AspireManager.Storage.FullName,
						path2: "production",
						path3: item.Name),
					overwrite: true);
				
				item.Delete();
			}
		}
		else
		{
			AspireManager.Storage!.CreateSubdirectory(path: "development");
			
			foreach (var item in AspireManager.Storage!.GetFiles())
			{
				item.CopyTo(
					destFileName: Path.Combine(
						path1: AspireManager.Storage.FullName,
						path2: "development",
						path3: item.Name),
					overwrite: true);
				
				item.Delete();
			}
		}
		
		#endregion
		
		#region upload files
		
		var productionPath = Path.Combine(
			path1: AspireManager.Storage!.FullName,
			path2: "production");
		
		if (Directory.Exists(path: productionPath))
		{
			var blobRoot = new BlobServiceClient(serviceUri: endpoint);
			var blobService = blobRoot.GetBlobContainerClient(blobContainerName: "web");
			
			foreach (var item in new DirectoryInfo(path: productionPath).GetFiles())
			{
				await using var fileStream = item.OpenRead();
				var content = new StreamContent(content: fileStream);
				
				var name = item.Name.Replace(
					oldValue: "|",
					newValue: "/");
				
				content.Headers.Add(
					name: "Content-Type",
					value: "image/png");
				
				content.Headers.Add(
					name: "Custom-Name",
					value: $"{AspireManager.Storage.CreationTimeUtc:yyyyMMdd}/{name}");
				
				var blobClient = blobService.GetBlobClient(blobName: $"{AspireManager.Storage.CreationTimeUtc:yyyyMMdd}/{name}");
				
				var response = await blobClient.UploadAsync(
					content: await content.ReadAsStreamAsync(),
					options: new BlobUploadOptions
					{
						HttpHeaders = new BlobHttpHeaders
						{
							ContentType = "image/png"
						}
					});
				
				if (!response.GetRawResponse().IsError)
					item.Delete();
			}
		}
		
		#endregion
		
		#region upload files
		
		var developmentPath = Path.Combine(
			path1: AspireManager.Storage!.FullName,
			path2: "development");
		
		if (Directory.Exists(path: developmentPath))
		{
			using var httpClient = new HttpClient();
			
			foreach (var item in new DirectoryInfo(path: developmentPath).GetFiles())
			{
				await using var fileStream = item.OpenRead();
				var content = new StreamContent(content: fileStream);
				
				var name = item.Name.Replace(
					oldValue: "|",
					newValue: "/");
				
				content.Headers.Add(
					name: "Content-Type",
					value: "image/png");
				
				content.Headers.Add(
					name: "Custom-Name",
					value: $"{AspireManager.Storage.CreationTimeUtc:yyyyMMdd}/{name}");
				
				var response = await httpClient.PostAsync(
					requestUri: new Uri(
						baseUri: AspireManager.Application.GetEndpoint(
							resourceName: "web-api",
							endpointName: "http"),
						relativeUri: "web/screenshot/file"),
					content: content);
				
				if (response.IsSuccessStatusCode)
					item.Delete();
			}
		}
		
		#endregion
	}
	
	public async ValueTask DisposeAsync()
	{
		#region suppress finalizer
		
		GC.SuppressFinalize(obj: this);
		
		#endregion
		
		#region dispose managers
		
		await AspireManager.DisposeAsync();
		await PlaywrightManager.DisposeAsync();
		
		#endregion
	}
}