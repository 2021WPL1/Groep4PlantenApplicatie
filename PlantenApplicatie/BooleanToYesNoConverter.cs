using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace PlantenApplicatie
{
    //boolean converter for the checkboxes and listview to say yes and no instead of true/false
    //Made by Davy
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
            return "Null";
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
