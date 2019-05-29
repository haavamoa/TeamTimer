using System.Threading.Tasks;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;

namespace TeamTimer.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : IViewModel;
        Task NavigateBack();
        void RegisterNavigation(IViewModel viewmodel, Page page);
    }
}