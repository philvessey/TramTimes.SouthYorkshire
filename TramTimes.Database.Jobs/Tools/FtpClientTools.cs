using System.Net;
using FluentFTP;

namespace TramTimes.Database.Jobs.Tools;

public static class FtpClientTools
{
    private static readonly string _hostname = Environment.GetEnvironmentVariable(variable: "FTP_HOSTNAME") ?? string.Empty;
    private static readonly string _username = Environment.GetEnvironmentVariable(variable: "FTP_USERNAME") ?? string.Empty;
    private static readonly string _password = Environment.GetEnvironmentVariable(variable: "FTP_PASSWORD") ?? string.Empty;

    public static async Task<FtpStatus> GetFromRemoteAsync(
        string localPath,
        string remoteFileName) {

        #region connect server

        var ftpClient = new AsyncFtpClient(
            host: _hostname,
            credentials: new NetworkCredential(
                userName: _username,
                password: _password));

        await ftpClient.Connect();

        #endregion

        #region download file

        var remotePath = Path.Combine(
            path1: "TNDSV2.5",
            path2: remoteFileName);

        var result = await ftpClient.DownloadFile(
            localPath: localPath,
            remotePath: remotePath,
            existsMode: FtpLocalExists.Overwrite,
            verifyOptions: FtpVerify.Retry);

        #endregion

        #region disconnect server

        await ftpClient.Disconnect();

        #endregion

        return result;
    }
}