/*
 * Copyright (C) 2010 Johan Nilsson <http://markupartist.com>
 *
 * Original (https://github.com/johannilsson/android-actionbar) Ported to Xamarin.Android
 * Copyright (C) 2013 LegacyBar - @Cheesebaron & @JamesMontemagno
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