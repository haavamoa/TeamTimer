using TeamTimer.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("TeamTimer")]
[assembly: ExportEffect(typeof(TapOrTapAndHoldEffect), "TapOrTapAndHoldEffect")]
namespace TeamTimer.iOS.Effects
{
    public class TapOrTapAndHoldEffect : PlatformEffect
    {
        private readonly UILongPressGestureRecognizer m_longPressRecognizer;
        private readonly UITapGestureRecognizer m_tapRecognizer;

        public TapOrTapAndHoldEffect()
        {
            m_longPressRecognizer = new UILongPressGestureRecognizer(OnLongPress);
            m_tapRecognizer = new UITapGestureRecognizer(OnTap);
        }

        private void OnTap()
        {
            var command = TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapCommand(Element);
            command?.Execute(TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapCommandParameter(Element));
        }

        protected override void OnAttached()
        {
            if (!IsAttached)
            {
                Container.AddGestureRecognizer(m_longPressRecognizer);
                Container.AddGestureRecognizer(m_tapRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (IsAttached)
            {
                Container.RemoveGestureRecognizer(m_longPressRecognizer);
                Container.RemoveGestureRecognizer(m_tapRecognizer);
            }
        }
        
        private void OnLongPress()
        {
            var command = TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapAndHoldCommand(Element);
            command?.Execute(TeamTimer.Effects.TapOrTapAndHoldEffect.GetTapAndHoldCommandParameter(Element));
        }
    }
}