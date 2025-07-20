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

        /// <summary>
        /// Switch to image processing function
        /// </summary>
        private void MenuImageProcessing_Click(object sender, RoutedEventArgs e)
        {
            ShowImageProcessing();
        }

        /// <summary>
        /// Switch to PAC management function
        /// </summary>
        private void MenuPacManagement_Click(object sender, RoutedEventArgs e)
        {
            ShowPacManagement();
        }

        /// <summary>
        /// Show about information
        /// </summary>
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            UiUtils.Info("Duckie - Image Processing and PAC Management Tool\nVersion: 1.0", "About");
        }

        /// <summary>
        /// Show image processing interface
        /// </summary>
        private void ShowImageProcessing()
        {
            ImageViewControl.Visibility = Visibility.Visible;
            PacManageViewControl.Visibility = Visibility.Collapsed;

            MenuImageProcessing.IsChecked = true;
            MenuPacManagement.IsChecked = false;

            Title = "Duckie - Image Processing";
        }

        /// <summary>
        /// Show PAC management interface
        /// </summary>
        private void ShowPacManagement()
        {
            ImageViewControl.Visibility = Visibility.Collapsed;
            PacManageViewControl.Visibility = Visibility.Visible;

            MenuImageProcessing.IsChecked = false;
            MenuPacManagement.IsChecked = true;

            Title = "Duckie - PAC Management";
        }
    }
}