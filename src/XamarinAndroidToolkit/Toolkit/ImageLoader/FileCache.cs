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
using Android.Content;
using Java.IO;

namespace MonoDroidToolkit
{
    public class FileCache
    {
        private File cacheDir;

        public FileCache(Context context)
        {
            var stuff = Android.OS.Environment.ExternalStorageState;
            if (Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted))
            {
                cacheDir = new File(Android.OS.Environment.ExternalStorageDirectory, "Android/data/" + context.ApplicationContext.PackageName);
                cacheDir = context.ExternalCacheDir;
            }
            else
            {
                cacheDir = context.CacheDir;
            }

            if (cacheDir == null)
                cacheDir = context.CacheDir;




            if (!cacheDir.Exists())
            {
                var success = cacheDir.Mkdirs();
            }
        }

        public File GetFile(string url)
        {
            var fileName = url.GetHashCode().ToString();
            var file = new File(cacheDir, fileName);
            return file;
        }

        public void Clear()
        {
            var files = cacheDir.ListFiles();
            if (files == null)
                return;

            foreach (var file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                    //TODO log exception
                }
            }
        }


    }
}