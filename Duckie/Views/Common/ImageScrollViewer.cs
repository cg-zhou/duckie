using System;
using System.Windows;
using System.Windows.Input;

namespace Duckie.Views.Common
{
    /// <summary>
    /// 专门为图像查看设计的滚动控件，支持鼠标拖动滚动
    /// </summary>
    internal class ImageScrollViewer : ScrollViewerEx
    {
        // 鼠标拖动相关字段
        private bool _isDragging = false;
        private Point _lastMousePosition;

        public ImageScrollViewer()
            : base()
        {
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // 只有在有内容时才启用拖动
            if (Content != null)
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(this);
                CaptureMouse();
                
                // 使用正确的手形光标
                Cursor = Cursors.Hand;
                e.Handled = true;
            }
            else
            {
                base.OnMouseLeftButtonDown(e);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
                e.Handled = true;
            }
            else
            {
                base.OnMouseLeftButtonUp(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                var currentPosition = e.GetPosition(this);
                var deltaX = currentPosition.X - _lastMousePosition.X;
                var deltaY = currentPosition.Y - _lastMousePosition.Y;

                // 反向移动滚动条（拖动图像的感觉）
                ScrollToHorizontalOffset(HorizontalOffset - deltaX);
                ScrollToVerticalOffset(VerticalOffset - deltaY);

                _lastMousePosition = currentPosition;
                e.Handled = true;
            }
            else
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }
            base.OnMouseLeave(e);
        }
    }
}
