﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PassManager;
using PassManager.Enums;
using PassManager.Pages;
using PassManager.ViewModels;

namespace PassManager_UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            PageService.SetMainPage(MainPage);
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
