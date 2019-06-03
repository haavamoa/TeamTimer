using System;
using LightInject;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TeamTimer.Services.Navigation;
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
        private readonly ITeamSetupViewModel m_teamSetupViewModel;
        private DateTime? m_whenSleptDateTime;

        public App()
        {
            InitializeComponent();

            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterFrom<CompositionRoot>();
            var navigation = container.GetInstance<INavigationService>();
            navigation.RegisterNavigation(container.GetInstance<ITeamSetupViewModel>(), container.GetInstance<MainPage>());
            navigation.RegisterNavigation(container.GetInstance<IMatchViewModel>(), container.GetInstance<MatchPage>());
            m_teamSetupViewModel = container.GetInstance<ITeamSetupViewModel>();
            MainPage = new NavigationPage(container.GetInstance<MainPage>());
        }

        protected override void OnStart()
        {
            //Handle when your app starts
            AppCenter.Start("android=5ea91421-1ccb-4b2e-b52e-d4e3ae1ee82c;" +
                            "uwp={Your UWP App secret here};" +
                            "ios={Your iOS App secret here}",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            if (m_teamSetupViewModel.MatchViewModel.IsMatchStarted)
            {
                m_teamSetupViewModel.MatchViewModel.StopwatchService.Pause();
                m_whenSleptDateTime = DateTime.Now;
            }
        }

        protected override void OnResume()
        {
            if (m_teamSetupViewModel.MatchViewModel.IsMatchStarted)
            {
                // Handle when your app resumes
                var whenResumedDateTime = DateTime.Now;
                var elapsed = whenResumedDateTime - m_whenSleptDateTime;
                if (elapsed.HasValue)
                {
                    m_teamSetupViewModel.MatchViewModel.UpdateMatchDuration((int)elapsed.Value.TotalSeconds);
                    m_teamSetupViewModel.MatchViewModel.StopwatchService.Start();
                }
            }
        }
    }
}