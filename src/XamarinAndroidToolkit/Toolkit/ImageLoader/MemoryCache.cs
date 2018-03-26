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
 * Ported from Xamarin Sample App
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Android.Graphics;

namespace MonoDroidToolkit
{
    internal class MemoryCache
    {
        private IDictionary<String, Bitmap> cache = new ConcurrentDictionary<String, Bitmap>();

        private List<String> cacheList = new List<String>();

        public void PopCache(int max)
        {
            if (max == 0)
                return;

            if (cacheList.Count >= max)
            {
                if (cache.ContainsKey(cacheList[0]))
                    cache.Remove(cacheList[0]);

                cacheList.RemoveAt(0);
            }


        }

        public Bitmap Get(String id)
        {
            if (!cache.ContainsKey(id))
                return null;

            return cache[id];
        }

        public void Put(string id, Bitmap bitmap)
        {
            if (!cache.ContainsKey(id))
                cache.Add(id, bitmap);

            if (!cacheList.Contains(id))
                cacheList.Add(id);

            //if(m_CacheList.Count == 60)
            //{
            //    for(int i = 30; i >=0; i--)
            //    {
            //        m_Cache.Remove(m_CacheList[i]);
            //        m_CacheList.RemoveAt(i);
            //    }
            //}

        }

        public void Clear()
        {
            cache.Clear();
            cacheList.Clear();
        }

    }
}