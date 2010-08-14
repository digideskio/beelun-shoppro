using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace HappyDog.SL.ValueConverters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal currency = (decimal)value;
            return currency.ToString("C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedValue = value.ToString().TrimStart('$');
            decimal currency;

            if (decimal.TryParse(formattedValue, out currency))
                return currency;
            else
                return DependencyProperty.UnsetValue;
        }
    }
}
