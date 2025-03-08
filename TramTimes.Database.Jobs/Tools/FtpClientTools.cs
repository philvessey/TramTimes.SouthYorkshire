using System.Net;
using FluentFTP;

namespace TramTimes.Database.Jobs.Tools;

public static class FtpClientTools
{
    private static readonly string UserName = Environment.GetEnvironmentVariable(variable: "FTP_USERNAME") ?? string.Empty;
    private static readonly string Password = Environment.GetEnvironmentVariable(variable: "FTP_PASSWORD") ?? string.Empty;
    
    public static async Task GetFromRemoteAsync(
        string localPath,
        string remoteFileName) {
        
        var ftpClient = new AsyncFtpClient(
            host: "tnds.basemap.co.uk",
            credentials: new NetworkCredential(
                userName: UserName,
                password: Password));
        
        await ftpClient.Connect();
        
        var remotePath = Path.Combine(
            path1: "TNDSV2.5",
            path2: remoteFileName);
        
        await ftpClient.DownloadFile(
            localPath: localPath,
            remotePath: remotePath,
            existsMode: FtpLocalExists.Overwrite,
            verifyOptions: FtpVerify.Retry);
        
        await ftpClient.Disconnect();
        
        await Task.CompletedTask;
    }
}