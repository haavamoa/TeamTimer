using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchPage : ContentPage
    {
        public MatchPage(IMatchViewModel matchViewModel)
        {
            InitializeComponent();

            BindingContext = matchViewModel;
        }
    }
}