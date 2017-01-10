using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoDroidToolkit;

namespace Samples
{
  [Activity(Label = "Circular Progress Bar")]
  public class CircularProgressBarActivity : Activity
  {
    private HoloCircularProgressBar m_Progress;
    protected override void OnCreate(Bundle bundle)
    {
      if (Intent != null && Intent.Extras != null)
      {
        var theme = Intent.GetIntExtra("theme", 0);
        SetTheme(theme != 0 ? theme : Resource.Style.MyTheme);
      }
      else
      {
        SetTheme(Resource.Style.MyTheme);
      }

      base.OnCreate(bundle);
      SetContentView(Resource.Layout.circular_progress_bar);
      // Create your application here

      FindViewById<Button>(Resource.Id.switch_theme).Click += (sender, args) => SwitchTheme();

      m_Progress = FindViewById<HoloCircularProgressBar>(Resource.Id.holoCircularProgressBar1);
      m_Progress.Indeterminate = true;
    }






    public void SwitchTheme()
    {

      Intent intent = Intent;
      Bundle extras = Intent.Extras;
      if (extras != null)
      {
        int theme = extras.GetInt("theme", -1);
        if (theme == Resource.Style.MyTheme)
        {
          Intent.RemoveExtra("theme");
        }
        else
        {
          intent.PutExtra("theme", Resource.Style.MyTheme);
        }
      }
      else
      {
        intent.PutExtra("theme", Resource.Style.MyThemeDark);
      }
      Finish();
      StartActivity(intent);
    }
  }
}