using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClipboardEx = System.Windows.Forms.Clipboard;

namespace Duckie.Services.Clipboard;

internal static class ClipboardUtils
{
    public static Image GetImage()
    {
        return ClipboardEx.GetDataObject()
            ?.GetData(DataFormats.Bitmap)
            as Image;
    }

    public static string GetText()
    {
        return ClipboardEx.GetDataObject()
            ?.GetData(DataFormats.UnicodeText)
            as string ?? string.Empty;
    }

    public static string GetFile()
    {
        if (ClipboardEx.ContainsFileDropList())
        {
            var files = ClipboardEx.GetFileDropList();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    return file;
                }
            }
        }

        return string.Empty;
    }
}