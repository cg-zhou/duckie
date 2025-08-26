using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Views;

public class AboutNavView : INavView
{
    public IconType IconType => IconType.InformationLine;

    public LocKey NameLocKey => LocKey.Nav_About;

    public NavMenuPosition NavMenuPosition => NavMenuPosition.Bottom;

    public int NavMenuOrder => 99;

    public UserControl CreateView()
    {
        return new AboutView();
    }
}
