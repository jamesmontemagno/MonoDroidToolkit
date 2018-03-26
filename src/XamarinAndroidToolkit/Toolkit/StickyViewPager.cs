

using Android.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;

namespace MonoDroidToolkit
{
    public class StickyViewPager : ViewPager
    {
        public bool PagerEnabled { get; set; }

        public StickyViewPager(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            PagerEnabled = true;
            ResourceIdManager.UpdateIdValues();
        }

        public StickyViewPager(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            PagerEnabled = true;
            ResourceIdManager.UpdateIdValues();

        }

        public StickyViewPager(Context context)
            : base(context)
        {
            PagerEnabled = true;
            ResourceIdManager.UpdateIdValues();
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (PagerEnabled)
                return base.OnInterceptTouchEvent(ev);

            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (PagerEnabled)
                return base.OnTouchEvent(e);

            return false;
        }

        public override bool OnInterceptHoverEvent(MotionEvent e)
        {
            if (PagerEnabled)
                return base.OnInterceptHoverEvent(e);

            return false;
        }

      
    }
}