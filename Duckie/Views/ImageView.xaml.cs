using Duckie.Utils;
using Duckie.Utils.Drawing;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Duckie.Views
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private string path = string.Empty;
        private double originalWidth;
        private double originalHeight;

        private void Open(string filePath)
        {
            path = filePath;
            var bitmapImage = File.ReadAllBytes(path).ToBitmapImage();

            // Get the DPI of the primary monitor
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;

            image.Source = bitmapImage;
            originalWidth = bitmapImage.PixelWidth / transform.M11;
            originalHeight = bitmapImage.PixelHeight / transform.M22;

            // Reset zoom
            ZoomSlider.Value = 1.0;
            UpdateImageSize();

            // Update status
            StatusText.Text = $"Loaded: {Path.GetFileName(filePath)}";
            ImageInfoText.Text = $"{bitmapImage.PixelWidth} × {bitmapImage.PixelHeight} pixels";
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (!PathUtils.SelectImageFile(out string selectedPath))
            {
                return;
            }

            try
            {
                Open(selectedPath);
            }
            catch (Exception exception)
            {
                UiUtils.Error(exception, $"Failed to open image: {selectedPath}");
            }
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

            UiUtils.Info($"The icon has been exported: {icoPath}");
        }



        private void ButtonRotate90_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null) return;
            RotateImage(90);
        }



        private void ButtonRotate270_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null) return;
            RotateImage(270);
        }

        private void ButtonFlipH_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null) return;
            FlipImage(true, false);
        }

        private void ButtonFlipV_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null) return;
            FlipImage(false, true);
        }

        private void RotateImage(double angle)
        {
            try
            {
                var transform = new RotateTransform(angle);
                var transformedBitmap = new TransformedBitmap();
                transformedBitmap.BeginInit();
                transformedBitmap.Source = (BitmapSource)image.Source;
                transformedBitmap.Transform = transform;
                transformedBitmap.EndInit();

                image.Source = transformedBitmap;

                // Update original dimensions for zoom calculations
                var dpiTransform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                originalWidth = transformedBitmap.PixelWidth / dpiTransform.M11;
                originalHeight = transformedBitmap.PixelHeight / dpiTransform.M22;

                // Update display size with current zoom
                UpdateImageSize();
            }
            catch (Exception ex)
            {
                UiUtils.Error(ex, "Failed to rotate image");
            }
        }

        private void FlipImage(bool flipHorizontal, bool flipVertical)
        {
            try
            {
                var scaleX = flipHorizontal ? -1 : 1;
                var scaleY = flipVertical ? -1 : 1;

                var transform = new ScaleTransform(scaleX, scaleY);
                var transformedBitmap = new TransformedBitmap();
                transformedBitmap.BeginInit();
                transformedBitmap.Source = (BitmapSource)image.Source;
                transformedBitmap.Transform = transform;
                transformedBitmap.EndInit();

                image.Source = transformedBitmap;

                // Update original dimensions for zoom calculations
                var dpiTransform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                originalWidth = transformedBitmap.PixelWidth / dpiTransform.M11;
                originalHeight = transformedBitmap.PixelHeight / dpiTransform.M22;

                // Update display size with current zoom
                UpdateImageSize();
            }
            catch (Exception ex)
            {
                UiUtils.Error(ex, "Failed to flip image");
            }
        }

        private void ImageView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var imageFiles = files.Where(f => IsImageFile(f)).ToArray();

                if (imageFiles.Length > 0)
                {
                    e.Effects = DragDropEffects.Copy;
                    // Add visual feedback
                    this.Background = new SolidColorBrush(Color.FromArgb(50, 0, 120, 215));
                    return;
                }
            }
            e.Effects = DragDropEffects.None;
        }

        private void ImageView_DragLeave(object sender, DragEventArgs e)
        {
            // Remove visual feedback
            this.Background = Brushes.Transparent;
        }

        private void ImageView_Drop(object sender, DragEventArgs e)
        {
            // Remove visual feedback
            this.Background = Brushes.Transparent;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var imageFiles = files.Where(f => IsImageFile(f)).ToArray();

                if (imageFiles.Length > 0)
                {
                    // Open the first image file
                    Open(imageFiles[0]);
                }
            }
        }

        private bool IsImageFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                   extension == ".gif" || extension == ".bmp";
        }

        private void UpdateImageSize()
        {
            if (image?.Source != null)
            {
                var zoom = ZoomSlider.Value;
                image.Width = originalWidth * zoom;
                image.Height = originalHeight * zoom;
            }
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateImageSize();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomSlider.Value = Math.Min(ZoomSlider.Maximum, ZoomSlider.Value + 0.2);
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomSlider.Value = Math.Max(ZoomSlider.Minimum, ZoomSlider.Value - 0.2);
        }

        private void ButtonZoomFit_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null) return;

            // Calculate the available space for the image
            var scrollViewer = FindScrollViewer();
            if (scrollViewer != null)
            {
                var availableWidth = scrollViewer.ViewportWidth - 20; // Some padding
                var availableHeight = scrollViewer.ViewportHeight - 20;

                var scaleX = availableWidth / originalWidth;
                var scaleY = availableHeight / originalHeight;
                var scale = Math.Min(scaleX, scaleY);

                ZoomSlider.Value = Math.Max(ZoomSlider.Minimum, Math.Min(ZoomSlider.Maximum, scale));
            }
        }

        private ScrollViewer FindScrollViewer()
        {
            // Find the ScrollViewer parent
            DependencyObject parent = image;
            while (parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is ScrollViewer scrollViewer)
                    return scrollViewer;
            }
            return null;
        }
    }
}
