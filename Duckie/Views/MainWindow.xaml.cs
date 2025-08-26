using Duckie.Shared;
using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views;
using Duckie.Views.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Duckie;

public partial class MainWindow : Window, IMainWindow
{
    private bool _isSidebarCollapsed = false;
    private const double CollapsedWidth = 54;
    private const double ExpandedWidth = 120;

    private NavModel[] navModels = Array.Empty<NavModel>();

    private class NavModel
    {
        public NavigationMenuItem NavMenuItem { get; set; }
        public INavView NavView { get; set; }
        public UserControl UserControl { get; set; }
    }

    public MainWindow()
    {
        AppEnv.MainWindow = this;

        InitializeComponent();

        var navViews = ReflectUtils.Get<INavView>();
        navModels = navViews
            .OrderBy(x => x.NavMenuPosition)
            .ThenBy(x => x.NavMenuOrder)
            .Select(CreateNavModel)
            .ToArray();

        RefreshNavMenu(navModels.FirstOrDefault()?.NavMenuItem);

        // 添加关闭事件处理
        this.Closing += MainWindow_Closing;
    }

    private NavModel CreateNavModel(INavView navView)
    {
        var navMenuItem = new NavigationMenuItem();
        navMenuItem.Click += (_, _) =>
        {
            RefreshNavMenu(navMenuItem);
        };

        var view = navView.CreateView();
        navViewContainer.Children.Add(view);

        switch (navView.NavMenuPosition)
        {
            case NavMenuPosition.Top:
                topNavMenuContainer.Children.Add(navMenuItem);
                break;
            case NavMenuPosition.Bottom:
                bottomNavMenuContainer.Children.Add(navMenuItem);
                break;
        }

        return new NavModel
        {
            NavMenuItem = navMenuItem,
            NavView = navView,
            UserControl = view
        };
    }

    private void RefreshNavMenu(NavigationMenuItem selectedNavMenuItem)
    {
        foreach (var navModel in navModels)
        {
            var isSelected = selectedNavMenuItem == navModel.NavMenuItem;
            navModel.NavMenuItem.IsSelected = isSelected;
            navModel.UserControl.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;

            navModel.NavMenuItem.IconType = navModel.NavView.IconType;
            navModel.NavMenuItem.Text = navModel.NavView.NameLocKey.Text();
        }
    }

    public void ToggleWindow()
    {
        if (Visibility == Visibility.Visible && WindowState != WindowState.Minimized)
        {
            Hide();
        }
        else
        {
            Show();
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
            Activate();
        }
    }

    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _isSidebarCollapsed = !_isSidebarCollapsed;

        var targetWidth = _isSidebarCollapsed ? CollapsedWidth : ExpandedWidth;

        // Simple width change without complex animation
        SidebarColumn.Width = new GridLength(targetWidth);

        foreach (var navModel in navModels)
        {
            navModel.NavMenuItem.IsTextVisible = !_isSidebarCollapsed;
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        var appSettings = UserConfigService.GetAppSettings();
        
        if (appSettings.MinimizeToTrayOnClose)
        {
            // 最小化到托盘而不是退出
            e.Cancel = true;
            Hide();
        }
    }
}