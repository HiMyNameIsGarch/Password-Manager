using System;
using System.Collections.Generic;
using System.Text;

namespace PassManager.Models.Interfaces
{
    public interface IBackButtonBehavior
    {
        System.Windows.Input.ICommand GoBack { get; }
        void GoBackButton();
    }
}
