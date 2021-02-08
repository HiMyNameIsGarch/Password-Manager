using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PassManager.Models
{
    public static class ClipboardHelper
    {
        public static async Task CopyToClipboard(string textToCopy)
        {
            if (!string.IsNullOrEmpty(textToCopy))
            {
                string clipboardText = await Clipboard.GetTextAsync() ?? "";
                if (textToCopy != clipboardText)
                    await Clipboard.SetTextAsync(textToCopy);
            }
        }
    }
}
