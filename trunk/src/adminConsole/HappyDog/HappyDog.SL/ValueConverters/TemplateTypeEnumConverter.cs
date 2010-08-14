using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;

namespace HappyDog.SL.ValueConverters
{
    public class TemplateTypeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<brand> allBrands = ModelProvider.Instance.MDC.allBrands;

            List<string> brandList = new List<string>();


            foreach (brand theBrand in allBrands)
            {
                brandList.Add(theBrand.name);
            }

            return brandList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BrandConverter.ConvertBack()");
        }
    }
}
