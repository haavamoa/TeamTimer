using System;
using System.Globalization;
using TeamTimer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Resources.Converters
{
    public class IsSamePlayerConverter: IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is PlayerViewModel playerValue))
            {
                throw new ArgumentException($"Value has to be {nameof(PlayerViewModel)}");
            }

            if (!(parameter is PlayerViewModel playerParameter))
            {
                throw new ArgumentException($"parameter has to be {nameof(PlayerViewModel)}");
            }

            return Inverted ? !playerValue.Equals(playerParameter) : playerValue.Equals(playerParameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}