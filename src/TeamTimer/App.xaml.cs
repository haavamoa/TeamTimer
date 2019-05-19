using System;
using LightInject;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using TeamTimer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TeamTimer
{
    public partial class App : Application
    {
        private IMainViewModel m_mainViewModel;

        public App()
        {
            InitializeComponent();

            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterFrom<CompositionRoot>();
            m_mainViewModel = container.GetInstance<IMainViewModel>();
            MainPage = new NavigationPage(container.GetInstance<MainPage>());
        }

        protected override async void OnStart()
        {
            await m_mainViewModel.Initialize(MainPage.Navigation);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
