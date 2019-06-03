using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamTimer.Helpers;
using TeamTimer.Models;
using TeamTimer.Resources.Commands;
using TeamTimer.Services.Dialog;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.Handlers;
using Xamarin.Forms;

namespace TeamTimer.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private readonly IDialogService m_dialogService;
        private readonly IProfilerService m_profilerService;
        private readonly IHandleMatch m_matchHandler;
        private readonly Player m_player;
        private readonly IHandleTeamSetup m_teamSetupHandler;
        private bool m_isLocked;
        private bool m_isMarkedForSubstitution;
        private bool m_isPlaying;
        private int m_playTimeInSeconds;

        public PlayerViewModel(Player player, IHandleTeamSetup teamSetupHandler, IHandleMatch matchHandler, IDialogService dialogService, 
        IProfilerService profilerService)
        {
            m_player = player;
            m_teamSetupHandler = teamSetupHandler;
            m_matchHandler = matchHandler;
            m_dialogService = dialogService;
            m_profilerService = profilerService;
            DeletePlayerCommand = new Command(_ => m_teamSetupHandler?.OnPlayerDeleted(this));
            OpenInformationCommand = new AsyncCommand(_ => OpenInformation());
            MarkedForSubstitutionCommand = new Command(MarkForSubstitution);
        }

        private void MarkForSubstitution()
        {
            if (!IsLocked)
            {
                m_matchHandler.OnPlayerMarkedForSub(this);
            }
        }

        public ICommand MarkedForSubstitutionCommand { get; private set; }

        public string Name => m_player.Name;

        public ICommand DeletePlayerCommand { get; private set; }

        public bool IsPlaying
        {
            get => m_isPlaying;
            set
            {
                SetProperty(ref m_isPlaying, value);
                m_teamSetupHandler.OnPlayerChanged(this);
                m_matchHandler.OnPlayerChanged(this);
            }
        }

        public bool IsMarkedForSubstitution
        {
            get => m_isMarkedForSubstitution;
            set => SetProperty(ref m_isMarkedForSubstitution, value);
        }

        public int PlayTimeInSeconds
        {
            get => m_playTimeInSeconds;
            set
            {
                SetProperty(ref m_playTimeInSeconds, value);
                OnPropertyChanged(nameof(PlayTimeInString));
            }
        }

        public string PlayTimeInString => TimeSpan.FromSeconds(PlayTimeInSeconds).ToShortForm();

        public ICommand OpenInformationCommand { get; }

        public bool IsLocked
        {
            get => m_isLocked;
            set
            {
                SetProperty(ref m_isLocked, value);
                m_matchHandler.OnPlayerChanged(this);
            }
        }

        private async Task OpenInformation()
        {
            m_profilerService.RaiseEvent("User opened extra information");
            IsMarkedForSubstitution = false;

            var actions = new List<DialogAction>
            {
                !IsPlaying
                    ? new DialogAction("Mark as playing", () => IsPlaying = true)
                    : new DialogAction("Mark as non-playing", () => IsPlaying = false)
                ,
                !IsLocked
                ? new DialogAction("Lock player", () => IsLocked = true)
                : new DialogAction("Unlock player", () => IsLocked = false)
            };

            await m_dialogService.ShowActionSheet($"Options for {Name}", "Cancel", null, actions.ToArray());
        }
    }
}