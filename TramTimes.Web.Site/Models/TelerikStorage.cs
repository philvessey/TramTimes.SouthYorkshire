using JetBrains.Annotations;

namespace TramTimes.Web.Site.Models;

public class TelerikStorage<T>
{
    [UsedImplicitly] public bool? Success { get; set; }
    [UsedImplicitly] public T? Value { get; set; }
}