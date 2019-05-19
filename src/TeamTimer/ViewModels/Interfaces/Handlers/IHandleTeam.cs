namespace TeamTimer.ViewModels.Interfaces.Handlers
{
    public interface IHandleTeam
    {
        void OnPlayerDeleted(PlayerViewModel deletedPlayer);
        void OnPlayerChanged(PlayerViewModel changedPlayer);
    }
}