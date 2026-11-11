using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Launcher;

/// <summary>
/// 快捷启动导航视图
/// </summary>
public class LauncherNavView : INavView
{
    public IconType IconType => IconType.SettingLine;

    public LocKey NameLocKey => LocKey.Nav_Launcher;

    public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

    public int NavMenuOrder => 5;

    public UserControl CreateView()
    {
        return new LauncherView();
    }
}
