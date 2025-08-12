using Duckie.Shared.Utils.Drawing;
using Duckie.Shared.Utils.Ui;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Duckie.Services.Clipboard;

internal static class CloudClipboard
{
    private const string url = "https://cg-zhou.top/api/clipboard";

    public static void UploadClipboardContent()
    {
        var text = ClipboardUtils.GetText();
        var filePath = ClipboardUtils.GetFile();
        var image = ClipboardUtils.GetImage();
        Task.Run(async () =>
        {
            try
            {
                var code = string.Empty;
                if (image != null)
                {
                    code = await UploadImageAsync(image);
                }
                else if (!string.IsNullOrWhiteSpace(text))
                {
                    code = await UploadTextAsync(text);
                }
                else if (!string.IsNullOrWhiteSpace(filePath))
                {
                    code = await UploadFileAsync(filePath);
                }

                if (!string.IsNullOrWhiteSpace(code))
                {
                    NotifyIconUtils.Notify("Extraction code: " + code, "Cloud Clipboard");
                }
            }
            catch (Exception e)
            {
                
                NotifyIconUtils.Notify($"Request failed: {e.Message}{e.InnerException?.Message}", "Cloud Clipboard");
            }
        });
    }

    private static Task<string> UploadImageAsync(Image image)
    {
        var fileName = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png";
        var bytes = image.ToBytes();
        var content = new ByteArrayContent(bytes);
        content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        return UploadAsync(fileName, content);
    }

    private static Task<string> UploadTextAsync(string text)
    {
        var fileName = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt";
        var bytes = Encoding.UTF8.GetBytes(text);
        var content = new ByteArrayContent(bytes);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        return UploadAsync(fileName, content);
    }

    private static async Task<string> UploadFileAsync(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        using (var stream = fileInfo.OpenRead())
        {
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return await UploadAsync(fileInfo.Name, content);
        }
    }

    private static async Task<string> UploadAsync(string fileName, HttpContent content)
    {
        using (var formData = new MultipartFormDataContent())
        {
            formData.Add(content, "file_0", fileName);
            var response = await HttpUtils.PostAsync(url, formData);
            var match = Regex.Match(response, @"^{""code"":""(\w+)""}$");
            if (!match.Success)
            {
                throw new Exception("Parse response failed: " + response);
            }

            var code = match.Groups[1].Value;
            return code;
        }
    }
}
