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
    public class ArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            orderItem[] orderItemArray = (orderItem[])value;
            decimal totalPrice = 0;

            List<string> orderItemList = new List<string>();
            for (int i = 0; i < orderItemArray.Length; i++)
            {
                orderItemList.Add(string.Format("Item {0}: {1}, Price: {2}.", i + 1, orderItemArray[i].item.name, orderItemArray[i].item.sellPrice));
                totalPrice += orderItemArray[i].item.sellPrice;
            }
            orderItemList.Add("------");
            orderItemList.Add(string.Format("Total Item: {0}. Total Price: {1}", orderItemArray.Length, totalPrice));

            return orderItemList;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ArrayConverter.ConvertBack()");
        }
    }
}
