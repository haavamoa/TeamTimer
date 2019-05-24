using System;

namespace TeamTimer.Services.Dialog
{
    public class ErrorDialogAction : DialogAction
    {
        public ErrorDialogAction() : base("Something went wrong", () => {})
        {
        }
    }
}