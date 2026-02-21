using System.IO.Compression;
using System.Text;

namespace TramTimes.Web.Utilities.Tools;

public static class StorageTools
{
    public static string Compress(string input)
    {
        #region Build buffer

        var buffer = Encoding.UTF8.GetBytes(s: input);

        #endregion

        #region Build stream

        using var stream = new MemoryStream();

        #endregion

        #region Build compression

        using var gzip = new GZipStream(
            mode: CompressionMode.Compress,
            stream: stream);

        #endregion

        #region Build writer

        gzip.Write(buffer: buffer);
        gzip.Flush();

        #endregion

        #region Build result

        var result = Convert.ToBase64String(inArray: stream.ToArray());

        #endregion

        return result;
    }

    public static string Decompress(string input)
    {
        #region Build buffer

        var buffer = Convert.FromBase64String(s: input);

        #endregion

        #region Build stream

        using var stream = new MemoryStream(buffer: buffer);

        #endregion

        #region Build compression

        using var gzip = new GZipStream(
            mode: CompressionMode.Decompress,
            stream: stream);

        #endregion

        #region Build reader

        using var reader = new StreamReader(
            encoding: Encoding.UTF8,
            stream: gzip);

        #endregion

        #region Build result

        var result = reader.ReadToEnd();

        #endregion

        return result;
    }
}