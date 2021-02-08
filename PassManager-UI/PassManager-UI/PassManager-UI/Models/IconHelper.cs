using PassManager.Enums;
using Xamarin.Forms;

namespace PassManager.Models
{
    public class IconHelper
    {
        public static string GetImageUrl(TypeOfItems itemType)
        {
            return $"PassManager-UI.Images.{itemType}.png";
        }
        public static string GetImageUrl(string imageName)
        {
            return $"PassManager-UI.Images.{imageName}.png";
        }
    }
}
