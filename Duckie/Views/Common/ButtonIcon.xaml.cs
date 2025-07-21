using Duckie.Utils.Ui;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Duckie.Views.Common
{
    public partial class ButtonIcon : Button
    {
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(ButtonIcon), new PropertyMetadata(14.0));

        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register(nameof(IconType), typeof(IconType), typeof(ButtonIcon), new PropertyMetadata(IconType.Search));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(SolidColorBrush), typeof(ButtonIcon), new PropertyMetadata(null));

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public IconType IconType
        {
            get { return (IconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public ButtonIcon()
        {
            InitializeComponent();
        }

        private SolidColorBrush oldIconColor;

        private void Icon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            oldIconColor = IconColor;
            IconColor = ThemeUtils.PrimaryBrush;
        }

        private void Icon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            IconColor = oldIconColor;
        }
    }
}
