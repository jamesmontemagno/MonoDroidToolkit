/*
 * Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace com.refractored.monodroidtoolkit
{
    public class GridViewNoScroll : GridView
    {

        public GridViewNoScroll(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public GridViewNoScroll(Context context, IAttributeSet attrs,
            int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        public GridViewNoScroll(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public GridViewNoScroll(Context context)
            : base(context)
        {
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
            int expandSpec = MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, expandSpec);
        }
    }
}