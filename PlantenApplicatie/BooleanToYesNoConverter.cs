using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace PlantenApplicatie
{
    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "Ja";
                else
                    return "Nee";
            }
            return "no";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString().ToLower())
            {
                case "Ja":
                    return true;
                case "Nee":
                    return false;
                default:
                    return Binding.DoNothing;
            }
        }
    }
}
