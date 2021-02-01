using Android.Graphics;

namespace PassManager.Models.Interfaces
{
    public interface IStatusBarPlatformSpecific
    {
        void ChangeStatusBarColor(Color color);
        void ChangeNavigationBarColor(Color color);
    }
}
