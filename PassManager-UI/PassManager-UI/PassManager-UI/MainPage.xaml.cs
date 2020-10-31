using PassManager.Models;
using PassManager.Models.Api;
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
        //construnctor for the page
        public MainPage()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
            ActionStatus = true;
            CurrentAction = TypeOfActions.Sign_In;
            InternetStatusText = string.Empty;
            if (CheckInternet())
            {
                IsInternet = true;
                SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
            }
            else DisplayError(false,"Your don't have internet access!");
            BindingContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private TypeOfActions CurrentAction { get; set; }
        private string _pageTitle;
        private bool _isInternet;
        private string _internetStatusText;
        private string _errorMsg;
        private string _changePageText;
        private string _infoText;
        private bool _isError;
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
        //hides the visibility for the errors behind the frame
        public bool InternetErrorVis
        {
            get { return !_isInternet; }
            set { NotifyPropertyChanged("InternetErrorVis"); }
        }
        //enable/disable buttons for actions that are executing
        private bool _actionStatus;
        public bool ActionStatus
        {
            get { return _actionStatus; }
            set { _actionStatus = value; NotifyPropertyChanged("ActionStatus"); }
        }

        //implementation of INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void RefreshPage(object sender, EventArgs e)
        {
            if (CheckInternet())
            {
                DisplayError(true, string.Empty);
                if (PageLoaded()) SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
            }
            else
                DisplayError(false,"You don't have internet access!");
        }
        private bool CheckInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
        async private void Action(object sender, EventArgs e)
        {
            ActionStatus = false;
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
            ActionStatus = true;
        }
        private void ChangePage(object sender, EventArgs e)
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Sign_In:
                    CurrentAction = TypeOfActions.Register;
                    SetNames("Register", "Sign in", "Already have an account?");
                    Entry confirmPass = new Entry()
                    {
                        IsPassword = true,
                        Placeholder = "Confirm Password",
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(10, 5, 10, 5),
                        FontSize = 17
                    };
                    fields.Children.Add(confirmPass);
                    break;
                case TypeOfActions.Register:
                    CurrentAction = TypeOfActions.Sign_In;
                    SetNames("Sign in", "Register", "Create a new account!");
                    fields.Children.Remove(fields.Children.Last());
                    break;
                default:
                    break;
            }
        }
        private bool PageLoaded()
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
                if (String.IsNullOrWhiteSpace(emailField.Text) || String.IsNullOrWhiteSpace(passwordField.Text) || String.IsNullOrWhiteSpace(confirmPass.Text)) DisplayError("You need to complete all fields in order to register!");
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
                                }
                                else DisplayError(statusRegister.Message);
                            }
                            else DisplayError("Your confirm password is not equal with your password!");
                        }
                        else DisplayError(passwordStatus.Message);
                    }
                    else DisplayError(emailStatus.Message);
                }
            }
            else DisplayError(false, "Check for internet connection, then refresh the page!");
        }
        async private Task CreateNewAccount()
        {
            if (CheckInternet())
            {
                //check if fields are completed
                if (String.IsNullOrWhiteSpace(emailField.Text) || String.IsNullOrWhiteSpace(passwordField.Text)) DisplayError("You need to complete all fields in order to register!");
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
                        DisplayError(statusLogin.Message);
                }
            }
            else DisplayError(false, "Check for internet connection, then refresh the page!");
        }
        private void DisplayError(string errorMsg)
        {
            IsError = true;
            ErrorMsg = errorMsg;
        }
        private void DisplayError(bool internet, string internetMsg)
        {
            IsInternet = internet;
            InternetStatusText = internetMsg;
        }
    }
}