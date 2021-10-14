using System;
using System.Globalization;
using Xamarin.Forms;

namespace FoodMaster.Converters
{
    public class LevelImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string level)
            {
                return level == "Fácil" ? "easy.png" : level == "Intermedio" ? "medium.png" : "high.png";
            }

            return "unknow.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
