
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