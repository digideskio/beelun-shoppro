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
    /// template -> template.name(string)
    /// </summary>
    public class TemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                template theBrand = (template)value;
                return theBrand.name;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("TemplateConverter.ConvertBack()");
        }
    }
}
