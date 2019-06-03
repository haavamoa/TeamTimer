using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IDialogService m_dialogService;
        private readonly IProfilerService m_profilerService;
        private readonly Dictionary<IViewModel, Page> m_navigationRegister;

        public NavigationService(IDialogService dialogService, IProfilerService profilerService)
        {
            m_dialogService = dialogService;
            m_profilerService = profilerService;
            m_navigationRegister = new Dictionary<IViewModel, Page>();
        }

        public async Task NavigateTo<TViewModel>() where TViewModel : IViewModel
        {
            try
            {
                foreach (var keyValuePair in m_navigationRegister)
                {
                    var keyType = keyValuePair.Key.GetType();
                    var interfaces = keyType.GetInterfaces();
                    if (interfaces.Contains(typeof(TViewModel)))
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(keyValuePair.Value);
                    }
                }
            }
            catch (Exception exception)
            {
               await m_dialogService.ShowAlert("Something went wrong when navigating", exception.Message, "Got it!", "Cancel");
               m_profilerService.RaiseError(exception);
            }
        }

        public async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public void RegisterNavigation(IViewModel viewmodel, Page page)
        {
            m_navigationRegister.Add(viewmodel, page);
        }
    }
}
