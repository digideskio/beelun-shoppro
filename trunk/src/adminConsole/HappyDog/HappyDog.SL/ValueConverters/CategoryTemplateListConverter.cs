using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Collections.Generic;
using HappyDog.SL.Data;


namespace HappyDog.SL.ValueConverters
{
    public class CategoryTemplateListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<template> allTemplates = ModelProvider.Instance.MDC.allCategoryTemplates;

            List<string> retList = new List<string>();

            foreach (template theTemplate in allTemplates)
            {
                retList.Add(theTemplate.name);
            }

            return retList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ItemTemplateListConverter.ConvertBack()");
        }

    }
}
