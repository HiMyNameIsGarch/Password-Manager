using System.Windows.Input;

namespace PassManager.Models.Interfaces
{
    public interface IBackButtonBehavior
    {
        ICommand GoBack { get; }
        void GoBackButton();
    }
}
