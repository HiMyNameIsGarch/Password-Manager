using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PassManager;
using PassManager.Views;
using PassManager.ViewModels;

namespace PassManager_UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainView();
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
