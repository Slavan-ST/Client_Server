using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public class MyConverter
    {
        public static byte[] ConverterFromImage(BitmapImage image)
        {
            MemoryStream memStream = new MemoryStream();
            Stream stream = image.StreamSource;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
