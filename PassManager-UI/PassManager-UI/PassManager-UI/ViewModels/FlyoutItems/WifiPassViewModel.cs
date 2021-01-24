using System;
using PassManager.Models.Items;
using PassManager.Enums;
using System.Threading.Tasks;
using PassManager.ViewModels.Bases;

namespace PassManager.ViewModels.FlyoutItems
{
    public class WifiPassViewModel : BaseListItemVM
    {
        public WifiPassViewModel() : base("Wifi")
        {
            if (IsInternet())
            {
                //add some data for page
            }
        }

        private protected override Task GetDataAsync()
        {
            throw new NotImplementedException();
        }

        private protected override Task RefreshPageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
