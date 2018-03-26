
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace MonoDroidToolkit
{

    /// <summary>
    /// Port of the SystemUiHider
    /// </summary>
    public abstract class SystemUiHider
    {
        /// <summary>
        /// When this flag is set on older devices makes the staut bar float on top of the activity layout.
        /// This flag isn't used on newer devices because the Action Bar
        /// </summary>
        public static int FLAG_LAYOUT_IN_SCREEN_OLDER_DEVICES = 0x1;

        /// <summary>
        /// Will force to full screen mode
        /// </summary>
        public static int FLAG_FULLSCREEN = 0x2;

        /// <summary>
        /// force to full screen and hide navigation
        /// </summary>
        public static int FLAG_HIDE_NAVIGATION = FLAG_FULLSCREEN | 0x4;


        protected Activity activity;
        protected View anchorView;
        protected int flags;
        protected IOnVisibilityChangeListener onVisibilityChangeListener;

        public static SystemUiHider GetInstance(Activity activity, View anchorView, int flags)
        {
            if ((int)Build.VERSION.SdkInt >= 11)// (honeycomb)
            {
                return new SystemUiHiderHoneycomb(activity, anchorView, flags);
            }
            else
            {
                return new SystemUiHiderBase(activity, anchorView, flags);
            }
        }

        protected SystemUiHider(Activity activity, View anchorView, int flags)
        {
            this.activity = activity;
            this.anchorView = anchorView;
            this.flags = flags;
        }

        public abstract void Setup();
        public abstract bool IsVisible { get; set; }
        public abstract void Hide();
        public abstract void Show();
        public void Toggle()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }


        public void SetOnVisibilityChangeListener(
            IOnVisibilityChangeListener listener)
        {
            if (listener == null)
            {
                listener = new DummyListener();
            }

            onVisibilityChangeListener = listener;
        }

        /// <summary>
        /// A dummy no-op callback for use when there is no other listener set.
        /// </summary>
        public class DummyListener : IOnVisibilityChangeListener
        {
            public void OnVisibilityChange(bool visible)
            {
            }
        }

        public interface IOnVisibilityChangeListener
        {
            void OnVisibilityChange(bool visible);
        }
    }
}