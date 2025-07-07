using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Quartz;
using TramTimes.Search.Jobs.Models;

namespace TramTimes.Search.Jobs.Workers;

public class Clean(
    ElasticsearchClient searchService,
    ILogger<Clean> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        var guid = Guid.NewGuid();
        
        var storage = Directory.CreateDirectory(path: Path.Combine(
            path1: Path.GetTempPath(),
            path2: guid.ToString()));
        
        try
        {
            #region delete search index
            
            await searchService.Indices.DeleteAsync(indices: "search");
            
            #endregion
            
            #region create search index
            
            await searchService.Indices.CreateAsync<SearchStop>(
                index: "search",
                configureRequest: request => request
                    .Mappings(configure: map => map
                        .Properties(properties: new Properties<SearchStop>
                        {
                            { "code", new KeywordProperty() },
                            { "id", new KeywordProperty() },
                            { "latitude", new DoubleNumberProperty() },
                            { "location", new GeoPointProperty() },
                            { "longitude", new DoubleNumberProperty() },
                            { "name", new KeywordProperty() },
                            { "platform", new TextProperty() },
                            { "points", new ObjectProperty() }
                        })));
            
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