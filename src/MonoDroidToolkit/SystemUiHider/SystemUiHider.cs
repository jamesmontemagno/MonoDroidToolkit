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


        protected Activity m_Activity;
        protected View m_AnchorView;
        protected int m_Flags;
        protected IOnVisibilityChangeListener m_OnVisibilityChangeListener;

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
            m_Activity = activity;
            m_AnchorView = anchorView;
            m_Flags = flags;
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

            m_OnVisibilityChangeListener = listener;
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