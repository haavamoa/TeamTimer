using System;
using System.Globalization;
using TeamTimer.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Resources.Converters
{
    public class SecondsToStringShortFormatConverter : IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int intValue))
            {
                throw new ArgumentException("Value has to be of type int");
            }
            return TimeSpan.FromSeconds(intValue).ToShortForm();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}