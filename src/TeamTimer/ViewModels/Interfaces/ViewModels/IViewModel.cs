using System;
using System.ComponentModel;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IViewModel : IDisposable, INotifyPropertyChanged
    {
        string Title { get; }
        bool IsBusy { get; set; }
    }
}