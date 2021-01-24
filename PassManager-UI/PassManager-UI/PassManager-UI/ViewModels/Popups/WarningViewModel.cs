using PassManager.Models;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.ViewModels.Bases;

namespace PassManager.ViewModels.Popups
{
    public class WarningViewModel : BaseViewModel
    {
        public WarningViewModel(string warningText) : base(warningText)
        {
            _closePopup = new Command(async () => 
            {
                await PageService.PopPopupAsync();
            });
        }
        private ICommand _closePopup;

        public ICommand ClosePopup
        {
            get { return _closePopup; }
        }
    }
}
