using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using Duckie.Views;
using System.Windows.Controls;

namespace Duckie.Windows
{
    public class PacManageNavView : INavView
    {
        public IconType IconType => IconType.Network;

        public LocKey NameLocKey => LocKey.Nav_PAC;

        public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

        public int NavMenuOrder => 2;

        public UserControl CreateView()
        {
            return new PacManageView();
        }
    }
}
