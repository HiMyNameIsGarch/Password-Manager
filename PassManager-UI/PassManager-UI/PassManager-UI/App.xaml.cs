using Xamarin.Forms;
using PassManager.Views;
using PassManager.Models.Api;

namespace PassManager_UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AuthenticationView();
            ApiHelper.InitializeClient();
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
