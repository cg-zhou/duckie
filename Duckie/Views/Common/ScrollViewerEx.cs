using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Duckie.Views.Common
{
    internal partial class ScrollViewerEx : ScrollViewer
    {
        public static readonly DependencyProperty OrientProperty = DependencyProperty.Register(
            nameof(Orient), typeof(OrientType), typeof(ScrollViewerEx),
            new PropertyMetadata(OrientType.Both));

        public OrientType Orient
        {
            get => (OrientType)GetValue(OrientProperty);
            set => SetValue(OrientProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            HorizontalScrollBarVisibility = Orient.HasFlag(OrientType.Horizontal)
                ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
            VerticalScrollBarVisibility = Orient.HasFlag(OrientType.Vertical)
                ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (ScrollInfo == null)
            {
                return;
            }

            var isShiftCtrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl)
                || Keyboard.IsKeyDown(Key.RightCtrl)
                || Keyboard.IsKeyDown(Key.LeftAlt)
                || Keyboard.IsKeyDown(Key.RightAlt);
            if (isShiftCtrlKeyDown)
            {
                return;
            }

            var isShiftKeyDown = Keyboard.IsKeyDown(Key.LeftShift)
                    || Keyboard.IsKeyDown(Key.RightShift);

            if (Orient.HasFlag(OrientType.Horizontal)
                && isShiftKeyDown)
            {
                if (e.Delta < 0)
                {
                    ScrollInfo.MouseWheelRight();
                }
                else
                {
                    ScrollInfo.MouseWheelLeft();
                }

                e.Handled = true;
            }

            if (Orient.HasFlag(OrientType.Vertical)
                && !isShiftKeyDown)
            {
                if (e.Delta < 0)
                {
                    ScrollInfo.MouseWheelDown();
                }
                else
                {
                    ScrollInfo.MouseWheelUp();
                }

                e.Handled = true;
            }
        }
    }
}
