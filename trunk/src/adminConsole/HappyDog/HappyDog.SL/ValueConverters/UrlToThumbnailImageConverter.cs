using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using HappyDog.SL.Common;

namespace HappyDog.SL.ValueConverters
{
    public class UrlToThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ShopproHelper.CreateThumbnailImage((string)value, 60); // TODO: pass width as parameter. Now it is hard-coded
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("UriToImageConverter convert back is not implemented.");
        }
    }
}
