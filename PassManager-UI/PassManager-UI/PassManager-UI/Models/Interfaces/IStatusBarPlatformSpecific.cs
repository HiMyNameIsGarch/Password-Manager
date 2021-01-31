using Android.Graphics;

namespace PassManager.Models.Interfaces
{
    public interface IStatusBarPlatformSpecific
    {
        void ChangeStatusBarColor(Color color);
        void ChangeNavigationBarColor(Color color);
        /// <summary>
        /// converts hex value to rgb value
        /// </summary>
        /// <param name="hexValue">the value needs to be 6 chars</param>
        /// <returns>it will return the rgb color from hex value, if the value is more or less than 6 it will return red color</returns>
        Color ColorFromHex(string hexValue);
    }
}
