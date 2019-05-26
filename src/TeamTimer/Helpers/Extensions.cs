using System;
using System.Collections.Generic;
using System.Linq;
using TeamTimer.ViewModels;

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
    }
}