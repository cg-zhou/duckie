using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using Duckie.Views;
using System.Windows.Controls;

namespace Duckie.Windows
{
    public class VolumeManagerNavView : INavView
    {
        public IconType IconType => IconType.SpeakerHigh;

        public LocKey NameLocKey => LocKey.Nav_VolumeControl;

        public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

        public int NavMenuOrder => 3;

        public UserControl CreateView()
        {
            return new VolumeManagerView();
        }
    }
}
