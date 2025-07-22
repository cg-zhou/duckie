using System;

namespace Duckie.Views.Common
{
    internal partial class ScrollViewerEx
    {
        [Flags]
        internal enum OrientType
        {
            None = 0b00,
            Vertical = 0b01,
            Horizontal = 0b10,
            Both = 0b11
        }
    }
}
