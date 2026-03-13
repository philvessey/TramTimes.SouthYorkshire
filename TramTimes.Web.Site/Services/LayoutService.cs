namespace TramTimes.Web.Site.Services;

public class LayoutService
{
    public event Action? OnLayoutChanged;
    public string? Layout { get; private set; }

    public void SetColumns(bool match)
    {
        #region Set value

        var layout = match ? "columns" : "rows";

        if (Layout == layout)
            return;

        Layout = layout;

        #endregion

        OnLayoutChanged?.Invoke();
    }

    public void SetRows(bool match)
    {
        #region Set value

        var layout = match ? "rows" : "columns";

        if (Layout == layout)
            return;

        Layout = layout;

        #endregion

        OnLayoutChanged?.Invoke();
    }
}