﻿using Duckie.Utils.Localization;
using Duckie.Utils.Ui;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Duckie
{
    public partial class MainWindow : Window
    {
        private bool _isSidebarCollapsed = false;
        private const double CollapsedWidth = 54;
        private const double ExpandedWidth = 120;

        public MainWindow()
        {
            InitializeComponent();
            UpdateNavigationState();
            ShowImageProcessing();
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
            ShowAbout();
        }

        /// <summary>
        /// Show settings page
        /// </summary>
        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        /// <summary>
        /// Update navigation button states with visual feedback
        /// </summary>
        private void UpdateNavigationState()
        {
            // Reset all navigation buttons
            NavImageProcessing.IsSelected = false;
            NavPacManagement.IsSelected = false;
            NavSettings.IsSelected = false;
            NavAbout.IsSelected = false;
        }



        /// <summary>
        /// Show image processing interface
        /// </summary>
        private void ShowImageProcessing()
        {
            ImageViewControl.Visibility = Visibility.Visible;
            PacManageViewControl.Visibility = Visibility.Collapsed;
            SettingsViewControl.Visibility = Visibility.Collapsed;
            AboutViewControl.Visibility = Visibility.Collapsed;

            // Update navigation state
            UpdateNavigationState();
            NavImageProcessing.IsSelected = true;

            Title = LocUtils.GetString("Title_Image");
        }

        private void ShowPacManagement()
        {
            ImageViewControl.Visibility = Visibility.Collapsed;
            PacManageViewControl.Visibility = Visibility.Visible;
            SettingsViewControl.Visibility = Visibility.Collapsed;
            AboutViewControl.Visibility = Visibility.Collapsed;

            // Update navigation state
            UpdateNavigationState();
            NavPacManagement.IsSelected = true;

            Title = LocUtils.GetString("Title_PAC");
        }

        private void ShowSettings()
        {
            ImageViewControl.Visibility = Visibility.Collapsed;
            PacManageViewControl.Visibility = Visibility.Collapsed;
            SettingsViewControl.Visibility = Visibility.Visible;
            AboutViewControl.Visibility = Visibility.Collapsed;

            // Update navigation state
            UpdateNavigationState();
            NavSettings.IsSelected = true;

            Title = LocUtils.GetString("Title_Settings");
        }

        private void ShowAbout()
        {
            ImageViewControl.Visibility = Visibility.Collapsed;
            PacManageViewControl.Visibility = Visibility.Collapsed;
            SettingsViewControl.Visibility = Visibility.Collapsed;
            AboutViewControl.Visibility = Visibility.Visible;

            // Update navigation state
            UpdateNavigationState();
            NavAbout.IsSelected = true;

            Title = LocUtils.GetString("Title_About");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!UiUtils.Confirm("Are you sure to exit?", "Exit"))
            {
                e.Cancel = true;
            }
        }
    }
}