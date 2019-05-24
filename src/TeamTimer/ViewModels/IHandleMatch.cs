using System;

namespace TeamTimer.ViewModels
{
    public interface IHandleMatch
    {
        void OnPlayerChanged(PlayerViewModel changedPlayer);
        void OnPlayerMarkedForSub(PlayerViewModel markedPlayer);
    }
}