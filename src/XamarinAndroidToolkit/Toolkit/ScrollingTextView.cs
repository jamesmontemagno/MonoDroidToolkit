

using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace MonoDroidToolkit
{
    public class ScrollingTextView : TextView
    {
        public ScrollingTextView(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public ScrollingTextView(Context context, IAttributeSet attrs,
            int defStyle)
            : base(context, attrs, defStyle)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public ScrollingTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public ScrollingTextView(Context context)
            : base(context)
        {
            ResourceIdManager.UpdateIdValues();
        }

        protected override void OnFocusChanged(bool gainFocus, FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
        {
            if (gainFocus)
                base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        }

        public override void OnWindowFocusChanged(bool hasWindowFocus)
        {
            if (hasWindowFocus)
                base.OnWindowFocusChanged(hasWindowFocus);
        }

        public override bool IsFocused
        {
            get
            {
                return true;
            }
        }
    }
}