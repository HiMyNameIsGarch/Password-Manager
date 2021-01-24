using PassManager.Models;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.ViewModels.Bases;

namespace PassManager.ViewModels.Popups
{
    public class PasswordGeneratorVM : BaseViewModel
    {
        public PasswordGeneratorVM() : base("Password Generator")
        {
            _copyPassword = new Command(CopyToClipboard);
            //set default values for the password
            _passwordLength = 9;
            LengthText = "Your password length is " + _passwordLength;
            _includeSpecialChars = false;
            _includeUpperCaseLetters = false;
            _includeNumbers = false;
            //generate and display it
            Password = PasswordGenerator.GeneratePassword(_passwordLength, _includeUpperCaseLetters, _includeNumbers, _includeSpecialChars);
        }
        //private fields
        private string _lengthText;
        private bool _includeNumbers;
        private bool _includeUpperCaseLetters;
        private bool _includeSpecialChars;
        private int _passwordLength;
        private string _password;
        private ICommand _copyPassword;
        //props
        public bool IncludeUpperCaseLetters {
            get
            {
                return _includeUpperCaseLetters;
            }
            set
            {
                _includeUpperCaseLetters = value;
                Password = PasswordGenerator.GeneratePassword(_passwordLength, _includeUpperCaseLetters, _includeNumbers, _includeSpecialChars);
                NotifyPropertyChanged();
            }
        }
        public bool IncludeNumbers {
            get
            {
                return _includeNumbers;
            }
            set
            {
                _includeNumbers = value;
                Password = PasswordGenerator.GeneratePassword(_passwordLength, _includeUpperCaseLetters, _includeNumbers, _includeSpecialChars);
                NotifyPropertyChanged();
            } 
        }
        public bool IncludeSpecialChars { 
            get
            {
                return _includeSpecialChars;
            }
            set
            {
                _includeSpecialChars = value;
                Password = PasswordGenerator.GeneratePassword(_passwordLength, _includeUpperCaseLetters, _includeNumbers, _includeSpecialChars);
                NotifyPropertyChanged();
            }
        }
        public string LengthText
        {
            get { return _lengthText; }
            private set { _lengthText = value; NotifyPropertyChanged(); }
        }
        public int PasswordLength
        {
            get { return _passwordLength; }
            set 
            {
                _passwordLength = value;
                LengthText = "Your password length is " + _passwordLength;
                Password = PasswordGenerator.GeneratePassword(_passwordLength, _includeUpperCaseLetters, _includeNumbers, _includeSpecialChars);
                NotifyPropertyChanged(); 
            }
        }
        public string Password
        {
            get { return _password; }
            private set { _password = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand CopyPassword
        {
            get { return _copyPassword; }
        }
        public ICommand Close
        {
            get { 
                return new Command(async () => 
                {
                    await PageService.PopPopupAsync(true);
                }); 
            }
        }
        //functions
        private async void CopyToClipboard()
        {
            string clipboardText = await Clipboard.GetTextAsync() ?? "";
            if (Password != clipboardText)
            {
                await Clipboard.SetTextAsync(Password);
            }
        }
    }
}
