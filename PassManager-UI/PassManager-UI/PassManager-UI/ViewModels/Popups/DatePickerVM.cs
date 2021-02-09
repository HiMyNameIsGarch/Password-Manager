using PassManager.Enums;
using PassManager.ViewModels.Bases;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.Popups
{
    public class DatePickerVM : BaseViewModel
    {
        public DatePickerVM(DateTime date) : base(date.ToString(DateFormat))
        {
            //set props for page
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
            _initialDate = date;
            //set commands
            _cancel = new Command(CancelCommand);
            _confirm = new Command(ConfirmCommand);
            _changeYear = new Command(ChangeYearCommand);
            _changeMonth = new Command(ChangeMonthCommand);
            _changeDay = new Command(ChangeDayCommand);
        }
        //variables
        private const string DateFormat = "dd MMMM yyyy";
        private readonly DateTime _initialDate;
        private ICommand _cancel;
        private ICommand _confirm;
        private ICommand _changeYear;
        private ICommand _changeMonth;
        private ICommand _changeDay;
        private int _day;
        private int _month;
        private int _year;
        public DateTime ReturnValue;
        //commands
        public ICommand ChangeYear
        {
            get { return _changeYear; }
        }
        public ICommand ChangeMonth
        {
            get { return _changeMonth; }
        }
        public ICommand ChangeDay
        {
            get { return _changeDay; }
        }
        public ICommand Cancel
        {
            get { return _cancel; }
        }
        public ICommand Confirm
        {
            get { return _confirm; }
        }
        //props
        public int Day
        {
            get { return _day; }
            private set { _day = value; NotifyPropertyChanged(); }
        }
        public int Month
        {
            get { return _month; }
            private set { _month = value; NotifyPropertyChanged(); }
        }
        public int Year
        {
            get { return _year; }
            private set { _year = value; NotifyPropertyChanged(); }
        }
        //implementation for commands
        private void ChangeYearCommand(object obj)
        {
            if (obj is null) return;
            var result = Enum.TryParse<ChangeDateMode>(obj.ToString(), out ChangeDateMode mode);
            if (result)
            {
                var date = GetCurrentDate();
                switch (mode)
                {
                    case ChangeDateMode.Increase:
                        date = date.AddYears(1);
                        break;
                    case ChangeDateMode.Decrease:
                        date = date.AddYears(-1);
                        break;
                }
                ChangeDate(date);
            }   
        }
        private void ChangeMonthCommand(object obj)
        {
            if (obj is null) return;
            var result = Enum.TryParse<ChangeDateMode>(obj.ToString(), out ChangeDateMode mode);
            if (result)
            {
                var date = GetCurrentDate();
                switch (mode)
                {
                    case ChangeDateMode.Increase:
                        date = date.AddMonths(1);
                        break;
                    case ChangeDateMode.Decrease:
                        date = date.AddMonths(-1);
                        break;
                }
                ChangeDate(date);
            }
        }
        private void ChangeDayCommand(object obj)
        {
            if (obj is null) return;
            var result = Enum.TryParse<ChangeDateMode>(obj.ToString(), out ChangeDateMode mode);
            if (result)
            {
                var date = GetCurrentDate();
                switch (mode)
                {
                    case ChangeDateMode.Increase:
                        date = date.AddDays(1);
                        break;
                    case ChangeDateMode.Decrease:
                        date = date.AddDays(-1);
                        break;
                }
                ChangeDate(date);
            }
        }
        private async void CancelCommand()
        {
            await ClosePopUp(_initialDate);
        }
        private async void ConfirmCommand()
        {
            await ClosePopUp(GetCurrentDate());
        }
        //functions
        private void ChangeDate(DateTime dateTime)
        {
            PageTitle = dateTime.ToString(DateFormat);
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
        }
        private DateTime GetCurrentDate()
        {
            return new DateTime(Year, Month, Day);
        }
        private async Task ClosePopUp(DateTime value)
        {
            ReturnValue = value;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
