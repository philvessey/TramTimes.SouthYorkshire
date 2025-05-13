namespace TramTimes.Web.Site.Builders;

public static class QueryBuilder
{
    private static readonly string Endpoint = Environment.GetEnvironmentVariable(variable: "API_ENDPOINT") ?? string.Empty;
    
    public static string GetLocationFromDatabase(double[] extent)
    {
        return $"{Endpoint}/" +
               $"database/" +
               $"stops/" +
               $"location/" +
               $"{string.Join(
                   separator: "/",
                   values: [
                       extent.ElementAt(index: 1),
                       extent.ElementAt(index: 2),
                       extent.ElementAt(index: 3),
                       extent.ElementAt(index: 0)
                   ])}";
    }
    
    public static string GetLocationFromSearch(double[] extent)
    {
        return $"{Endpoint}/" +
               $"search/" +
               $"stops/" +
               $"location/" +
               $"{string.Join(
                   separator: "/",
                   values: [
                       extent.ElementAt(index: 1),
                       extent.ElementAt(index: 2),
                       extent.ElementAt(index: 3),
                       extent.ElementAt(index: 0)
                   ])}";
    }
    
    public static string GetNameFromDatabase(string name)
    {
        return $"{Endpoint}/" +
               $"database/" +
               $"stops/" +
               $"name/" +
               $"{name}";
    }
    
    public static string GetNameFromSearch(string name)
    {
        return $"{Endpoint}/" +
               $"search/" +
               $"stops/" +
               $"name/" +
               $"{name}";
    }
}