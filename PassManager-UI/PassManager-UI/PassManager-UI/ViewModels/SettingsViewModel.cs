using PassManager.Models;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

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
    public class SettingModel
    {
        public SettingModel(string text, ICommand command)
        {
            Text = text;
            WhenClicked = command;
        }
        public string Text { get; set; }
        public ICommand WhenClicked { get; set; }
    }
}
