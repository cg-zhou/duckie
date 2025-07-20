using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Duckie.Views.Controls
{
    /// <summary>
    /// Navigation menu item component
    /// </summary>
    public partial class NavigationMenuItem : UserControl
    {
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(string), typeof(NavigationMenuItem), 
                new PropertyMetadata(string.Empty, OnIconDataChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NavigationMenuItem), 
                new PropertyMetadata(string.Empty, OnTextChanged));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NavigationMenuItem), 
                new PropertyMetadata(false, OnIsSelectedChanged));

        public static readonly DependencyProperty IsTextVisibleProperty =
            DependencyProperty.Register("IsTextVisible", typeof(bool), typeof(NavigationMenuItem), 
                new PropertyMetadata(true, OnIsTextVisibleChanged));

        public event EventHandler<RoutedEventArgs> Click;

        public NavigationMenuItem()
        {
            InitializeComponent();
        }

        public string IconData
        {
            get { return (string)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsTextVisible
        {
            get { return (bool)GetValue(IsTextVisibleProperty); }
            set { SetValue(IsTextVisibleProperty, value); }
        }

        private static void OnIconDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationMenuItem control)
            {
                control.UpdateIcon();
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationMenuItem control)
            {
                control.UpdateText();
            }
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationMenuItem control)
            {
                control.UpdateSelectedState();
            }
        }

        private static void OnIsTextVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationMenuItem control)
            {
                control.UpdateTextVisibility();
            }
        }

        private void UpdateIcon()
        {
            if (!string.IsNullOrEmpty(IconData))
            {
                try
                {
                    IconPath.Data = Geometry.Parse(IconData);
                }
                catch
                {
                    // Ignore invalid geometry
                }
            }
        }

        private void UpdateText()
        {
            TextBlock.Text = Text ?? string.Empty;
        }

        private void UpdateSelectedState()
        {
            try
            {
                var template = MenuButton.Template;
                if (template != null)
                {
                    MenuButton.ApplyTemplate();
                    var indicator = template.FindName("SelectionIndicator", MenuButton) as Rectangle;
                    if (indicator != null)
                    {
                        indicator.Visibility = IsSelected ? Visibility.Visible : Visibility.Collapsed;
                    }
                }

                if (IsSelected)
                {
                    MenuButton.Background = new SolidColorBrush(Color.FromRgb(0xE3, 0xF2, 0xFD));
                    IconPath.Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC));
                    TextBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC));
                }
                else
                {
                    MenuButton.Background = Brushes.Transparent;
                    IconPath.Fill = new SolidColorBrush(Color.FromRgb(0x49, 0x50, 0x57));
                    TextBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x49, 0x50, 0x57));
                }
            }
            catch
            {
                // Fallback: simple background change
                MenuButton.Background = IsSelected ? 
                    new SolidColorBrush(Color.FromRgb(0xE3, 0xF2, 0xFD)) : 
                    Brushes.Transparent;
            }
        }

        private void UpdateTextVisibility()
        {
            TextBlock.Visibility = IsTextVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
