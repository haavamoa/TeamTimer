using LightInject;
using TeamTimer.Services;
using TeamTimer.Services.Dialog;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Navigation;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using TeamTimer.Views;

namespace TeamTimer
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterServices(serviceRegistry);
            RegisterViewModels(serviceRegistry);
            RegisterViews(serviceRegistry);
            serviceRegistry.Register<INavigationService, NavigationService>(new PerContainerLifetime());
        }

        private static void RegisterServices(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IDialogService, DialogService>(new PerContainerLifetime());
            serviceRegistry.Register<IStopwatchService, StopwatchService>(new PerContainerLifetime());
        }

        private static void RegisterViews(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<MainPage>();
            serviceRegistry.Register<MatchPage>();
        }

        private static void RegisterViewModels(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMainViewModel, MainViewModel>(new PerContainerLifetime());
            serviceRegistry.Register<IMatchViewModel, MatchViewModel>(new PerContainerLifetime());
        }
    }
}