using System.Windows.Input;

namespace PassManager.Models
{
    public class SettingModel
    {
        public SettingModel(string text, ICommand command)
        {
            Text = text;
            WhenClicked = command;
        }
        public string Text { get; set; }
        public ICommand WhenClicked { get; }
    }
}
