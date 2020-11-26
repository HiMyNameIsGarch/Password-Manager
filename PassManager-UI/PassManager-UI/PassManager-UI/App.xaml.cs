using Xamarin.Forms;
using PassManager.Models;
using PassManager.Views;

namespace PassManager_UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AuthenticationView();
            PageService.SetMainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
