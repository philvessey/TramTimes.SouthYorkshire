using System.Xml;
using TramTimes.Database.Jobs.Models;

namespace TramTimes.Database.Jobs.Tools;

public static class TravelineStopPointTools
{
    public static TimeSpan GetRunTime(
        List<TransXChangeJourneyPatternTimingLink>? timingLinks,
        int index) {
        
        var result = TimeSpan.Zero;
        
        #region build result
        
        switch (timingLinks)
        {
            case { Count: > 0 } when index > 0:
            {
                var link = timingLinks.ElementAt(index: index - 1);
                
                if (link is { RunTime: not null })
                    result = result.Add(ts: XmlConvert.ToTimeSpan(s: link.RunTime));
                
                break;
            }
        }
        
        #endregion
        
        return result;
    }
    
    public static TimeSpan GetWaitTime(
        List<TransXChangeJourneyPatternTimingLink>? timingLinks,
        int index) {
        
        var result = TimeSpan.Zero;
        
        #region build result
        
        switch (timingLinks)
        {
            case { Count: > 0 } when index > 0:
            {
                var link = timingLinks.ElementAt(index: index - 1);
                
                if (link is { To.WaitTime: not null })
                    result = result.Add(ts: XmlConvert.ToTimeSpan(s: link.To.WaitTime));
                
                break;
            }
        }
        
        switch (timingLinks)
        {
            case { Count: > 0 }:
            {
                var link = timingLinks.ElementAt(index: index);
                
                if (link is { From.WaitTime: not null })
                    result = result.Add(ts: XmlConvert.ToTimeSpan(s: link.From.WaitTime));
                
                break;
            }
        }
        
        #endregion
        
        return result;
    }
}