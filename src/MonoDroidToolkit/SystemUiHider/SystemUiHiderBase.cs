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
    public class SystemUiHiderBase : SystemUiHider
    {

        public SystemUiHiderBase(Activity activity, View anchorView, int flags) :
            base(activity, anchorView, flags)
        {
        }

        public override void Setup()
        {
            if ((m_Flags & FLAG_LAYOUT_IN_SCREEN_OLDER_DEVICES) == 0)
            {
                m_Activity.Window.SetFlags(WindowManagerFlags.LayoutInScreen | WindowManagerFlags.LayoutNoLimits,
                                            WindowManagerFlags.LayoutInScreen | WindowManagerFlags.LayoutNoLimits);
            }
        }

        private bool m_IsVisible = true;
        public override bool IsVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; }
        }

        public override void Hide()
        {
            if ((m_Flags & FLAG_FULLSCREEN) != 0)
            {
                m_Activity.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            }
            m_OnVisibilityChangeListener.OnVisibilityChange(false);
            m_IsVisible = false;
        }

        public override void Show()
        {
            if ((m_Flags & FLAG_FULLSCREEN) != 0)
            {
                m_Activity.Window.SetFlags(0, WindowManagerFlags.Fullscreen);
            }
            m_OnVisibilityChangeListener.OnVisibilityChange(true);
            m_IsVisible = true;
        }
    }
}