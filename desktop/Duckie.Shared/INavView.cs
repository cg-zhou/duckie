using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Shared;

public enum NavMenuPosition
{
    Top,
    Bottom
}

public interface INavView
{
    UserControl CreateView();
    IconType IconType { get; }
    LocKey NameLocKey { get; }
    NavMenuPosition NavMenuPosition { get; }
    int NavMenuOrder { get; }
}