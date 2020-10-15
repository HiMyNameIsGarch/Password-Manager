using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public TypeOfActions CurrentAction { get; set; }
        public string PageTitle { get; set; }

        public string ChangePageText { get; set; }

        public string InfoText { get; set; }
        public MainPage(TypeOfActions action)
        {
            InitializeComponent();
            CurrentAction = action;
            switch (CurrentAction)
            {
                case TypeOfActions.Register:
                    SetNames(CurrentAction.ToString(), "Sign in", "Already have an account?");
                    ActionBtn.Clicked -= CreateNewAccount;
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
            BindingContext = this;
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
        private void SetNames(string title, string page, string infoText)
        {
            PageTitle = title;
            ChangePageText = page;
            InfoText = infoText;
        }
        private void Register(object sender, EventArgs e)
        {

        }
        private void CreateNewAccount(object sender, EventArgs e)
        {

        }
    }
}