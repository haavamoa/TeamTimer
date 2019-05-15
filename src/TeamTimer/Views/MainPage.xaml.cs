﻿using TeamTimer.ViewModels.Interfaces;
using Xamarin.Forms;

namespace TeamTimer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IMainViewModel mainViewModel)
        {
            InitializeComponent();

            BindingContext = mainViewModel;
        }
    }
}
