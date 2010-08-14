using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;

namespace HappyDog.SL.ValueConverters
{
    /// <summary>
    /// Convert brand to its name
    /// </summary>
    public class BrandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                brand theBrand = (brand)value;
                return theBrand.name;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BrandConverter.ConvertBack()");
        }
    }
}
