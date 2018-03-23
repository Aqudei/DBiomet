using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Biomet.Helpers
{
    public class BitmapConverter
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static BitmapSource Convert(Bitmap source)
        {
            BitmapSource bitmapSource;
            var hBitmap = source.GetHbitmap();

            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                bitmapSource.Freeze();
                DeleteObject(hBitmap);
            }
            catch (Exception)
            {
                bitmapSource = null;
            }


            return bitmapSource;
        }
    }
}
