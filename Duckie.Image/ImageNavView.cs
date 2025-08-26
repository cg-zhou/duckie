using Duckie.Image.Views;
using Duckie.Shared;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Views.Common;
using System.Windows.Controls;

namespace Duckie.Image
{
    public class ImageNavView : INavView
    {
        public IconType IconType => IconType.Image;

        public LocKey NameLocKey => LocKey.Nav_Image;

        public NavMenuPosition NavMenuPosition => NavMenuPosition.Top;

        public int NavMenuOrder => 0;

        public UserControl CreateView()
        {
            return new ImageView();
        }
    }
}
