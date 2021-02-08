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
        public static ImageSource GetImageSource(string imageName)
        {
            return ImageSource.FromResource($"PassManager-UI.Images.{imageName}.png");
        }
    }
}
