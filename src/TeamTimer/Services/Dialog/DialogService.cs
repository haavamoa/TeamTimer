using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Profiling;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.Services.Dialog
{
    public partial class DialogService : IDialogService
    {
        private readonly IProfilerService m_profilerService;

        public DialogService(IProfilerService profilerService)
        {
            m_profilerService = profilerService;
        }
        
        public async Task<bool> ShowAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task ShowActionSheet(string title, string cancel, DialogAction confirmAction = null, params DialogAction[] actions)
        {
            var buttons = actions.Select(a => a.ButtonText).ToArray();
            var buttonPressed = await Application.Current.MainPage.DisplayActionSheet(title, cancel, confirmAction?.ButtonText, buttons);

            if (buttonPressed == null || buttonPressed.Equals(cancel))
            {
                return;
            }

            try
            {
                if (confirmAction != null && buttonPressed.Equals(confirmAction.ButtonText))
                {
                    confirmAction.Action.Invoke();
                    
                }
                else
                {
                    actions?.Single(a => a.ButtonText.Equals(buttonPressed)).Action.Invoke();    
                }
                m_profilerService.RaiseEvent($"User used {buttonPressed} action from action sheet");
            }
            catch (Exception exception)
            {
                await ShowAlert("Something went wrong", exception.Message, "Got it", "Cancel");
                m_profilerService.RaiseError(exception);
            }
        }
    }
}