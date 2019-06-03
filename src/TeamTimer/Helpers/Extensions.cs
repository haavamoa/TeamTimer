using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TeamTimer.ViewModels;
using Xamarin.Forms.Internals;

namespace TeamTimer.Helpers
{
    static class Extensions
    {
        public static string ToShortForm(this TimeSpan t)
        {
            var shortForm = "";
            if (t.Hours > 0)
            {
                shortForm += string.Format("{0}h", t.Hours.ToString())+" ";
            }
            if (t.Minutes > 0)
            {
                shortForm += string.Format("{0}m", t.Minutes.ToString())+" ";
            }
            if (t.Seconds > 0)
            {

                shortForm += string.Format("{0}s", t.Seconds.ToString());
            }
            return shortForm;
        }

        public static void MoveLockedToEnd(this List<PlayerViewModel> listToMoveIn)
        {
            var lockedPlayers = listToMoveIn.Where(p => p.IsLocked).ToList();
            lockedPlayers.ForEach(lp => listToMoveIn.Remove(lp));
            lockedPlayers.ForEach(listToMoveIn.Add);
        }
        
        public static void DeMarkEveryoneExcept(this IEnumerable<PlayerViewModel> listOfPlayers, PlayerViewModel thePlayerToIgnore)
        {
            listOfPlayers.Where(p => !p.Equals(thePlayerToIgnore)).ForEach(p => p.IsMarkedForSubstitution = false);
        }

        public static void AddPlayersToExisting(this ObservableCollection<PlayerViewModel> listOfPlayers, IList<PlayerViewModel> newListOfPlayers)
        {
            if (listOfPlayers.Any())
            {
                foreach (var newNonPlayingPlayer in newListOfPlayers.Where(n => !listOfPlayers.Contains(n)))
                {
                    listOfPlayers.Add(newNonPlayingPlayer);
                }
            }
            else
            {
                listOfPlayers = new ObservableCollection<PlayerViewModel>(newListOfPlayers);
            }
        }
    }
}