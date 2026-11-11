using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Views;

public class SettingsNavView : INavView
{
    public IconType IconType => IconType.SettingLine;

    public LocKey NameLocKey => LocKey.Nav_Settings;

    public NavMenuPosition NavMenuPosition => NavMenuPosition.Bottom;

    public int NavMenuOrder => 98;

    public UserControl CreateView()
    {
        return new SettingsView();
    }
}