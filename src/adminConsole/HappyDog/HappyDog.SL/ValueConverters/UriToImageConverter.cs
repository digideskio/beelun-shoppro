using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;

using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;

namespace HappyDog.SL.ValueConverters
{
    /// <summary>
    /// TODO: this will consume huge amount of memory. FIXME:
    /// Refer to: http://www.wintellect.com/CS/blogs/jprosise/archive/2009/12/17/silverlight-s-big-image-problem-and-what-you-can-do-about-it.aspx
    /// UrlToThumbnailImageConverter is one of example.
    /// </summary>
    public class UriToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            BitmapImage bi = new BitmapImage();
            bi.UriSource = (Uri)new Uri((string)value);
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("UriToImageConverter convert back is not implemented.");
        }
    }
}
