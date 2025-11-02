using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Quartz;

namespace TramTimes.Search.Jobs.Workers;

public class Production(
    ElasticsearchClient searchService,
    ILogger<Production> logger) : IJob {
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            #region delete search index
            
            await searchService.Indices.DeleteAsync(request: new DeleteIndexRequest
            {
                Indices = "southyorkshire"
            });
            
            #endregion
            
            #region create search index
            
            await searchService.Indices.CreateAsync(request: new CreateIndexRequest
            {
                Index = "southyorkshire",
                Mappings = new TypeMapping
                {
                    Properties = new Properties
                    {
                        { "code", new KeywordProperty() },
                        { "id", new KeywordProperty() },
                        { "latitude", new DoubleNumberProperty() },
                        { "location", new GeoPointProperty() },
                        { "longitude", new DoubleNumberProperty() },
                        { "name", new KeywordProperty() },
                        { "platform", new TextProperty() },
                        { "points", new ObjectProperty() }
                    }
                }
            });
            
            #endregion
        }
        catch (Exception e)
        {
            logger.LogError(
                message: "Exception: {exception}",
                args: e.ToString());
        }
    }
}