namespace TeamTimer.ViewModels.Interfaces.Handlers
{
    public interface IHandleTeamSetup
    {
        void OnPlayerDeleted(PlayerViewModel deletedPlayer);
        void OnPlayerChanged(PlayerViewModel changedPlayer);
    }
}