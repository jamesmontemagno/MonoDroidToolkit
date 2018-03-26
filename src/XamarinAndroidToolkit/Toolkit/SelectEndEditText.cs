

using Android.Content;
using Android.Widget;
using Android.Util;

namespace MonoDroidToolkit
{
    public class SelectEndEditText : EditText
    {
        public SelectEndEditText(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public SelectEndEditText(Context context, IAttributeSet attrs,
            int defStyle)
            : base(context, attrs, defStyle)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public SelectEndEditText(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            ResourceIdManager.UpdateIdValues();
        }

        public SelectEndEditText(Context context)
            : base(context)
        {
            ResourceIdManager.UpdateIdValues();
        }

        /// <summary>
        /// On gain focus got ahead and set selection to end.
        /// </summary>
        /// <param name="gainFocus"></param>
        /// <param name="direction"></param>
        /// <param name="previouslyFocusedRect"></param>
        protected override void OnFocusChanged(bool gainFocus, Android.Views.FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
        {
           

            if (gainFocus)
            {
                if (!string.IsNullOrWhiteSpace(this.Text))
                {
                    SetSelection(this.Text.Length, this.Text.Length); 
                }
            }

            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        }
    }
}