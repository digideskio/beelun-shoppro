using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;
using HappyDog.SL.ViewModels;


namespace HappyDog.SL.ValueConverters
{
    /// <summary>
    /// template.name <--> template(name is unique)
    /// </summary>
    public class TemplateIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return ((template)value).name;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                foreach (template theBrand in ModelProvider.Instance.MDC.allItemTemplates)
                {
                    if (theBrand.name == (string)value)
                    {
                        return (theBrand);
                    }
                }

                foreach (template theBrand in ModelProvider.Instance.MDC.allCategoryTemplates)
                {
                    if (theBrand.name == (string)value)
                    {
                        return (theBrand);
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

    }
}
