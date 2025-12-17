using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Terminal;

/// <summary>
/// 终端管理导航视图
/// </summary>
public class TerminalManagerNavView : INavView
{
    public IconType IconType => IconType.SettingLine;

    public LocKey NameLocKey => LocKey.Nav_Terminal;

    public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

    public int NavMenuOrder => 5;

    public UserControl CreateView()
    {
        return new TerminalManagerView();
    }
}
