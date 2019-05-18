using System.Threading.Tasks;
using TeamTimer.ViewModels.Interfaces.ViewModels;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : IMatchViewModel
    {
        public Task Initialize()
        {
            return Task.CompletedTask;
        }
    }
}