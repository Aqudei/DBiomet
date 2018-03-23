using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Biomet.Extentions
{
    public static class StringExtentions
    {
        public static ImageSource BitmapFromStringPath(this string source)
        {
            using (var fs = new FileStream(source, FileMode.Open))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = fs;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
    }
}
