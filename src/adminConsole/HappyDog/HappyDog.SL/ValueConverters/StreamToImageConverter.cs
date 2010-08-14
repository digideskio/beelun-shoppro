using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;
using HappyDog.SL.Common;


namespace HappyDog.SL.ValueConverters
{
    public class StreamToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ShopproHelper.CreateThumbnailImage((Stream)value, 60); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("UriToImageConverter convert back is not implemented.");
        }
    }
}
