using PassManager_WebApi.Enums;

namespace PassManager_WebApi.Models
{
    public static class IconHelper
    {
        public static string GetImageUrl(TypeOfItems itemType)
        {
            return $"PassManager-UI.Images.{itemType}.png";
        }
    }
}