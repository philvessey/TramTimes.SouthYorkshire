namespace TramTimes.Aspire.Host.Tools;

public static class ContainerTools
{
    public static string GetTargetFilePath(string executable)
    {
        #region build environment

        var environment = Environment.GetEnvironmentVariable(variable: "PATH");

        if (string.IsNullOrWhiteSpace(value: environment))
            return executable;

        #endregion

        #region build path

        foreach (var item in environment.Split(separator: Path.PathSeparator))
        {
            var path = Path.Combine(
                path1: item,
                path2: OperatingSystem.IsWindows()
                    ? executable + ".exe"
                    : executable);

            if (File.Exists(path: path))
                return path;
        }

        #endregion

        return executable;
    }
}