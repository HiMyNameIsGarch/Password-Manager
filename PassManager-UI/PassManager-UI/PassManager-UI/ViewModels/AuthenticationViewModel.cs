using PassManager.CustomRenderer;
using PassManager.Enums;
using PassManager.Models.Interfaces;
using PassManager.Models.Api;
using PassManager.ViewModels;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.Views
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public AuthenticationViewModel(IPageService pageService) : base(pageService)
        {
            ApiHelper.InitializeClient();
            //set some default values
            ActionStatus = true;
            CurrentAction = TypeOfActions.Sign_In;
            IsRegisterPage = false;
            //set commands
            _changeVisCommand = new Command(ChangeVisOfPassField);
            _changePageCommand = new Command(ChangePage);
            _actionCommand = new Command(Action);
            //set some names for page
            SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
        }
        private TypeOfActions CurrentAction { get; set; }
        //private props
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
        private ICommand _changePageCommand;
        private ICommand _actionCommand;
        public ICommand ChangeVisCommand
        {
            get { return _changeVisCommand; }
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
            set { _username = value; NotifyPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); }
        }
        public string ConfirmPass
        {
            get { return _confirmPass; }
            set { _confirmPass = value; NotifyPropertyChanged(); }
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
            private set { _isError = value; NotifyPropertyChanged(); }
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
            //empty user fields and errors if exists
            Username = Password = ConfirmPass = string.Empty;
            DisplayError(string.Empty);
        }
        private void Action()
        {
            ActionStatus = false;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    Register();
                    break;
                case TypeOfActions.Sign_In:
                    SignIn();
                    break;
                default:
                    break;
            }
            ActionStatus = true;
        }
        //methods
        private void Register()
        {
            if (CheckInternet())
            {
            //    //check if all fields are completed
            //    if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(ConfirmPass)) DisplayError("You need to complete all fields in order to register!");
            //    else
            //    {
            //        //verify status of fields
            //        Models.TaskStatus emailStatus = FieldsHelper.VerifyEmail(Username);
            //        if (!emailStatus.IsError)
            //        {
            //            Models.TaskStatus passwordStatus = FieldsHelper.VerifyPassword(Password);
            //            if (!passwordStatus.IsError)
            //            {
            //                if (ConfirmPass == Password)
            //                {
            //                    Models.TaskStatus statusRegister = await UserProcessor.Register(ApiHelper.ApiClient, Username, Password, ConfirmPass);
            //                    if (!statusRegister.IsError)
            //                    {
            //                        Username = Password = ConfirmPass = string.Empty;
                                    _pageService.ChangeMainPage(new MainView());
            //                    }
            //                    else DisplayError(statusRegister.Message);
            //                }
            //                else DisplayError("Your confirm password is not equal with your password!");
            //            }
            //            else DisplayError(passwordStatus.Message);
            //        }
            //        else DisplayError(emailStatus.Message);
            //    }
            }
        }
        private void SignIn()
        {
            if (CheckInternet())
            {
                //    //check if fields are completed
                //    if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password)) DisplayError("You need to complete all fields in order to register!");
                //    else
                //    {
                //        Models.TaskStatus statusLogin = await UserProcessor.LogIn(ApiHelper.ApiClient, Username, Password);
                //        if (!statusLogin.IsError)
                //        {
                //            Username = Password = string.Empty;
                _pageService.ChangeMainPage(new MainView());
            //        }
            //        else
            //            DisplayError(statusLogin.Message);
            //    }
            }
        }
        private void SetNames(string title, string page, string question)
        {
            PageTitle = title;
            AnotherPageText = page;
            QuestionForUser = question;
        }
        //the method can be used to clear the errors as well
        private void DisplayError(string errorMsg)
        {
            IsError = true;
            ErrorMsg = errorMsg;
        }
    }
}
