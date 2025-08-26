using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using Duckie.Views;
using System.Windows.Controls;

namespace Duckie.Windows
{
    public class HotkeyManagerNavView : INavView
    {
        public IconType IconType => IconType.SettingLine;

        public LocKey NameLocKey => LocKey.Nav_Hotkey;

        public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

        public int NavMenuOrder => 1;

        public UserControl CreateView()
        {
            return new HotkeyManagerView();
        }
    }
}
