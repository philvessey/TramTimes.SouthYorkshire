namespace TramTimes.Web.Site.Types;

public enum ResponseType
{
    Idle = 0,
    Loading = 1,
    Syncing = 2,
    Success = 3,
    Failure = 4,
    Exception = 5
}