using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TypeOfActions CurrentAction { get; set; }
        private string _pageTitle;
        private bool _isInternet;
        private string _internetStatusText;
        private string _errorMsg;
        private string _changePageText;
        private string _infoText;
        private bool _isError;
        private bool _errorNeedBtn;
        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; NotifyPropertyChanged("PageTitle"); }
        }
        public bool IsInternet
        {
            get { return _isInternet; }
            set { _isInternet = value; NotifyPropertyChanged("IsInternet"); }
        }
        public string InternetStatusText
        {
            get { return _internetStatusText; }
            set { _internetStatusText = value; NotifyPropertyChanged("InternetStatusText"); }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; NotifyPropertyChanged("ErrorMsg"); }
        }
        public string ChangePageText
        {
            get { return _changePageText; }
            set { _changePageText = value; NotifyPropertyChanged("ChangePageText"); }
        }
        public string InfoText
        {
            get { return _infoText; }
            set { _infoText = value; NotifyPropertyChanged("InfoText"); }
        }
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; NotifyPropertyChanged("IsError"); }
        }
        public bool ErrorNeedBtn
        {
            get { return _errorNeedBtn; }
            set { _errorNeedBtn = value; NotifyPropertyChanged("ErrorNeedBtn"); }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public MainPage(TypeOfActions action)
        {
            InitializeComponent();
            CurrentAction = action;
            InternetStatusText = string.Empty;
            if (CheckInternet())
            {
                LoadPage();
            }
            else
            {
                IsInternet = false;
                InternetStatusText = "You don't have internet!";
            }
            BindingContext = this;
        }

        private void RefreshPage(object sender, EventArgs e)
        {
            if (CheckInternet())
                LoadPage();
            else
                InternetStatusText = "You still don't have internet!";
        }
        private bool CheckInternet()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet) return true;
            else return false;
        }
        private void LoadPage()
        {
            IsError = false;
            IsInternet = true;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    SetNames(CurrentAction.ToString(), "Sign in", "Already have an account?");
                    ActionBtn.Clicked -= CreateNewAccount;
                    ActionBtn.Clicked += Register;
                    Entry confirmPass = new Entry()
                    {
                        IsPassword = true,
                        Placeholder = "Confirm Password",
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = 10,
                        FontSize = 16
                    };
                    fields.Children.Add(confirmPass);
                    break;
                case TypeOfActions.Sign_In:
                    SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
                    ActionBtn.Clicked += CreateNewAccount;
                    break;
                default:
                    break;
            }
        }
        async private void ChangePage(object sender, EventArgs e)
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    await Navigation.PopModalAsync(true);
                    break;
                case TypeOfActions.Sign_In:
                    await Navigation.PushModalAsync(new MainPage(TypeOfActions.Register), true);
                    break;
                default:
                    break;
            }
        }
        private void SetNames(string title, string page, string infoText)
        {
            PageTitle = title;
            ChangePageText = page;
            InfoText = infoText;
        }
        private void Register(object sender, EventArgs e)
        {
            if (CheckInternet())
            {

            }
            else
            {
                DisplayError(false, "Check for internet connection, then refresh the page!", true, true);
            }
        }
        private void CreateNewAccount(object sender, EventArgs e)
        {
            if (CheckInternet())
            {

            }
            else
            {
                DisplayError(false, "Check for internet connection, then refresh the page!", true, true);
            }
        }
        private void DisplayError(bool internet, string errorMsg, bool error, bool btnError)
        {
            IsInternet = internet;
            InternetStatusText = errorMsg;
            IsError = error;
            ErrorNeedBtn = btnError;
        }
    }
}