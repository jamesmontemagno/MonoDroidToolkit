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
 * 
 * Ported from mic: http://stackoverflow.com/users/907695/mice
 * via: http://stackoverflow.com/questions/1258275/vertical-rotated-label-in-android
 */

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroidToolkit
{
    public class VerticalTextView : TextView
    {
        private bool topDown;

        public VerticalTextView(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Initialize();
        }


        public VerticalTextView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public VerticalTextView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            ResourceIdManager.UpdateIdValues();
            topDown = true;

            var gravity = Gravity;
            if (Android.Views.Gravity.IsVertical(gravity) && (gravity & GravityFlags.VerticalGravityMask) == GravityFlags.Bottom)
            {
                Gravity = ((gravity & GravityFlags.HorizontalGravityMask) | GravityFlags.Top);
                topDown = false;
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(heightMeasureSpec, widthMeasureSpec);
            SetMeasuredDimension(MeasuredHeight, MeasuredWidth);
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            var textPaint = this.Paint;
            textPaint.Color = new Color(this.CurrentTextColor);

            canvas.Save();

            if (topDown)
            {
                canvas.Translate(this.Width, 0);
                canvas.Rotate(90.0f);
            }
            else
            {
                canvas.Translate(0, this.Height);
                canvas.Rotate(-90.0f);
            }

            canvas.Translate(this.CompoundPaddingLeft, this.ExtendedPaddingTop);
            this.Layout.Draw(canvas);
            canvas.Restore();
        }
    }
}
