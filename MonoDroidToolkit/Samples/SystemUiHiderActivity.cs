using Android.App;
using Android.Views;
using Android.OS;
using com.refractored.monodroidtoolkit.SystemUiHider;

namespace Samples
{

  [Activity (Label = "HideUI")]
    public class SystemUiHiderActivity : Activity, SystemUiHider.IOnVisibilityChangeListener, View.IOnClickListener, View.IOnTouchListener
    {
        private static readonly bool AUTO_HIDE = true;
        private static int AUTO_HIDE_DELAY_MILLIS = 3000;
        private static readonly bool TOGGLE_ON_CLICK = true;
        private static readonly int HIDER_FLAGS = SystemUiHider.FLAG_HIDE_NAVIGATION;
        private SystemUiHider m_SystemUiHider;
        Handler m_Handler = new Handler();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            View contentView = FindViewById(Resource.Id.main_layout);

            if (contentView == null)
                return;


            m_SystemUiHider = SystemUiHider.GetInstance(this, contentView, HIDER_FLAGS);
            m_SystemUiHider.Setup();
            m_SystemUiHider.SetOnVisibilityChangeListener(this);
            // Create your application here
        }

         protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            DelayedHide(100);
        }

        
        private void HideRunnable()
        {
            m_SystemUiHider.Hide();
        }

        private void DelayedHide(int delayMillis)
        {
            m_Handler.RemoveCallbacks(HideRunnable);
            m_Handler.PostDelayed(HideRunnable, delayMillis);
        }

        public void OnVisibilityChange(bool visible)
        {
            if (m_SystemUiHider == null || (int)Build.VERSION.SdkInt < 13)
                return;

            if (visible && AUTO_HIDE)
            {
                DelayedHide(AUTO_HIDE_DELAY_MILLIS);
            }

        }


        public void OnClick(View v)
        {
            if (m_SystemUiHider == null)
                return;

            if (TOGGLE_ON_CLICK)
            {
                m_SystemUiHider.Toggle();
            }
            else
            {
                m_SystemUiHider.Show();
            }
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (AUTO_HIDE)
            {
                DelayedHide(AUTO_HIDE_DELAY_MILLIS);
            }

            return false;
        }

    }

}

