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