﻿using System;
using LightInject;
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
        private readonly IMainViewModel m_mainViewModel;
        private DateTime? m_whenSleptDateTime;

        public App()
        {
            InitializeComponent();

            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterFrom<CompositionRoot>();
            var navigation = container.GetInstance<INavigationService>();
            navigation.RegisterNavigation(container.GetInstance<IMainViewModel>(), container.GetInstance<MainPage>());
            navigation.RegisterNavigation(container.GetInstance<IMatchViewModel>(), container.GetInstance<MatchPage>());
            m_mainViewModel = container.GetInstance<IMainViewModel>();
            MainPage = new NavigationPage(container.GetInstance<MainPage>());
        }

        protected override void OnStart()
        {
            //Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            if (m_mainViewModel.MatchViewModel.IsMatchStarted)
            {
                m_mainViewModel.MatchViewModel.StopwatchService.Pause();
                m_whenSleptDateTime = DateTime.Now;
            }
        }

        protected override void OnResume()
        {
            if (m_mainViewModel.MatchViewModel.IsMatchStarted)
            {
                // Handle when your app resumes
                var whenResumedDateTime = DateTime.Now;
                var elapsed = whenResumedDateTime - m_whenSleptDateTime;
                if (elapsed.HasValue)
                {
                    m_mainViewModel.MatchViewModel.UpdateMatchDuration((int)elapsed.Value.TotalSeconds);
                    m_mainViewModel.MatchViewModel.StopwatchService.Start();
                }
            }
        }
    }
}