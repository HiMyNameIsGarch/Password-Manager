using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace PassManager_UI.Droid
{
    [Activity(Theme = "@style/SplashTheme.Splash", MainLauncher = true, NoHistory = true, Label = "ShaMan")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupTask = new Task(() => { SimulateStartup(); });
            startupTask.Start();
        }
        async void SimulateStartup()
        {
            await Task.Delay(1500);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}