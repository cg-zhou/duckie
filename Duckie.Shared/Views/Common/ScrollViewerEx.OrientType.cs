namespace Duckie.Views.Common;

partial class ScrollViewerEx
{
    [Flags]
    public enum OrientType
    {
        None = 0b00,
        Vertical = 0b01,
        Horizontal = 0b10,
        Both = 0b11
    }
}
