﻿using PassManager.Enums;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;
using PassManager.ViewModels.Bases;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using PassManager.Models;
using PassManager.Views.Popups;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PassManager.Views
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public AuthenticationViewModel()
        {
            //set some default values
            LoadEmail().Await();
            IsConfirmPassVisible = true;
            IsPasswordVisible = true;
            PassEntryIcon = IconHelper.GetImageUrl("Locked");
            ConfirmPassEntryIcon = IconHelper.GetImageUrl("Locked");
            CurrentAction = TypeOfActions.Sign_In;
            IsRegisterPage = false;
            //set commands
            _changeVisOfConfirmPass = new Command(ChangeVisOfConfirmPassword);
            _changeVisOfPass = new Command(ChangeVisOfPassword);
            _changePageCommand = new Command(ChangePage);
            _actionCommand = new Command(Action);
            //set some names for page
            SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account", "Confirm");
        }
        //private props
        private const string defaultValueForEmail = " ";
        private const string emailKey = "user_email";
        private TypeOfActions CurrentAction;
        private string _anotherPageText;
        private string _questionForUser;
        private string _actionBtnText;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _confirmPass = string.Empty;
        private string _passEntryIcon;
        private string _confirmPassEntryIcon;
        //booleans
        private bool _rememberEmail = false;
        private bool _isRegisterPage;
        private bool _isPasswordVisible;
        private bool _isConfirmPassVisible;
        //commands
        private ICommand _changePageCommand;
        private ICommand _actionCommand;
        private ICommand _changeVisOfConfirmPass;
        private ICommand _changeVisOfPass;
        //commands prop
        public ICommand ChangeVisOfConfirmPass
        {
            get { return _changeVisOfConfirmPass; }
        }
        public ICommand ChangeVisOfPass
        {
            get { return _changeVisOfPass; }
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
        public string PassEntryIcon
        {
            get { return _passEntryIcon; }
            private set { _passEntryIcon = value; NotifyPropertyChanged(); }
        }
        public string ConfirmPassEntryIcon
        {
            get { return _confirmPassEntryIcon; }
            private set { _confirmPassEntryIcon = value; NotifyPropertyChanged(); }
        }
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            private set { _isPasswordVisible = value; NotifyPropertyChanged(); }
        }
        public bool IsConfirmPassVisible
        {
            get { return _isConfirmPassVisible; }
            private set { _isConfirmPassVisible = value; NotifyPropertyChanged(); }
        }
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
        public string ActionBtnText
        {
            get { return _actionBtnText; }
            private set { _actionBtnText = value; NotifyPropertyChanged(); }
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
        public bool RememberEmail
        {
            get { return _rememberEmail; }
            set { _rememberEmail = value; NotifyPropertyChanged(); }
        }
        public bool IsRegisterPage
        {
            get { return _isRegisterPage; }
            private set { _isRegisterPage = value; NotifyPropertyChanged(); }
        }
        //commands implementation
        private void ChangeVisOfConfirmPassword()
        {
            ConfirmPassEntryIcon = IconHelper.GetImageUrl(IsPasswordVisible ? "Open" : "Locked");
            IsConfirmPassVisible = !IsConfirmPassVisible;
        }
        private void ChangeVisOfPassword()
        {
            PassEntryIcon = IconHelper.GetImageUrl(IsPasswordVisible ? "Open" : "Locked");
            IsPasswordVisible = !IsPasswordVisible;
        }
        private void ChangePage()
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Sign_In:
                    CurrentAction = TypeOfActions.Register;
                    IsRegisterPage = true;
                    SetNames("Create a new account", "Sign in", "Already have an account?", TypeOfActions.Register.ToString());
                    break;
                case TypeOfActions.Register:
                    CurrentAction = TypeOfActions.Sign_In;
                    IsRegisterPage = false;
                    SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account", "Confirm");
                    break;
                default:
                    break;
            }
            //empty user fields and errors if exists
            Username = Password = ConfirmPass = string.Empty;
        }
        private async void Action()
        {
            bool isAuthSuccess = false;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    isAuthSuccess = await Register();
                    break;
                case TypeOfActions.Sign_In:
                    isAuthSuccess = await SignIn();
                    break;
            }
            if (isAuthSuccess)
            {
                if (RememberEmail)
                {
                    try
                    {
                        await SecureStorage.SetAsync(emailKey, Username);
                    }
                    catch
                    {
                        await PageService.DisplayAlert("Ooops", "Something went wrong and we couldn't save your email!","Ok","Cancel");
                    }
                }
                else
                {
                    await SecureStorage.SetAsync(emailKey, defaultValueForEmail);
                }
                Username = Password = ConfirmPass = string.Empty;
                PageService.ChangeMainPage(new MainView());
                PageService.ChangeNavBarColor(PageService.CeladonBlueColor);
            }
        }
        //methods
        private async Task LoadEmail()
        {
            string newEmail = string.Empty;
            try
            {
                newEmail = await SecureStorage.GetAsync(emailKey);
            }
            catch
            {
                await PageService.DisplayAlert("Ooops", "Something went wrong and we couldn't save your email!", "Ok", "Cancel");
            }
            if (string.IsNullOrEmpty(newEmail) || string.IsNullOrWhiteSpace(newEmail))
            {
                RememberEmail = false;
            }
            else
            {
                RememberEmail = true;
                Username = newEmail;
            }
        }
        private async Task<bool> Register()
        {
            if (IsInternet())
            {
                //check if all fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(ConfirmPass)) await DisplayError(ErrorMsg.CompleteAllFieldsError);
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
                                if (!Password.ToLower().Contains(Username.ToLower()))
                                {
                                    await PageService.PushPopupAsync(new WaitForActionView());
                                    string authPassword = VaultManager.CreateAuthPassword(Username, Password);
                                    Models.TaskStatus statusRegister = await UserProcessor.Register(ApiHelper.ApiClient, Username, authPassword, authPassword);
                                    if (!statusRegister.IsError)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        await PageService.PopPopupAsync();
                                        await DisplayError(statusRegister.Message);
                                    }
                                }
                                else await DisplayError("Your email can't be in your password!");
                            }
                            else await DisplayError("Your Confirm Password and your Password are not the same!");
                        }
                        else await DisplayError(passwordStatus.Message);
                    }
                    else await DisplayError(emailStatus.Message);
                }
            }
            return false;
        }
        private async Task<bool> SignIn()
        {
            if (IsInternet())
            {
                //check if fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password)) await DisplayError(ErrorMsg.CompleteAllFieldsError);
                else
                {
                    await PageService.PushPopupAsync(new WaitForActionView());
                    //create auth password
                    string authPassword = VaultManager.CreateAuthPassword(Username, Password);
                    Models.TaskStatus statusLogin = await UserProcessor.LogIn(ApiHelper.ApiClient, Username, authPassword);
                    if (!statusLogin.IsError)
                    {
                        return true;
                    }
                    else
                    {
                        await PageService.PopPopupAsync();
                        await DisplayError(statusLogin.Message);
                    }
                }
            }
            return false;
        }
        private void SetNames(string title, string pageText, string question, string actionText)
        {
            PageTitle = title;
            AnotherPageText = pageText;
            ActionBtnText = actionText;
            QuestionForUser = question;
        }
        private async Task DisplayError(string errorMsg)
        {
            await PageService.PushPopupAsync(new ErrorView(errorMsg));
        }
    }
}
