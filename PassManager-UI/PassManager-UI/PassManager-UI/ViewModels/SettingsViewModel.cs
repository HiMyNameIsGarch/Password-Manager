using PassManager.Models;
using PassManager.Models.Api;
using System.Collections.Generic;
using Xamarin.Forms;
using PassManager.ViewModels.Bases;

namespace PassManager.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() : base("Settings")
        {
        }
        public IEnumerable<SettingModel> AllSettings { 
            get
            {
                return new List<SettingModel>()
                {
                    new SettingModel("Delete Account", new Command( () => 
                    {

                    })),
                    new SettingModel("Sign Out", new Command( () => 
                    {
                        ApiHelper.DeleteAuthorization();
                        PageService.ChangeMainPage(new Views.AuthenticationView());
                    })),
                };
            }
        }
    }
}
