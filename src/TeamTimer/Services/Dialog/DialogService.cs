using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TeamTimer.Services.Dialog.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.Services.Dialog
{
    public partial class DialogService : IDialogService
    {
        public async Task<bool> ShowAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task ShowActionSheet(string title, string cancel, string destruction = null, params DialogAction[] actions)
        {
            var buttons = actions.Select(a => a.ButtonText).ToArray();
            var buttonPressed = await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);

            if (buttonPressed == null || buttonPressed.Equals(cancel))
            {
                return;
            }

            try
            {
                actions.Single(a => a.ButtonText.Equals(buttonPressed)).Action.Invoke();
                Analytics.TrackEvent($"User used {buttonPressed} action from action sheet");
            }
            catch (Exception exception)
            {
                await ShowAlert("Something went wrong", exception.Message, "Got it", "Cancel");
                Crashes.TrackError(exception);
            }
        }
    }
}