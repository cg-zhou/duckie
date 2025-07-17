using Duckie.Utils;
using System.Windows;

namespace Duckie
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Toggle()
        {
            if (Visibility == Visibility.Visible)
            {
                Hide();
            }
            else
            {
                Show();
                Activate();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyIconUtils.Initialize();
        }
    }
}