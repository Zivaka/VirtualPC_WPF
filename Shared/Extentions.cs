using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Shared
{
    public static class BitmapConversion
    {
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                source.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
