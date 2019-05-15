using LightInject;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.Views;

namespace TeamTimer
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterViewModels(serviceRegistry);
            RegisterViews(serviceRegistry);
        }

        private void RegisterViews(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<MainPage>();
        }

        private void RegisterViewModels(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMainViewModel, MainViewModel>();
        }
    }
}