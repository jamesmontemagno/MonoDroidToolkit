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

            FindViewById<Button>(Resource.Id.circular_progress_bar).Click +=
              (sender, args) => StartActivity(typeof(CircularProgressBarActivity));

            FindViewById<Button>(Resource.Id.scale_image).Click +=
              (sender, args) => StartActivity(typeof(ScaleImageActivity));

            FindViewById<Button>(Resource.Id.image_loader).Click +=
                (sender, args) => StartActivity(typeof(ImageLoaderActivity));

            FindViewById<Button>(Resource.Id.hide_ui).Click +=
                  (sender, args) => StartActivity(typeof(SystemUiHiderActivity));

            FindViewById<Button>(Resource.Id.network).Click +=
                    (sender, args) => StartActivity(typeof(NetworkActivity));
        }
    }
}