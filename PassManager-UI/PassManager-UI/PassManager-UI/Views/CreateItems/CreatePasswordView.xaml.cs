﻿using PassManager.ViewModels.CreateItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.CreateItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePasswordView : ContentPage
    {
        public CreatePasswordView()
        {
            InitializeComponent();
            BindingContext = new CreatePasswordVM();
        }
    }
}