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
using Android.Preferences;
using Android.Util;

namespace com.refractored.monodroidtoolkit.preferneces
{
    /// <summary>
    /// Specify a value list of integers for your list preference
    /// </summary>
    public class IntListPreference : ListPreference
    {
        public IntListPreference(Context context)
            : base(context)
        {
        }

        public IntListPreference(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

        }


        protected override string GetPersistedString(string defaultReturnValue)
        {

            return GetPersistedInt(0).ToString();
        }

        protected override bool PersistString(string value)
        {
            int persistValue;
            int.TryParse(value, out persistValue);

            return PersistInt(persistValue);
        }
    }
}