using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;


namespace HappyDog.SL.ValueConverters
{
    /// <summary>
    /// Below code is borrowed from:
    /// http://stackoverflow.com/questions/1281490/binding-comboboxes-to-enums-in-silverlight
    /// </summary>

    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.Parse(targetType, value.ToString(), true);
        }
    }

    public class EnumToIEnumerableConverter : IValueConverter
    {
        private Dictionary<Type, List<object>> cache = new Dictionary<Type, List<object>>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value.GetType();
            if (!this.cache.ContainsKey(type))
            {
                var fields = type.GetFields().Where(field => field.IsLiteral);
                var values = new List<object>();
                foreach (var field in fields)
                {
                    DescriptionAttribute[] a = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (a != null && a.Length > 0)
                    {
                        values.Add(a[0].Description);
                    }
                    else
                    {
                        values.Add(field.GetValue(value));
                    }
                }
                this.cache[type] = values;
            }

            return this.cache[type];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
