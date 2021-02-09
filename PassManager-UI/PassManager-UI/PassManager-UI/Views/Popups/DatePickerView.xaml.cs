using PassManager.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerView : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<DateTime> _taskCompletionSource;
        public Task<DateTime> PopupClosedTask => _taskCompletionSource.Task;
        public DatePickerView(DateTime dateTime)
        {
            InitializeComponent();
            BindingContext = new DatePickerVM(dateTime);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _taskCompletionSource = new TaskCompletionSource<DateTime>();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(((DatePickerVM)BindingContext).ReturnValue);
        }
    }
}