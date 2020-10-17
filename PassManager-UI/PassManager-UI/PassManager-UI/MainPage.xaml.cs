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
        public TypeOfActions CurrentAction { get;}
        private string _pageTitle;
        private bool _isInternet;
        private string _internetStatusText;
        private string _errorMsg;
        private string _changePageText;
        private string _infoText;
        private bool _isError;
        private bool _errorNeedBtn;
        //this is the page title, also the action button text
        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; NotifyPropertyChanged("PageTitle"); }
        } 
        //hides the visibility of the frame if not internet
        public bool IsInternet
        {
            get { return _isInternet; }
            set { _isInternet = value; NotifyPropertyChanged("IsInternet"); }
        }
        //display and msg about internet status(if needed)
        public string InternetStatusText
        {
            get { return _internetStatusText; }
            set { _internetStatusText = value; NotifyPropertyChanged("InternetStatusText"); }
        }
        //display the error in the frame excluded the internet ones
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; NotifyPropertyChanged("ErrorMsg"); }
        }
        //display the text for the button that changes the pages
        public string ChangePageText
        {
            get { return _changePageText; }
            set { _changePageText = value; NotifyPropertyChanged("ChangePageText"); }
        }
        //display a little information about your current status
        public string InfoText
        {
            get { return _infoText; }
            set { _infoText = value; NotifyPropertyChanged("InfoText"); }
        }
        //hides the visibility for the errors in the frame
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; NotifyPropertyChanged("IsError"); }
        }
        //change the visibility for the error button in the frame
        public bool ErrorNeedBtn
        {
            get { return _errorNeedBtn; }
            set { _errorNeedBtn = value; NotifyPropertyChanged("ErrorNeedBtn"); }
        }
        //implementation of INotifyPropertyChanged
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //construnctor for the page
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
            if (!CheckIfPageLoaded()) return;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    SetNames(CurrentAction.ToString(), "Sign in", "Already have an account?");
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
        private bool CheckIfPageLoaded()
        {
            return (PageTitle is null || ChangePageText is null || InfoText is null);
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
                DisplayError("Our server is down, please refresh the page!", true);
            }
            else
            {
                DisplayError(false, "Check for internet connection, then refresh the page!");
            }
        }
        private void CreateNewAccount(object sender, EventArgs e)
        {
            if (CheckInternet())
            {
                DisplayError("Our server is down, please refresh the page!", true);
            }
            else
            {
                DisplayError(false, "Check for internet connection, then refresh the page!");
            }
        }
        private void DisplayError(string errorMsg, bool needBtn)
        {
            if (needBtn)
            {
                Title.VerticalOptions = LayoutOptions.Center;
                if (CurrentAction == TypeOfActions.Sign_In) Errors.VerticalOptions = LayoutOptions.End;
                else Errors.VerticalOptions = LayoutOptions.Start;
            }
            IsError = true;
            ErrorNeedBtn = needBtn;
            ErrorMsg = errorMsg;
        }
        private void DisplayError(bool internet, string internetMsg)
        {
            IsInternet = internet;
            InternetStatusText = internetMsg;
        }
    }
}