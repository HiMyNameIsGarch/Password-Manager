using PassManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PassManager.Enums;
using PassManager.AccountPages;

namespace PassManager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage(TypeOfActions action)
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
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
        //private bool CheckApi()
        //{
        //    PingReply apiResponse = new Ping().Send(ApiHelper.SERVER);
        //    if (apiResponse.Status == IPStatus.Success) return true;
        //    else return false;
        //}
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
                    break;
                default:
                    break;
            }
        }
        async private void Action(object sender, EventArgs e)
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    await Register();
                    break;
                case TypeOfActions.Sign_In:
                    await CreateNewAccount();
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
        async private Task Register()
        {
            if (CheckInternet())
            {
                //take the last field(confirm password)
                Entry confirmPass = fields.Children.Last() as Entry;
                //check if all fields are completed
                if (String.IsNullOrWhiteSpace(emailField.Text) || String.IsNullOrWhiteSpace(passwordField.Text) || String.IsNullOrWhiteSpace(confirmPass.Text)) DisplayError("You need to complete all fields in order to register!", false);
                else
                {
                    //verify status of fields
                    Models.TaskStatus emailStatus = FieldsHelper.VerifyEmail(emailField.Text);
                    if (!emailStatus.IsError)
                    {
                        Models.TaskStatus passwordStatus = FieldsHelper.VerifyPassword(passwordField.Text);
                        if (!passwordStatus.IsError)
                        {
                            if (confirmPass.Text == passwordField.Text)
                            {
                                Models.TaskStatus statusRegister = await UserProcessor.Register(ApiHelper.ApiClient, emailField.Text, passwordField.Text, confirmPass.Text);
                                if (!statusRegister.IsError)
                                {
                                    emailField.Text = null;
                                    passwordField.Text = null;
                                    confirmPass.Text = null;
                                    await Navigation.PushModalAsync(new ListAccounts(), true);
                                    //await Navigation.PopModalAsync(true);
                                }
                                else DisplayError(statusRegister.Message, false);
                            }
                            else DisplayError("Your confirm password is not equal with your password!",false);
                        }
                        else DisplayError(passwordStatus.Message, false);
                    }
                    else DisplayError(emailStatus.Message, false);
                }
            }
            else DisplayError(false, "Check for internet connection, then refresh the page!");
        }
        async private Task CreateNewAccount()
        {
            if (CheckInternet())
            {
                //check if fields are completed
                if (String.IsNullOrWhiteSpace(emailField.Text) || String.IsNullOrWhiteSpace(passwordField.Text)) DisplayError("You need to complete all fields in order to register!", false);
                else
                {
                    Models.TaskStatus statusLogin = await UserProcessor.Login(ApiHelper.ApiClient, emailField.Text, passwordField.Text);
                    if (!statusLogin.IsError)
                    {
                        emailField.Text = null;
                        passwordField.Text = null;
                        await Navigation.PushModalAsync(new ListAccounts(), true);
                    }
                    else
                        DisplayError(statusLogin.Message, false);
                }
            }
            else DisplayError(false, "Check for internet connection, then refresh the page!");
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