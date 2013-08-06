using Android.App;
using Android.OS;
using Android.Widget;

namespace Samples
{
    [Activity(Label = "Samples", MainLauncher = true)]
    public class SampleHomeActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.progress_button).Click +=
                (sender, args) => StartActivity(typeof (ProgressButtonActivity));
        }
    }
}