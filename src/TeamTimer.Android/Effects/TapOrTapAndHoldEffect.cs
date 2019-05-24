using System;
using TeamTimer.Android.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ResolutionGroupName("TeamTimer")]
[assembly: ExportEffect(typeof(TapOrTapAndHoldEffect), "TapOrTapAndHoldEffect")]

namespace TeamTimer.Android.Effects
{
    public class TapOrTapAndHoldEffect : PlatformEffect
    {

        /// <summary>
        /// Initializer to avoid linking out
        /// </summary>
        public static void Initialize() { }

        public TapOrTapAndHoldEffect()
        {
            
        }
        
        protected override void OnAttached()
        {
            if (!IsAttached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += OnLongClick;
                    Control.Clickable = true;
                    Control.Click += OnClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += OnLongClick;
                    Container.Clickable = true;
                    Container.Click += OnClick;
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            var command = TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapCommand(Element);
            command?.Execute(TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapCommandParameter(Element));
        }

        private void OnLongClick(object sender, View.LongClickEventArgs e)
        {
            var command = TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapAndHoldCommand(Element);
            command?.Execute(TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapAndHoldCommandParameter(Element));
        }

        protected override void OnDetached()
        {
            if (IsAttached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= OnLongClick;
                    Control.Clickable = false;
                    Control.Click -= OnClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= OnLongClick;
                    Container.Clickable = false;
                    Container.Click -= OnClick;
                }
            }
        }
    }
}