using System.Xml;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineStopPointTools
{
    public static async Task<TimeSpan> GetRunTimeAsync(
        List<TransXChangeJourneyPatternTimingLink>? timingLinks,
        int index) {
        
        var value = TimeSpan.Zero;
        
        switch (timingLinks)
        {
            case { Count: > 0 } when index > 0:
            {
                var link = timingLinks[index - 1];
                
                if (link is { RunTime: not null })
                    value = value.Add(ts: XmlConvert.ToTimeSpan(s: link.RunTime));
                
                break;
            }
        }
        
        return await Task.FromResult(result: value);
    }
    
    public static async Task<TimeSpan> GetWaitTimeAsync(
        List<TransXChangeJourneyPatternTimingLink>? timingLinks,
        int index) {
        
        var value = TimeSpan.Zero;
        
        switch (timingLinks)
        {
            case { Count: > 0 } when index > 0:
            {
                var link = timingLinks[index - 1];
                
                if (link is { To.WaitTime: not null })
                    value = value.Add(ts: XmlConvert.ToTimeSpan(s: link.To.WaitTime));
                
                break;
            }
        }
        
        switch (timingLinks)
        {
            case { Count: > 0 }:
            {
                var link = timingLinks[index];
                
                if (link is { From.WaitTime: not null })
                    value = value.Add(ts: XmlConvert.ToTimeSpan(s: link.From.WaitTime));
                
                break;
            }
        }
        
        return await Task.FromResult(result: value);
    }
}