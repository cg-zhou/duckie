using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Views;

public class ContextMenuNavView : INavView
{
    public IconType IconType => IconType.ContextMenu;
    public LocKey NameLocKey => LocKey.Nav_ContextMenu;
    public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;
    public int NavMenuOrder => 1;
    public UserControl CreateView() => new ContextMenuView();
}
