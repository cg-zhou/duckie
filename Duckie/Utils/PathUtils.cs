using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Duckie.Utils
{
    internal static class PathUtils
    {
        public static bool SelectImageFile(out string path)
        {
            path = string.Empty;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public static void Reveal(string path)
        {
            if (File.Exists(path))
            {
                Process.Start("explorer.exe", $"/select, \"{path}\"");
            }
            else if (Directory.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                throw new Exception($"Can't find path: {path}");
            }
        }

        public static string GetAlternativePath(string path)
        {
            var folder = Directory.GetParent(path).FullName;
            var existedNames = Directory.EnumerateFileSystemEntries(folder)
                .Select(x => Path.GetFileName(x))
                .ToArray();
            var name = Path.GetFileName(path);
            if (!existedNames.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return path;
            }

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            var namePrefix = nameWithoutExtension;
            var index = nameWithoutExtension.LastIndexOf('_');
            if (index > 0
                && int.TryParse(nameWithoutExtension.Substring(index + 1), out _))
            {
                namePrefix = nameWithoutExtension.Substring(0, index);
            }

            for (var i = 1; i < int.MaxValue; i++)
            {
                var newName = $"{namePrefix}_{i}{extension}";

                if (!newName.Equals(name, StringComparison.OrdinalIgnoreCase)
                    && !existedNames.Any(x => x.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                {
                    return Path.Combine(folder, newName);
                }
            }

            throw new Exception("Failed to get alternative name");
        }
    }
}
