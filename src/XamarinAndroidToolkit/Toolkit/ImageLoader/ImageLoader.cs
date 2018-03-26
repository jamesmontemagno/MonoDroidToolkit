

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Java.IO;
using Java.Net;
using File = Java.IO.File;

namespace MonoDroidToolkit
{

    public class ImageLoader
    {
        public static void CopyStream(Stream inputStream, OutputStream os)
        {
            var buffer_size = 1024;
            try
            {
                var bytes = new byte[buffer_size];
                for (;;)
                {
                    var count = inputStream.Read(bytes, 0, buffer_size);
                    if (count <= 0)
                        break;
                    os.Write(bytes, 0, count);
                }
            }
            catch (Exception ex) { }
        }

        internal class PhotoToLoad
        {
            public String Url;
            public ImageView ImageView;
            public PhotoToLoad(String url, ImageView imageView)
            {
                Url = url;
                ImageView = imageView;
            }
        }

        private MemoryCache memoryCache = new MemoryCache();
        private FileCache fileCache;

        private IDictionary<ImageView, String> imageViews = new ConcurrentDictionary<ImageView, String>();
        private int stubId = -1;

        private int scale;
        private int maxImages;

        public ImageLoader(Context context, int scale = 64, int maxImages = 0)
        {
            fileCache = new FileCache(context);
            this.scale = scale;
            this.maxImages = maxImages;
        }

        public void DisplayImage(string url, ImageView imageView, int defaultResourceId)
        {
            stubId = defaultResourceId;
            if (imageViews.ContainsKey(imageView))
            {
                if (defaultResourceId != -1)
                    imageView.SetImageResource(defaultResourceId);

                imageViews.Remove(imageView);
            }

            memoryCache.PopCache(maxImages);




            //if (m_ImageList.Contains(imageView))
            //    m_ImageList.Remove(imageView);


            /*if (m_MemoryCache..Count == 10)
            {
                var tempImageView = m_ImageList[0];
                tempImageView.SetImageResource(m_StubID);
                m_ImageViews.Remove(tempImageView);
                m_ImageList.RemoveAt(0);
            }*/

            imageViews.Add(imageView, url);
            //m_ImageList.Add(imageView);





            var bitmap = memoryCache.Get(url);
            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
            }
            else
            {
                QueueImage(url, imageView);
                if (defaultResourceId != -1)
                    imageView.SetImageResource(defaultResourceId);
            }
        }

        public void QueueImage(string url, ImageView imageView)
        {
            var photoToUpload = new PhotoToLoad(url, imageView);
            ThreadPool.QueueUserWorkItem(state => LoadPhoto(photoToUpload));
        }

        private Bitmap GetBitmap(string url)
        {
            var f = fileCache.GetFile(url);

            ////from SD cache
            var b = DecodeFile(f, scale);
            if (b != null)
                return b;

            ////from web
            try
            {
                Bitmap bitmap = null;
                var imageUrl = new URL(url);
                var conn = (HttpURLConnection)imageUrl.OpenConnection();
                conn.ConnectTimeout = 5000;
                conn.ReadTimeout = 5000;
                conn.InstanceFollowRedirects = true;

                if (conn.ErrorStream != null)
                    return null;

                var inputStream = conn.InputStream;
                OutputStream os = new FileOutputStream(f);
                CopyStream(inputStream, os);
                os.Close();
                bitmap = DecodeFile(f, scale);
                return bitmap;
            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                return null;
            }
        }

        private static Bitmap DecodeFile(File file, int requiredSize)
        {
            try
            {
                //decode image size
                var options = new BitmapFactory.Options { InJustDecodeBounds = true, InPurgeable = true };

                BitmapFactory.DecodeStream(new FileStream(file.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), null, options);//FileStream?

                //Find the correct scale value. It should be the power of 2.
                var tempWidth = options.OutWidth;
				var tempHeight = options.OutHeight;

                var scale = 1;

                while (true)
                {
                    if (tempWidth / 2 < requiredSize || tempHeight / 2 < requiredSize)
                        break;

                    tempWidth /= 2;
                    tempHeight /= 2;
                    scale *= 2;
                }

				//decode with inSampleSize
				var options2 = new BitmapFactory.Options { InSampleSize = scale };

                return BitmapFactory.DecodeStream(new FileStream(file.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), null, options2);//FileStream?
            }
            catch (Exception e)
            {

            }

            return null;
        }

        internal bool ImageViewReused(PhotoToLoad photoToLoad)
        {
            try
            {
                if (!imageViews.ContainsKey(photoToLoad.ImageView))
                    return true;

                if (!imageViews[photoToLoad.ImageView].Equals(photoToLoad.Url))
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public void LoadPhoto(object param)
        {
            var photoToLoad = param as PhotoToLoad;
            if (photoToLoad == null)
                return;

            if (ImageViewReused(photoToLoad))
                return;

			var bitmap = GetBitmap(photoToLoad.Url);
            memoryCache.Put(photoToLoad.Url, bitmap);
            if (ImageViewReused(photoToLoad))
                return;

            BitmapDisplayer(bitmap, photoToLoad);
        }

        internal void BitmapDisplayer(Bitmap bitmap, PhotoToLoad photoToLoad)
        {
            var activity = (Activity)photoToLoad.ImageView.Context;
            activity.RunOnUiThread(() =>
                                       {
                                           if (ImageViewReused(photoToLoad))
                                               return;
                                           photoToLoad.ImageView.Visibility = Android.Views.ViewStates.Visible;
                                           if (bitmap != null)
                                               photoToLoad.ImageView.SetImageBitmap(bitmap);
                                           else if (stubId != -1)
                                               photoToLoad.ImageView.SetImageResource(stubId);
                                       });
        }

        public void ClearCache()
        {
            memoryCache.Clear();
            //m_FileCache.Clear();
            imageViews.Clear();
        }

        public void ClearFileCache()
        {
            fileCache.Clear();
        }
    }
}