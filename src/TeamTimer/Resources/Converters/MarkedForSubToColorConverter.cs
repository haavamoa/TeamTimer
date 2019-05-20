using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Resources.Converters
{
    public class MarkedForSubToColorConverter : IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return((bool)value) ? (Color)App.Current.Resources["ColorPrimary"] : (Color)App.Current.Resources["ColorAccent"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}