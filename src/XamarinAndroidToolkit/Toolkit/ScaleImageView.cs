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
 * Ported from mic: https://github.com/matabii/scale-imageview-android/blob/master/src/com/matabii/dev/scaleimageview/ScaleImageView.java
 */

using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroidToolkit
{

    public class ScaleImageViewGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        private readonly ScaleImageView scaleImageView;
        public ScaleImageViewGestureDetector(ScaleImageView imageView)
        {
            scaleImageView = imageView;
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            scaleImageView.MaxZoomTo((int)e.GetX(), (int)e.GetY());
            scaleImageView.Cutting();
            return true;
        }
    }

    public class ScaleImageView : ImageView, View.IOnTouchListener
    {
        private Context context;

        private float maxScale = 2.0f;

        private Matrix matrix;
        private float[] matrixValues = new float[9];
        private int width;
        private int height;
        private int intrinsicWidth;
        private int intrinsicHeight;
        private float scale;
        private float minScale;
        private float previousDistance;
        private int previousMoveX;
        private int previousMoveY;

        private bool isScaling;
        private GestureDetector gestureDetector;

        public ScaleImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.context = context;
            Initialize();
        }

        public ScaleImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            this.Initialize();
        }

        public override void SetImageResource(int resId)
        {
            base.SetImageResource(resId);
            this.Initialize();
        }

        private void Initialize()
        {
            this.SetScaleType(ScaleType.Matrix);
            matrix = new Matrix();

            if (Drawable != null)
            {
                intrinsicWidth = Drawable.IntrinsicWidth;
                intrinsicHeight = Drawable.IntrinsicHeight;
                this.SetOnTouchListener(this);
            }

            gestureDetector = new GestureDetector(context, new ScaleImageViewGestureDetector(this));
        }

        protected override bool SetFrame(int l, int t, int r, int b)
        {
            width = r - l;
            height = b - t;

            matrix.Reset();
            var r_norm = r - l;
            scale = (float)r_norm / (float)intrinsicWidth;

            var paddingHeight = 0;
            var paddingWidth = 0;
            if (scale * intrinsicHeight > height)
            {
                scale = (float)height / (float)intrinsicHeight;
                matrix.PostScale(scale, scale);
                paddingWidth = (r - width) / 2;
            }
            else
            {
                matrix.PostScale(scale, scale);
                paddingHeight = (b - height) / 2;
            }

            matrix.PostTranslate(paddingWidth, paddingHeight);
            ImageMatrix = matrix;
            minScale = scale;
            ZoomTo(scale, width / 2, height / 2);
            Cutting();
            return base.SetFrame(l, t, r, b);
        }

        private float GetValue(Matrix matrix, int whichValue)
        {
            matrix.GetValues(matrixValues);
            return matrixValues[whichValue];
        }



        public float Scale
        {
            get { return this.GetValue(matrix, Matrix.MscaleX); }
        }

        public float TranslateX
        {
            get { return this.GetValue(matrix, Matrix.MtransX); }
        }

        public float TranslateY
        {
            get { return this.GetValue(matrix, Matrix.MtransY); }
        }

        public void MaxZoomTo(int x, int y)
        {
            if (this.minScale != this.Scale && (Scale - minScale) > 0.1f)
            {
                var scale = minScale / Scale;
                ZoomTo(scale, x, y);
            }
            else
            {
                var scale = maxScale / Scale;
                ZoomTo(scale, x, y);
            }
        }

    public void ZoomTo(float scale, int x, int y)
    {
        if (Scale * scale < minScale)
        {
            scale = minScale / Scale;
        }
        else
        {
            if (scale >= 1 && Scale * scale > maxScale)
            {
                scale = maxScale / Scale;
            }
        }
        matrix.PostScale(scale, scale);
        //move to center
        matrix.PostTranslate(-(width * scale - width) / 2, -(height * scale - height) / 2);

        //move x and y distance
        matrix.PostTranslate(-(x - (width / 2)) * scale, 0);
        matrix.PostTranslate(0, -(y - (height / 2)) * scale);
        ImageMatrix = matrix;
    }
    
        public void Cutting()
        {
            var width = (int)(intrinsicWidth * Scale);
            var height = (int)(intrinsicHeight * Scale);
            if (TranslateX < -(width - this.width))
            {
				matrix.PostTranslate(-(TranslateX + width - this.width), 0);
            }

            if (TranslateX > 0)
            {
                matrix.PostTranslate(-TranslateX, 0);
            }

            if (TranslateY < -(height - this.height))
            {
				matrix.PostTranslate(0, -(TranslateY + height - this.height));
            }

            if (TranslateY > 0)
            {
                matrix.PostTranslate(0, -TranslateY);
            }

            if (width < this.width)
            {
				matrix.PostTranslate((this.width - width) / 2, 0);
            }

            if (height < this.height)
            {
				matrix.PostTranslate(0, (this.height - height) / 2);
            }

            ImageMatrix = matrix;
        }

        private float Distance(float x0, float x1, float y0, float y1)
        {
            var x = x0 - x1;
            var y = y0 - y1;
            return FloatMath.Sqrt(x * x + y * y);
        }

        private float DispDistance()
        {
            return FloatMath.Sqrt(width * width + height * height);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (gestureDetector.OnTouchEvent(e))
            {
                previousMoveX = (int)e.GetX();
                previousMoveY = (int)e.GetY();
                return true;
            }

            var touchCount = e.PointerCount;
            switch (e.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Pointer1Down:
                case MotionEventActions.Pointer2Down:
                    {
                        if (touchCount >= 2)
                        {
                            var distance = this.Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            previousDistance = distance;
                            isScaling = true;
                        }
                    }
                    break;
                
                case MotionEventActions.Move:
                    {
                        if (touchCount >= 2 && isScaling)
                        {
                            var distance = this.Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            var scale = (distance - previousDistance) / this.DispDistance();
                            previousDistance = distance;
                            scale += 1;
                            scale = scale * scale;
                            this.ZoomTo(scale, width / 2, height / 2);
                            this.Cutting();
                        }
                        else if (!isScaling)
                        {
                            var distanceX = previousMoveX - (int)e.GetX();
                            var distanceY = previousMoveY - (int)e.GetY();
                            previousMoveX = (int)e.GetX();
                            previousMoveY = (int)e.GetY();

                            matrix.PostTranslate(-distanceX, -distanceY);
                            this.Cutting();
                        }
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                case MotionEventActions.Pointer2Up:
                    {
                        if (touchCount <= 1)
                        {
                            isScaling = false;
                        }
                    }
                    break;
            }
            return true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return OnTouchEvent(e);
        }
    }
}
