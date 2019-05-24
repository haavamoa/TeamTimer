using System;
using System.Threading.Tasks;

namespace TeamTimer.Services.Dialog.Interfaces
{
    public interface IDialogService
    {
        /// <summary>
        ///     Display a alert on the current main page
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message to display in the alert</param>
        /// <param name="accept">The text of the accept button</param>
        /// <param name="cancel">The text of the cancel button</param>
        /// <returns>A boolean value indicating whether the user clicked accept(true) or cancel(false)</returns>
        Task<bool> ShowAlert(string title, string message, string accept, string cancel);

        /// <summary>
        ///     Display a action sheet that the user where the user can make choices
        /// </summary>
        /// <param name="title">The title of the action sheet</param>
        /// <param name="cancel">The text in the cancel button</param>
        /// <param name="destruction">The text in the destruction button, can be left null if it shouldn't be visible</param>
        /// <param name="actions">The actions a user can choose amongst, which will run a <see cref="Action"/> </param>
        Task ShowActionSheet(string title, string cancel, string? destruction = null, params DialogAction[] actions);

    }
}