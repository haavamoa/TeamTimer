using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Resources.Converters
{
    public class IsListEmptyConverter : IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public bool Inverted { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IList listValue))
            {
                throw new ArgumentException("Value has to be of type IList");
            }

            if (Inverted)
            {
                return listValue.Count > 0;
            }
            return listValue.Count == 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}