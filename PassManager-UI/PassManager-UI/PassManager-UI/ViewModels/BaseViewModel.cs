﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel(){ }
        public BaseViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }
        //variables
        public event PropertyChangedEventHandler PropertyChanged;
        private string _pageTitle;
        protected private IPageService _pageService;
        //commands
        //props
        public string PageTitle
        {
            get { return _pageTitle; }
            protected private set { _pageTitle = value; NotifyPropertyChanged(); }
        }
        //methods
        protected private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected private bool CheckInternet()
        {
            bool internet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if(!internet)
            {
                _pageService.PushPopupAsync(new Views.InternetErrorView());
            }
            return internet;
        }
    }
}