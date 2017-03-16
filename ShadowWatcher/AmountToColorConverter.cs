using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ShadowWatcher
{
    public class AmountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            if (parameter == null || (string)parameter != "Reverse")
            {
                if (val > 2)
                    return new SolidColorBrush(Colors.Red);
                else if (val == 2)
                    return new SolidColorBrush(Colors.Orange);
                else if (val < 0)
                    return new SolidColorBrush(Colors.Blue);
                else if (val == 0)
                    return new SolidColorBrush(Colors.LightGray);
                else
                    return new SolidColorBrush(Colors.Black);
            }
            else
            {
                if (val == 0)
                    return new SolidColorBrush(Colors.Red);
                else if (val == 1)
                    return new SolidColorBrush(Colors.Orange);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
