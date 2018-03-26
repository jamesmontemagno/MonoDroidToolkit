

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroidToolkit
{
    public class GridViewNoScroll : GridView
    {

        public GridViewNoScroll(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public GridViewNoScroll(Context context, IAttributeSet attrs,
            int defStyle)
            : base(context, attrs, defStyle)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public GridViewNoScroll(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public GridViewNoScroll(Context context)
            : base(context)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Move)
            {
                return true;
            }
            return base.OnTouchEvent(e);
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // Calculate entire height by providing a very large height hint.
            // But do not use the highest 2 bits of this integer; those are
            // reserved for the MeasureSpec mode.
            var expandSpec = MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, expandSpec);
        }
    }
}