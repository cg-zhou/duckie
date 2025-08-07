using Duckie.Utils;
using Duckie.Utils.Drawing;
using Duckie.Utils.Localization;
using Duckie.Utils.Ui;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Duckie.Views;

public partial class ImageView : UserControl, IDisposable
{
    // 预定义静态Brush对象，避免重复创建
    private static readonly SolidColorBrush DragOverBrush = new SolidColorBrush(Color.FromArgb(50, 0, 120, 215));

    private bool _disposed = false;

    public ImageView()
    {
        InitializeComponent();

        MouseWheel += ImageView_MouseWheel;
    }

    private string path = string.Empty;
    private double originalWidth;
    private double originalHeight;

    private void Open(string filePath)
    {
        ThrowIfDisposed();

        // 释放之前的图像资源
        DisposeCurrentImage();

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

        // Enable all controls
        EnableImageControls(true);

        // Default to fit to window
        ButtonZoomFit_Click(null, null);

        // Update status
        StatusText.Text = LocKey.Status_Loaded.Text(Path.GetFileName(filePath));
        ImageInfoText.Text = $"{bitmapImage.PixelWidth} × {bitmapImage.PixelHeight} pixels";
    }

    /// <summary>
    /// 释放当前图像资源
    /// </summary>
    private void DisposeCurrentImage()
    {
        if (image?.Source != null)
        {
            // 清理图像源
            image.Source = null;

            // Disable all controls when no image is loaded
            EnableImageControls(false);

            // 强制垃圾回收（在图像处理应用中是合理的）
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    /// <summary>
    /// 启用或禁用图像相关控件
    /// </summary>
    /// <param name="enabled">是否启用</param>
    private void EnableImageControls(bool enabled)
    {
        // Transform controls
        RotateLeftButton.IsEnabled = enabled;
        RotateRightButton.IsEnabled = enabled;
        FlipHButton.IsEnabled = enabled;
        FlipVButton.IsEnabled = enabled;

        // Zoom controls
        ZoomOutButton.IsEnabled = enabled;
        ZoomInButton.IsEnabled = enabled;
        ZoomSlider.IsEnabled = enabled;
        FitToWindowButton.IsEnabled = enabled;

        // Save button
        SaveButton.IsEnabled = enabled;
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
            UiUtils.Error(exception, LocKey.Error_FailedToOpenImage.Text(selectedPath));
        }
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (image.Source == null)
        {
            UiUtils.Warning(LocKey.Error_PleaseOpenImage.Text(), LocKey.Error_SaveImage.Text());
            return;
        }

        var folder = Directory.GetParent(path).FullName;

        var icoPath = Path.Combine(folder, $"{Path.GetFileNameWithoutExtension(path)}.ico");
        icoPath = PathUtils.GetAlternativePath(icoPath);

        var ico = image.Source.ToBytes()
            .ToBitmap()
            .ToIco();
        File.WriteAllBytes(icoPath, ico);

        UiUtils.Info(LocKey.Success_IconExported.Text(icoPath));
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
            if (image.Source == null) return;

            // 保存当前图像源的引用
            var currentSource = (BitmapSource)image.Source;

            var transform = new RotateTransform(angle);
            var transformedBitmap = new TransformedBitmap();
            transformedBitmap.BeginInit();
            transformedBitmap.Source = currentSource;
            transformedBitmap.Transform = transform;
            transformedBitmap.EndInit();

            // 先设置新图像，再清理旧图像（避免闪烁）
            image.Source = transformedBitmap;

            // Update original dimensions for zoom calculations
            var dpiTransform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            originalWidth = transformedBitmap.PixelWidth / dpiTransform.M11;
            originalHeight = transformedBitmap.PixelHeight / dpiTransform.M22;

            // Update display size with current zoom
            UpdateImageSize();

            // 如果当前源是TransformedBitmap，释放它以避免内存累积
            if (currentSource is TransformedBitmap)
            {
                GC.Collect();
            }
        }
        catch (Exception ex)
        {
            UiUtils.Error(ex, LocKey.Error_FailedToRotateImage.Text());
        }
    }

    private void FlipImage(bool flipHorizontal, bool flipVertical)
    {
        try
        {
            if (image.Source == null) return;

            // 保存当前图像源的引用
            var currentSource = (BitmapSource)image.Source;

            var scaleX = flipHorizontal ? -1 : 1;
            var scaleY = flipVertical ? -1 : 1;

            var transform = new ScaleTransform(scaleX, scaleY);
            var transformedBitmap = new TransformedBitmap();
            transformedBitmap.BeginInit();
            transformedBitmap.Source = currentSource;
            transformedBitmap.Transform = transform;
            transformedBitmap.EndInit();

            // 先设置新图像，再清理旧图像（避免闪烁）
            image.Source = transformedBitmap;

            // Update original dimensions for zoom calculations
            var dpiTransform = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            originalWidth = transformedBitmap.PixelWidth / dpiTransform.M11;
            originalHeight = transformedBitmap.PixelHeight / dpiTransform.M22;

            // Update display size with current zoom
            UpdateImageSize();

            // 如果当前源是TransformedBitmap，释放它以避免内存累积
            if (currentSource is TransformedBitmap)
            {
                GC.Collect();
            }
        }
        catch (Exception ex)
        {
            UiUtils.Error(ex, LocKey.Error_FailedToFlipImage.Text());
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
                // 使用预定义的静态Brush，避免重复创建
                Background = DragOverBrush;
                return;
            }
        }
        e.Effects = DragDropEffects.None;
    }

    private void ImageView_DragLeave(object sender, DragEventArgs e)
    {
        // Remove visual feedback
        Background = Brushes.Transparent;
    }

    private void ImageView_Drop(object sender, DragEventArgs e)
    {
        // Remove visual feedback
        Background = Brushes.Transparent;

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var imageFiles = files.Where(f => IsImageFile(f)).ToArray();

            if (imageFiles.Length > 0)
            {
                try
                {
                    // Open the first image file
                    Open(imageFiles[0]);
                }
                catch (Exception exception)
                {
                    UiUtils.Error(exception, LocKey.Error_FailedToOpenImage.Text(imageFiles[0]));
                }
            }
        }
    }

    private bool IsImageFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
               extension == ".gif" || extension == ".bmp";
    }

    private void ImageView_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        // 只有在按下 Ctrl 键且有图像时才处理缩放
        if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control &&
            image.Source != null)
        {
            // 计算缩放增量
            double zoomDelta = e.Delta > 0 ? 0.1 : -0.1;
            double newZoom = ZoomSlider.Value + zoomDelta;

            // 限制在滑块的范围内
            newZoom = Math.Max(ZoomSlider.Minimum, Math.Min(ZoomSlider.Maximum, newZoom));

            // 应用新的缩放值
            ZoomSlider.Value = newZoom;

            // 标记事件已处理，防止页面滚动
            e.Handled = true;
        }
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

    #region IDisposable Implementation

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源的具体实现
    /// </summary>
    /// <param name="disposing">是否正在释放托管资源</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // 释放托管资源
                DisposeCurrentImage();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~ImageView()
    {
        Dispose(false);
    }

    #endregion

    /// <summary>
    /// 检查对象是否已被释放
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ImageView));
        }
    }
}
