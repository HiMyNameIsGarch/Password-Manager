using Xamarin.Forms;
using PassManager.Views;
using PassManager.Models.Api;
using System.Threading;
using PassManager.Models.Api.Processors;
using PassManager.Models;

namespace PassManager_UI
{
    public partial class App : Application
    {
        private const int TimeSpan = 300000;//5 minutes
        private Timer Timer;
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
            Timer = new Timer(LogOutUser, null, TimeSpan, 0);
        }

        protected override void OnResume()
        {
            if (Timer != null)
                Timer.Dispose();
         
            if(Shell.Current is null)
                PageService.ChangeNavBarColor(PageService.PrussianBlueColor);
        }
        private void LogOutUser(object sender)
        {
            if(Shell.Current != null)
            {
                UserProcessor.LogOut();
            }
            Timer.Dispose();
        }
    }
}
