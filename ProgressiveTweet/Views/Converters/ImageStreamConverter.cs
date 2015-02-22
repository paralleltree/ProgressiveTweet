using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ProgressiveTweet.Views.Converters
{
    /// <summary>
    /// <see cref="System.IO.Stream"/>をImageコントロールで表示可能な<see cref="System.Windows.Media.Imaging.BitmapImage"/>へ変換します。
    /// </summary>
    class ImageStreamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stream = value as Stream;
            if (stream == null) return null;

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            if (image.CanFreeze) image.Freeze();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
