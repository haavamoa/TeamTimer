using System.Windows.Input;
using Xamarin.Forms;

namespace TeamTimer.Effects
{
    public class TapOrTapAndHoldEffect : RoutingEffect
    {
        public static readonly BindableProperty TapAndHoldCommandProperty = BindableProperty.CreateAttached(
            "TapAndHoldCommand",
            typeof(ICommand),
            typeof(TapOrTapAndHoldEffect),
            null);

        public static readonly BindableProperty TapAndHoldCommandParameterProperty = BindableProperty.CreateAttached(
            "TapAndHoldCommandParameter",
            typeof(object),
            typeof(TapOrTapAndHoldEffect),
            null);

        public static readonly BindableProperty TapCommandProperty = BindableProperty.CreateAttached(
            "TapCommand",
            typeof(ICommand),
            typeof(TapOrTapAndHoldEffect),
            null);

        public static readonly BindableProperty TapCommandParameterProperty = BindableProperty.CreateAttached(
            "TapCommandParameter",
            typeof(object),
            typeof(TapOrTapAndHoldEffect),
            null);

        public TapOrTapAndHoldEffect() : base("TeamTimer.TapOrTapAndHoldEffect")
        {
        }

        public static ICommand GetTapAndHoldCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(TapAndHoldCommandProperty);
        }

        public static void SetTapAndHoldCommand(BindableObject view, ICommand value)
        {
            view.SetValue(TapAndHoldCommandProperty, value);
        }

        public static object GetTapAndHoldCommandParameter(BindableObject view)
        {
            return view.GetValue(TapAndHoldCommandParameterProperty);
        }

        public static void SetTapAndHoldCommandParameter(BindableObject view, object value)
        {
            view.SetValue(TapAndHoldCommandParameterProperty, value);
        }

        public static ICommand GetTapCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(TapCommandProperty);
        }

        public static void SetTapCommand(BindableObject view, ICommand value)
        {
            view.SetValue(TapCommandProperty, value);
        }

        public static object GetTapCommandParameter(BindableObject view)
        {
            return view.GetValue(TapCommandParameterProperty);
        }

        public static void SetTapCommandParameter(BindableObject view, object value)
        {
            view.SetValue(TapCommandParameterProperty, value);
        }
    }
}