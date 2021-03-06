﻿using PassManager.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;
using PassManager.Views.Popups;

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
                    new SettingModel("Generate Password", new Command(async () => 
                    {
                        await PageService.PushPopupAsync(new PasswordGeneratorView());
                    })),
                    new SettingModel("Delete Account", new Command( () => 
                    {

                    })),
                    new SettingModel("Sign Out", new Command( () => 
                    {
                        UserProcessor.LogOut();
                        PageService.ChangeNavBarColor(PageService.PrussianBlueColor);
                    })),
                };
            }
        }
    }
}
