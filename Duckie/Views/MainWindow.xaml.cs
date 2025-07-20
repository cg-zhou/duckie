using Duckie.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Duckie
{
    public partial class MainWindow : Window
    {
        private bool _isSidebarCollapsed = false;
        private const double CollapsedWidth = 60;
        private const double ExpandedWidth = 120;

        public MainWindow()
        {
            InitializeComponent();
            UpdateNavigationState();
            ShowImageProcessing(); // Set default view
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
        /// Toggle sidebar collapse/expand
        /// </summary>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isSidebarCollapsed = !_isSidebarCollapsed;

            var targetWidth = _isSidebarCollapsed ? CollapsedWidth : ExpandedWidth;

            // Simple width change without complex animation
            SidebarColumn.Width = new GridLength(targetWidth);

            // Handle text visibility
            NavImageProcessing.IsTextVisible = !_isSidebarCollapsed;
            NavPacManagement.IsTextVisible = !_isSidebarCollapsed;
            NavAbout.IsTextVisible = !_isSidebarCollapsed;

            // Update toggle icon
            UpdateToggleIcon();
        }

        /// <summary>
        /// Update toggle button icon based on sidebar state
        /// </summary>
        private void UpdateToggleIcon()
        {
            try
            {
                var toggleIcon = FindName("ToggleIcon") as Path;
                if (toggleIcon != null)
                {
                    var iconData = _isSidebarCollapsed
                        ? "M9,5 L15,12 L9,19" // Right arrow (expand)
                        : "M15,5 L9,12 L15,19"; // Left arrow (collapse)

                    toggleIcon.Data = Geometry.Parse(iconData);
                }
            }
            catch
            {
                // Ignore icon update errors
            }
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
        /// Update navigation button states with visual feedback
        /// </summary>
        private void UpdateNavigationState()
        {
            // Reset all navigation buttons
            NavImageProcessing.IsSelected = false;
            NavPacManagement.IsSelected = false;
            NavAbout.IsSelected = false;
        }



        /// <summary>
        /// Show image processing interface
        /// </summary>
        private void ShowImageProcessing()
        {
            ImageViewControl.Visibility = Visibility.Visible;
            PacManageViewControl.Visibility = Visibility.Collapsed;

            // Update navigation state
            UpdateNavigationState();
            NavImageProcessing.IsSelected = true;

            Title = "Duckie - Image Processing";
        }

        private void ShowPacManagement()
        {
            ImageViewControl.Visibility = Visibility.Collapsed;
            PacManageViewControl.Visibility = Visibility.Visible;

            // Update navigation state
            UpdateNavigationState();
            NavPacManagement.IsSelected = true;

            Title = "Duckie - PAC Management";
        }
    }
}