using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views.Common
{
    internal class TextEx : TextBox
    {
        public TextEx()
        {
            IsReadOnly = true;
            BorderThickness = new Thickness(0);
        }
    }
}
