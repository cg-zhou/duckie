using Duckie.Utils;
using Duckie.Utils.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private string path = string.Empty;

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (!PathUtils.SelectImageFile(out path))
            {
                return;
            }
            var bitmapImage = File.ReadAllBytes(path).ToBitmapImage();

            // Get the DPI of the primary monitor
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;

            image.Source = bitmapImage;
            image.Width = bitmapImage.PixelWidth / transform.M11;
            image.Height = bitmapImage.PixelHeight / transform.M22;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null)
            {
                UiUtils.Warning("Please open an image", "Save Image");
                return;
            }

            var folder = Directory.GetParent(path).FullName;

            var icoPath = Path.Combine(folder, $"{Path.GetFileNameWithoutExtension(path)}.ico");
            icoPath = PathUtils.GetAlternativePath(icoPath);

            var ico = image.Source.ToBytes()
                .ToBitmap()
                .ToIco();
            File.WriteAllBytes(icoPath, ico);

            PathUtils.Reveal(icoPath);
        }
    }
}
