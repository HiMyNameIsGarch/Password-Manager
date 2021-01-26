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

namespace PassManager.Views
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public AuthenticationViewModel()
        {
            //set some default values
            IsConfirmPassVisible = true;
            IsPasswordVisible = true;
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.Locked.png");
            ConfirmPassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.Locked.png");
            CurrentAction = TypeOfActions.Sign_In;
            IsRegisterPage = false;
            //set commands
            _changeVisOfConfirmPass = new Command(ChangeVisOfConfirmPassword);
            _changeVisOfPass = new Command(ChangeVisOfPassword);
            _changePageCommand = new Command(ChangePage);
            _actionCommand = new Command(Action);
            //set some names for page
            SetNames("Sign in", TypeOfActions.Register.ToString(), "Create a new account!");
        }
        private TypeOfActions CurrentAction { get; set; }
        //private props
        private string _anotherPageText;
        private string _questionForUser;
        private string _username;
        private string _password;
        private string _confirmPass;
        private ImageSource _passEntryIcon;
        private ImageSource _confirmPassEntryIcon;
        //booleans
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
        public ImageSource PassEntryIcon
        {
            get { return _passEntryIcon; }
            private set { _passEntryIcon = value; NotifyPropertyChanged(); }
        }
        public ImageSource ConfirmPassEntryIcon
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
        public bool IsRegisterPage
        {
            get { return _isRegisterPage; }
            private set { _isRegisterPage = value; NotifyPropertyChanged(); }
        }
        //commands implementation
        private void ChangeVisOfConfirmPassword()
        {
            ConfirmPassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsConfirmPassVisible ? "Open" : "Locked")}.png");
            IsConfirmPassVisible = !IsConfirmPassVisible;
        }
        private void ChangeVisOfPassword()
        {
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsPasswordVisible ? "Open" : "Locked")}.png");
            IsPasswordVisible = !IsPasswordVisible;
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
        }
        private void Action()
        {
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    Register();
                    break;
                case TypeOfActions.Sign_In:
                    SignIn();
                    break;
            }
        }
        //methods
        private async void Register()
        {
            if (IsInternet())
            {
                //check if all fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(ConfirmPass)) await DisplayError("You need to complete all fields in order to register!");
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
                                if (!Password.Contains(Username))
                                {
                                    await PageService.PushPopupAsync(new WaitForActionView());
                                    string authPassword = VaultManager.CreateAuthPassword(Username, Password);
                                    Models.TaskStatus statusRegister = await UserProcessor.Register(ApiHelper.ApiClient, Username, authPassword, authPassword);
                                    if (!statusRegister.IsError)
                                    {
                                        Username = Password = ConfirmPass = string.Empty;
                                        PageService.ChangeMainPage(new MainView());
                                    }
                                    else
                                    {
                                        await PageService.PopPopupAsync();
                                        await DisplayError(statusRegister.Message);
                                    }
                                }
                                else await DisplayError("Your email can't be in your password!");
                            }
                            else await DisplayError("Your confirm password is not equal with your password!");
                        }
                        else await DisplayError(passwordStatus.Message);
                    }
                    else await DisplayError(emailStatus.Message);
                }
            }
        }
        private async void SignIn()
        {
            if (IsInternet())
            {
                //check if fields are completed
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password)) await DisplayError("You need to complete all fields in order to register!");
                else
                {
                    await PageService.PushPopupAsync(new WaitForActionView());
                    //create auth password
                    string authPassword = VaultManager.CreateAuthPassword(Username, Password);
                    Models.TaskStatus statusLogin = await UserProcessor.LogIn(ApiHelper.ApiClient, Username, authPassword);
                    if (!statusLogin.IsError)
                    {
                        Username = Password = string.Empty;
                        PageService.ChangeMainPage(new MainView());
                    }
                    else
                    {
                        await PageService.PopPopupAsync();
                        await DisplayError(statusLogin.Message);
                    }
                }
            }
        }
        private void SetNames(string title, string page, string question)
        {
            PageTitle = title;
            AnotherPageText = page;
            QuestionForUser = question;
        }
        //the method can be used to clear the errors as well
        private async Task DisplayError(string errorMsg)
        {
            await PageService.PushPopupAsync(new ErrorView(errorMsg));
        }
    }
}
