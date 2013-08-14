using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
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
 * Converted from: https://github.com/passsy/android-HoloCircularProgressBar
 */
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace com.refractored.monodroidtoolkit
{
    public class HoloCircularProgressBar : View
    {
        private static string TAG = "CircularProgressBar";
        private const string INSTANCE_STATE_SAVEDSTATE = "saved_state";
        private const string INSTANCE_STATE_PROGRESS = "progress";
        private const string INSTANCE_STATE_MARKER_PROGRESS = "marker_progress";

        private readonly bool m_IsInitializing = true;
        private readonly Paint m_BackgroundColorPaint = new Paint();
        private readonly int m_CircleStrokeWidth = 10;
        private readonly int m_ThumbRadius = 20;
        private readonly RectF m_CircleBounds = new RectF();
        private float m_Radius;
        /// <summary>
        /// Gets or sets the progress color
        /// </summary>
        public int ProgressColor { get; set; }
        private readonly Paint m_ProgressColorPaint;
        private readonly int m_ProgressBackgroundColor;
        private float m_Progress = 30.0f;


        private float m_Max = 100.0f;
        public float Max
        {
            get {return m_Max; }
            set
            {
                m_Max = value;
                if (m_Max <= 0)
                    m_Max = 1;
            }
        }

        /// <summary>
        /// gets or sets the progress 0 to Max(default 100)
        /// </summary>
        public float Progress
        {
            get { return m_Progress; }
            set
            {
                if (Math.Abs(m_Progress - value) < float.Epsilon)
                    return;
                
                m_Progress = value;
                m_Overdraw = value >= Max;

                if (m_IsInitializing)
                    return;

                Invalidate();
            }
        }
        private readonly Paint m_ThumbColorPaint = new Paint();
        private float m_MarkerProgress = 0.0f;
        /// <summary>
        /// Gets or sets the marker progress. Settings this will also enable marker
        /// </summary>
        public float MarkerProgress
        {
            get { return m_MarkerProgress; }
            set 
            { 
                m_MarkerProgress = value;
                IsMarkerEnabled = true;
            }
        }

        private readonly Paint m_MarkerColorPaint;
        /// <summary>
        /// Gets or sets if marker is enabled
        /// </summary>
        public bool IsMarkerEnabled { get; set; }

        private readonly int m_Gravity;
        private int m_HorizontalInset = 0;
        private int m_VerticalInset = 0;
        private float m_TranslationOffsetX;
        private float m_TranslationOffsetY;
        private float m_ThumbPosX;
        private float m_ThumbPosY;
        private bool m_Overdraw = false;




        public HoloCircularProgressBar(Context context) :
            this(context, null)
        {
        }



        public HoloCircularProgressBar(Context context, IAttributeSet attrs) :
            this(context, attrs, Resource.Attribute.circularProgressBarStyle)
        {
        }

        public HoloCircularProgressBar(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

            var indeterminate = false;
            var indeterminateInterval = Resources.GetInteger(Resource.Integer.circular_indeterminent_interval);

            var a = context.ObtainStyledAttributes(attrs,
                                                    Resource.Styleable.HoloCircularProgressBar,
                                                    defStyle,
                                                    0);

            ProgressColor = a.GetColor(Resource.Styleable.HoloCircularProgressBar_circular_progress_color, Color.Cyan);
            m_ProgressBackgroundColor = a.GetColor(Resource.Styleable.HoloCircularProgressBar_circular_progress_background_color, Color.Magenta);
            Progress = a.GetFloat(Resource.Styleable.HoloCircularProgressBar_circular_progress, 0.0f);
            MarkerProgress = a.GetFloat(Resource.Styleable.HoloCircularProgressBar_circular_marker_progress, 0.0f);
            m_CircleStrokeWidth = (int)a.GetDimension(Resource.Styleable.HoloCircularProgressBar_circular_stroke_width, 10);
            m_Gravity = a.GetInt(Resource.Styleable.HoloCircularProgressBar_circular_gravity, (int)GravityFlags.Center);

            indeterminateInterval = a.GetInteger(Resource.Styleable.HoloCircularProgressBar_circular_indeterminate_interval, indeterminateInterval);
            indeterminate = a.GetBoolean(Resource.Styleable.HoloCircularProgressBar_circular_indeterminate, indeterminate);

            a.Recycle();

            m_ThumbRadius = m_CircleStrokeWidth*2;

            m_BackgroundColorPaint = new Paint(PaintFlags.AntiAlias);
            m_BackgroundColorPaint.Color = new Color(m_ProgressBackgroundColor);
            m_BackgroundColorPaint.SetStyle(Paint.Style.Stroke);
            m_BackgroundColorPaint.StrokeWidth = m_CircleStrokeWidth;

            m_MarkerColorPaint = new Paint(PaintFlags.AntiAlias);
            m_MarkerColorPaint.Color = new Color(m_ProgressBackgroundColor);
            m_MarkerColorPaint.SetStyle(Paint.Style.Stroke);
            m_MarkerColorPaint.StrokeWidth = (m_CircleStrokeWidth / 2.0f);

            m_ProgressColorPaint = new Paint(PaintFlags.AntiAlias);
            m_ProgressColorPaint.Color = new Color(ProgressColor);
            m_ProgressColorPaint.SetStyle(Paint.Style.Stroke);
            m_ProgressColorPaint.StrokeWidth = m_CircleStrokeWidth;

            m_ThumbColorPaint = new Paint(PaintFlags.AntiAlias);
            m_ThumbColorPaint.Color = new Color(ProgressColor);
            m_ThumbColorPaint.SetStyle(Paint.Style.FillAndStroke);
            m_ThumbColorPaint.StrokeWidth = m_CircleStrokeWidth;


            m_IndeterminateInterval = indeterminateInterval;
            Indeterminante = indeterminate;

            m_IsInitializing = false;
        }

        protected override void OnDraw(Canvas canvas)
        {
            // All of our positions are using our internal coordinate system.
            // Instead of translating
            // them we let Canvas do the work for us.
            canvas.Translate(m_TranslationOffsetX, m_TranslationOffsetY);
            var progressRotation = CurrentRoatation;

            //draw the background
            if (!m_Overdraw)
            {
                canvas.DrawArc(m_CircleBounds, 270, -(360 - progressRotation), false, m_BackgroundColorPaint);
            }

            //draw the progress or a full circle if overdraw is true
            canvas.DrawArc(m_CircleBounds, 270, m_Overdraw ? 360 : progressRotation, false, m_ProgressColorPaint);

            //draw the marker at the correct rortated position
            if (IsMarkerEnabled)
            {
                var markerRotation = MarkerRotation;
                canvas.Save();
                canvas.Rotate(markerRotation - 90);
                canvas.DrawLine(m_ThumbPosX + m_ThumbRadius / 2.0f * 1.4f, m_ThumbPosY,
                                m_ThumbPosX - m_ThumbRadius / 2.0f * 1.4f, m_ThumbPosY, m_MarkerColorPaint);
                canvas.Restore();
            }

            //draw the thumb square at the correct rotated position
            canvas.Save();
            canvas.Rotate(progressRotation - 90);
            //rotate the square by 45 degrees
            canvas.Rotate(45, m_ThumbPosX, m_ThumbPosY);
            var rect = new RectF();
            rect.Left = m_ThumbPosX - m_ThumbRadius / 3.0f;
            rect.Right = m_ThumbPosX + m_ThumbRadius / 3.0f;
            rect.Top = m_ThumbPosY - m_ThumbRadius / 3.0f;
            rect.Bottom = m_ThumbPosY + m_ThumbRadius / 3.0f;
            canvas.DrawRect(rect, m_ThumbColorPaint);
            canvas.Restore();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var height = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);
            var width = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);
            var min = Math.Min(width, height);

            SetMeasuredDimension(min, height);
            var halfWidth = min*.5f;
            m_Radius = halfWidth - m_ThumbRadius;
            m_CircleBounds.Set(-m_Radius, -m_Radius, m_Radius, m_Radius);
            m_ThumbPosX = (float) (m_Radius*Math.Cos(0));
            m_ThumbPosY = (float) (m_Radius*Math.Sin(0));

            ComputeInsets(width - min, height - min);

            m_TranslationOffsetX = halfWidth + m_HorizontalInset;
            m_TranslationOffsetY = halfWidth + m_VerticalInset;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            var bundle = state as Bundle;
            if (bundle != null)
            {
                Progress = bundle.GetFloat(INSTANCE_STATE_PROGRESS);
                MarkerProgress = bundle.GetFloat(INSTANCE_STATE_MARKER_PROGRESS);
                base.OnRestoreInstanceState(bundle.GetParcelable(INSTANCE_STATE_SAVEDSTATE) as IParcelable);
                return;
            }

            base.OnRestoreInstanceState(state);
        }

        protected override IParcelable OnSaveInstanceState()
        {
            var bundle = new Bundle();
            bundle.PutParcelable(INSTANCE_STATE_SAVEDSTATE, base.OnSaveInstanceState());
            bundle.PutFloat(INSTANCE_STATE_PROGRESS, Progress);
            bundle.PutFloat(INSTANCE_STATE_MARKER_PROGRESS, MarkerProgress);
            return bundle;
        }

        private int m_IndeterminateInterval = 100;
        public int IndeterminanteInterval
        {
            get { return m_IndeterminateInterval; }
            set
            {
                m_IndeterminateInterval = value;
                if (m_IndeterminateInterval < 50)
                    m_IndeterminateInterval = 50;

            }
        }

        private bool m_Indeterminate = false;
        /// <summary>
        /// Get or set if the Spinner is indeterminante
        /// </summary>
        public bool Indeterminante
        {
            get { return m_Indeterminate; }
            set
            {

                m_Indeterminate = value;
                if (m_Indeterminate)
                {
                    //start
                    UpdateIndeterminante();
                }
                else
                {
                    //stop
                    if (m_IndeterminanteHandler == null)
                        return;

                    m_IndeterminanteHandler.RemoveCallbacks(IndeterminanteRunnable);
                }
            }
        }

        readonly Handler m_IndeterminanteHandler = new Handler();
        private void UpdateIndeterminante()
        {
            if (m_IndeterminanteHandler == null)
                return;

            m_IndeterminanteHandler.RemoveCallbacks(IndeterminanteRunnable);
            m_IndeterminanteHandler.PostDelayed(IndeterminanteRunnable, m_IndeterminateInterval);
        }


        private void IndeterminanteRunnable()
        {
            var newProgress = (Progress) + .5f;
            if (newProgress > Max)
                newProgress = 0;

            Progress = newProgress;

            if (Indeterminante)
                m_IndeterminanteHandler.PostDelayed(IndeterminanteRunnable, m_IndeterminateInterval);
        }


        private void ComputeInsets(int dx, int dy) 
        {
            var absoluteGravity = m_Gravity;

            //TODO if JB need to check absolute positon
            /*if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN)
            {
                layoutDirection = getLayoutDirection();
                absoluteGravity = Gravity.getAbsoluteGravity(mGravity, layoutDirection);
            }
            */

            switch ((GravityFlags)(absoluteGravity & (long) GravityFlags.HorizontalGravityMask)) 
            {
		        case GravityFlags.Left:
			        m_HorizontalInset = 0;
			        break;
		        case GravityFlags.Right:
			        m_HorizontalInset = dx;
			        break;
		        case GravityFlags.CenterHorizontal:
		        default:
			        m_HorizontalInset = dx / 2;
			        break;
		    }

            switch ((GravityFlags)(absoluteGravity & (long)GravityFlags.VerticalGravityMask))
            {
		        case GravityFlags.Top:
			        m_VerticalInset = 0;
			        break;
		        case GravityFlags.Bottom:
			        m_VerticalInset = dy;
			        break;
		        case GravityFlags.CenterVertical:
		        default:
			        m_VerticalInset = dy / 2;
			        break;
		    }
	    }

        /// <summary>
        /// Gets the current rotation
        /// </summary>
        private float CurrentRoatation
        {
            get { return 360*(m_Progress/Max); }
        }

        /// <summary>
        /// Gets the marker rotation
        /// </summary>
        private float MarkerRotation
        {
            get { return 360*(m_MarkerProgress/Max); }
        }

       


    }
}