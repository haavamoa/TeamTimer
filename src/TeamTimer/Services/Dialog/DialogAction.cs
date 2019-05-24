using System;

namespace TeamTimer.Services.Dialog
{
    public class DialogAction
    {
        /// <summary>
        /// The text on the action inside the dialog
        /// </summary>
        public string ButtonText { get; }
        /// <summary>
        /// The action to run when the user has clicked the button for this dialog action
        /// </summary>
        public Action Action { get; }

        public DialogAction(string buttonText, Action action)
        {
            ButtonText = buttonText;
            Action = action;
        }
    }
}