using System;

namespace Duckie.Utils.HotKeys
{
    [Flags]
    internal enum KeyModifiers
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
