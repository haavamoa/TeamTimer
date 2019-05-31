using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeamTimer.Resources.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, bool> m_canExecute;
        private readonly Func<object, Task> m_command;

        public AsyncCommand(Func<object, Task> command, Func<object, bool> canExecute = null)
        {
            m_command = command;
            m_canExecute = canExecute ?? (o => true);
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public async Task ExecuteAsync(object parameter)
        {
            await m_command(parameter);
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(null, new EventArgs());
        }
    }
}