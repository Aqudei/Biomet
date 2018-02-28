using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Biomet.Helpers
{
    public class BitmapConverter
    {
        public static BitmapSource Convert(Bitmap source)
        {
            BitmapSource bitmapSource;
            var hBitmap = source.GetHbitmap();

            using (new Microsoft.Win32.SafeHandles.SafeAccessTokenHandle(hBitmap))
            {
                try
                {
                    bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    bitmapSource.Freeze();
                }
                catch (Win32Exception)
                {
                    bitmapSource = null;
                }
            }

            return bitmapSource;
        }
    }
}
