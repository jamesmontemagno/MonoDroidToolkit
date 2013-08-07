using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using com.refractored.monodroidtoolkit;

namespace Samples
{
    [Activity(Label = "Progress Button", Theme = "@style/MyThemeDark")]
    public class ProgressButtonActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.progress_button);
            // Create your application here


            var progressButton1 = FindViewById<ProgressButton>(Resource.Id.pin_progress_1);
            var progressButton2 = FindViewById<ProgressButton>(Resource.Id.pin_progress_2);
            var progressButton3 = FindViewById<ProgressButton>(Resource.Id.pin_progress_3);
            var progressButton4 = FindViewById<ProgressButton>(Resource.Id.pin_progress_4);

            var container = FindViewById<LinearLayout>(Resource.Id.container);

            //Default Implemnetation, is not clickable, is not pinned
            var progressButton5 = AddProgressButton(container);

            //Starts pinned, and is not clickable, so it stays pinned
            var progressButton6 = AddProgressButton(container);
            progressButton6.Pinned = true;
            progressButton6.Indeterminante = true;

            //Starts pinned, is clickable so it's pinned state can be changed by user
            var progressButton7 = AddProgressButton(container);
            progressButton7.Pinned = true;
            progressButton7.Clickable = true;
            progressButton7.Focusable = true;


            //Example of how to use style the button in code
            var progressButton8 = AddProgressButton(container);
            progressButton8.ProgressColor = Resources.GetColor(Resource.Color.holo_green_light);
            progressButton8.CircleColor = Resources.GetColor(Resource.Color.holo_green_dark);
            progressButton8.Clickable = true;
            progressButton8.Focusable = true;

            progressButton1.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton) sender);
            progressButton2.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton3.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton4.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton5.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton6.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton7.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);
            progressButton8.CheckedChange +=
                (sender, args) => UpdatePinProgressContentDescription((ProgressButton)sender);


            var progressSeekBar = FindViewById<SeekBar>(Resource.Id.progress_seek_bar);
            progressSeekBar.ProgressChanged += (sender, args) =>
                {
                    UpdateProgressButton(progressButton1, progressSeekBar);
                    UpdateProgressButton(progressButton2, progressSeekBar);
                    UpdateProgressButton(progressButton3, progressSeekBar);
                    UpdateProgressButton(progressButton4, progressSeekBar);
                    UpdateProgressButton(progressButton5, progressSeekBar);
                    UpdateProgressButton(progressButton6, progressSeekBar);
                    UpdateProgressButton(progressButton7, progressSeekBar);
                    UpdateProgressButton(progressButton8, progressSeekBar);
                };

            UpdateProgressButton(progressButton1, progressSeekBar);
            UpdateProgressButton(progressButton2, progressSeekBar);
            UpdateProgressButton(progressButton3, progressSeekBar);
            UpdateProgressButton(progressButton4, progressSeekBar);
            UpdateProgressButton(progressButton5, progressSeekBar);
            UpdateProgressButton(progressButton6, progressSeekBar);
            UpdateProgressButton(progressButton7, progressSeekBar);
            UpdateProgressButton(progressButton8, progressSeekBar);

        }

        private void UpdateProgressButton(ProgressButton progressButton, SeekBar progressSeekBar)
        {
            if (progressButton.Indeterminante)
                return;

            progressButton.Progress = progressSeekBar.Progress;
            UpdatePinProgressContentDescription(progressButton);
        }

        private void UpdatePinProgressContentDescription(ProgressButton button)
        {
            if (button.Progress <= 0)
            {
                button.ContentDescription =
                    GetString(button.Pinned
                                  ? Resource.String.content_desc_pinned_not_downloaded
                                  : Resource.String.content_desc_unpinned_not_downloaded);
            }
            else if (button.Progress >= 100)
            {
                button.ContentDescription =
                    GetString(button.Pinned
                                  ? Resource.String.content_desc_pinned_downloaded
                                  : Resource.String.content_desc_unpinned_downloaded);
            }
            else
            {
                button.ContentDescription =
                    GetString(button.Pinned
                                  ? Resource.String.content_desc_pinned_downloading
                                  : Resource.String.content_desc_unpinned_downloading);
            }
        }

        private ProgressButton AddProgressButton(LinearLayout container)
        {
            var layoutParams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f);
            var progressButton = new ProgressButton(this);
            progressButton.LayoutParameters = layoutParams;
            container.AddView(progressButton);
            return progressButton;
        }
    }
}