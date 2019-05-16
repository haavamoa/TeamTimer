using LightInject;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using TeamTimer.Views;
using Xamarin.Forms;

namespace TeamTimer
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterViewModels(serviceRegistry);
            RegisterViews(serviceRegistry);
        }

        private INavigation CreateSingletonNavigation(IServiceFactory container)
        {
            var navigation = container.GetInstance<MainPage>().Navigation;
            return navigation;
        }

        private void RegisterViews(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<MainPage>();
        }

        private void RegisterViewModels(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMainViewModel, MainViewModel>();
            serviceRegistry.Register<IMatchViewModel, MatchViewModel>();
        }
    }
}