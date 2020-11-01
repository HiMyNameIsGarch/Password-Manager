using PassManager.AccountPages;
using PassManager.CustomRenderer;
using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PassManager.Pages
{
    public class MainPage: INotifyPropertyChanged
    {
        public MainPage()
        {
            ApiHelper.InitializeClient();
            //set some default values
            ActionStatus = true;
            CurrentAction = TypeOfActions.Sign_In;
            IsRegisterPage = false;
            InternetStatusText = string.Empty;
            //set commands
            _changeVisCommand = new Command(ChangeVisOfPassField);
            _refreshPageCommand = new Command(RefreshPage);
            _changePageCommand = new Command(ChangePage);
            _actionCommand = new Command(Action);
            //check if can put data on page
            if (CheckInternet())
            {
                IsInternet = true;
                SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
            }
            else DisplayError(false, "Your don't have internet access!");
        }
        private TypeOfActions CurrentAction { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        //private props
        private string _pageTitle;
        private bool _isInternet;
        private string _internetStatusText;
        private string _errorMsg;
        private string _anotherPageText;
        private string _questionForUser;
        private bool _isError;
        private bool _actionStatus;
        private bool _isRegisterPage;
        private string _username;
        private string _password;
        private string _confirmPass;
        //commands
        private ICommand _changeVisCommand;
        private ICommand _refreshPageCommand;
        private ICommand _changePageCommand;
        private ICommand _actionCommand;
        public ICommand ChangeVisCommand
        {
            get { return _changeVisCommand; }
        }
        public ICommand RefreshPageCommand
        {
            get { return _refreshPageCommand; }
        }
        public ICommand ChangePageCommand
        {
            get { return _changePageCommand; }
        }
        public ICommand ActionCommand
        {
            get { return _actionCommand; }
        }
        //prop for the view
        public string Username
        {
            get { return _username; }
            private set { _username = value; NotifyPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            private set { _password = value; NotifyPropertyChanged(); }
        }
        public string ConfirmPass
        {
            get { return _confirmPass; }
            private set { _confirmPass = value; NotifyPropertyChanged(); }
        }
        public string PageTitle
        {
            get { return _pageTitle; }
            private set { _pageTitle = value; NotifyPropertyChanged(); }
        }
        public bool IsInternet
        {
            get { return _isInternet; }
            private set { _isInternet = value; NotifyPropertyChanged(); }
        }
        public string InternetStatusText
        {
            get { return _internetStatusText; }
            private set { _internetStatusText = value; NotifyPropertyChanged(); }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            private set { _errorMsg = value; NotifyPropertyChanged(); }
        }
        public string AnotherPageText
        {
            get { return _anotherPageText; }
            private set { _anotherPageText = value; NotifyPropertyChanged(); }
        }
        public string QuestionForUser
        {
            get { return _questionForUser; }
            private set { _questionForUser = value; NotifyPropertyChanged(); }
        }
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; NotifyPropertyChanged(); }
        }
        public bool ActionStatus
        {
            get { return _actionStatus; }
            private set { _actionStatus = value; NotifyPropertyChanged(); }
        }
        public bool IsRegisterPage
        {
            get { return _isRegisterPage; }
            private set { _isRegisterPage = value; NotifyPropertyChanged(); }
        }
        //commands implementation
        private void ChangeVisOfPassField(object obj)
        {
            var mainStack = obj as StackLayout;
            CustomeEntry entry = mainStack.Children.FirstOrDefault() as CustomeEntry;
            bool statusPassVis = false;
            if (entry != null)
            {
                statusPassVis = entry.IsPassword;
                entry.IsPassword = statusPassVis == true ? false : true;
            }
            Frame frame = mainStack.Children.Where(s => s.GetType().Name == "Frame").FirstOrDefault() as Frame;
            if (frame != null)
            {
                Image image = frame.Content as Image;
                if (image != null) image.Source = ImageSource.FromResource($"PassManager-UI.Images.{(statusPassVis ? "Open" : "Locked")}.png");
            }
        }
        private void RefreshPage()
        {
            if (CheckInternet())
            {
                DisplayError(true, string.Empty);
                if (IsPageLoaded()) SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
            }
            else
                DisplayError(false, "You don't have internet access!");
        }
        private void ChangePage()
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Sign_In:
                    CurrentAction = TypeOfActions.Register;
                    IsRegisterPage = true;
                    SetNames("Register", "Sign in", "Already have an account?");
                    break;
                case TypeOfActions.Register:
                    CurrentAction = TypeOfActions.Sign_In;
                    IsRegisterPage = false;
                    SetNames("Sign in", "Register", "Create a new account!");
                    break;
                default:
                    break;
            }
            Username = Password = ConfirmPass = string.Empty;
        }
        private async void Action()
        {
            ActionStatus = false;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    await Register();
                    break;
                case TypeOfActions.Sign_In:
                    await SignIn();
                    break;
                default:
                    break;
            }
            ActionStatus = true;
        }
        //methods
        private async Task Register()
        {
            if (CheckInternet())
            {
                //check if all fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(ConfirmPass)) DisplayError("You need to complete all fields in order to register!");
                else
                {
                    //verify status of fields
                    Models.TaskStatus emailStatus = FieldsHelper.VerifyEmail(Username);
                    if (!emailStatus.IsError)
                    {
                        Models.TaskStatus passwordStatus = FieldsHelper.VerifyPassword(Password);
                        if (!passwordStatus.IsError)
                        {
                            if (ConfirmPass == Password)
                            {
                                Models.TaskStatus statusRegister = await UserProcessor.Register(ApiHelper.ApiClient, Username, Password, ConfirmPass);
                                if (!statusRegister.IsError)
                                {
                                    Username = Password = ConfirmPass = string.Empty;
                                    //await Navigation.PushModalAsync(new ListAccounts(), true);
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
        private async Task SignIn()
        {
            if (CheckInternet())
            {
                //check if fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password)) DisplayError("You need to complete all fields in order to register!");
                else
                {
                    Models.TaskStatus statusLogin = await UserProcessor.Login(ApiHelper.ApiClient, Username, Password);
                    if (!statusLogin.IsError)
                    {
                        Username = Password = string.Empty;
                        //await Navigation.PushModalAsync(new ListAccounts(), true);
                    }
                    else
                        DisplayError(statusLogin.Message);
                }
            }
            else DisplayError(false, "Check for internet connection, then refresh the page!");
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private bool CheckInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
        private bool IsPageLoaded()
        {
            return (PageTitle is null || AnotherPageText is null || QuestionForUser is null);
        }
        private void SetNames(string title, string page, string question)
        {
            PageTitle = title;
            AnotherPageText = page;
            QuestionForUser = question;
        }
        private void DisplayError(bool internet, string internetMsg)
        {
            IsInternet = internet;
            InternetStatusText = internetMsg;
        }
        private void DisplayError(string errorMsg)
        {
            IsError = true;
            ErrorMsg = errorMsg;
        }
    }
}
