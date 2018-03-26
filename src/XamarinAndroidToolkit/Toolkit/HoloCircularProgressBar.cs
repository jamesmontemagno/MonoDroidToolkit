using System;
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
using Android.Util;
using Android.Views;

namespace MonoDroidToolkit
{
	public class HoloCircularProgressBar : View
    {
        private static string TAG = "CircularProgressBar";
        private const string INSTANCE_STATE_SAVEDSTATE = "saved_state";
        private const string INSTANCE_STATE_PROGRESS = "progress";
        private const string INSTANCE_STATE_MARKER_PROGRESS = "marker_progress";

        private readonly bool isInitializing = true;
        private Paint backgroundColorPaint = new Paint();
        private int circleStrokeWidth = 10;
        private int thumbRadius = 20;
        private readonly RectF circleBounds = new RectF();
        private float radius;
        private int progressColor;
        private int progressBackgroundColor;

        public int CircleStrokeWidth
		{
			get => circleStrokeWidth;
			set
			{
				if (circleStrokeWidth == value)
					return;

				circleStrokeWidth = value;
				thumbRadius = circleStrokeWidth * 2;
				ProgressColor = ProgressColor;
				ProgressBackgroundColor = ProgressBackgroundColor;
			}
		}
		/// <summary>
		/// Gets or sets the progress color
		/// </summary>
		public int ProgressColor
		{
			get => progressColor;
			set
			{
				progressColor = value;
				progressColorPaint = new Paint(PaintFlags.AntiAlias);
				progressColorPaint.Color = new Color(progressColor);
				progressColorPaint.SetStyle(Paint.Style.Stroke);
				progressColorPaint.StrokeWidth = circleStrokeWidth;

				thumbColorPaint = new Paint(PaintFlags.AntiAlias);
				thumbColorPaint.Color = new Color(progressColor);
				thumbColorPaint.SetStyle(Paint.Style.FillAndStroke);
				thumbColorPaint.StrokeWidth = circleStrokeWidth;
				Invalidate();
			}
		}

		public int ProgressBackgroundColor
		{
			get => progressBackgroundColor;
			set
			{
				progressBackgroundColor = value;
				backgroundColorPaint = new Paint(PaintFlags.AntiAlias);
				backgroundColorPaint.Color = new Color(progressBackgroundColor);
				backgroundColorPaint.SetStyle(Paint.Style.Stroke);
				backgroundColorPaint.StrokeWidth = circleStrokeWidth;

				markerColorPaint = new Paint(PaintFlags.AntiAlias);
				markerColorPaint.Color = new Color(progressBackgroundColor);
				markerColorPaint.SetStyle(Paint.Style.Stroke);
				markerColorPaint.StrokeWidth = (circleStrokeWidth / 2.0f);
				Invalidate();
			}
		}

		private int indeterminateInterval = 100;
        public int IndeterminateInterval
		{
			get => indeterminateInterval;
			set
			{
				indeterminateInterval = value;
				if (indeterminateInterval < 50)
					indeterminateInterval = 50;

			}
		}

		private bool indeterminate = false;
        /// <summary>
        /// Get or set if the Spinner is indeterminate
        /// </summary>
        public bool Indeterminate
		{
			get => indeterminate;
			set
			{

				indeterminate = value;
				if (indeterminate)
				{
					//start
					UpdateIndeterminate();
				}
				else
				{
					//stop
					if (indeterminateHandler == null)
						return;

					indeterminateHandler.RemoveCallbacks(IndeterminateRunnable);
				}
			}
		}

		private Paint progressColorPaint;
        private float progress = 30.0f;


        private float max = 100.0f;
        public float Max
		{
			get => max;
			set
			{
				max = value;
				if (max <= 0)
					max = 1;
			}
		}

		/// <summary>
		/// gets or sets the progress 0 to Max(default 100)
		/// </summary>
		public float Progress
		{
			get => progress;
			set
			{
				if (Math.Abs(progress - value) < float.Epsilon)
					return;

				progress = value;
				overdraw = value >= Max;

				if (isInitializing)
					return;

				Invalidate();
			}
		}
		private Paint thumbColorPaint = new Paint();
        private float markerProgress = 0.0f;
        /// <summary>
        /// Gets or sets the marker progress. Settings this will also enable marker
        /// </summary>
        public float MarkerProgress
		{
			get => markerProgress;
			set
			{
				markerProgress = value;
				IsMarkerEnabled = true;
			}
		}

		private Paint markerColorPaint;
        /// <summary>
        /// Gets or sets if marker is enabled
        /// </summary>
        public bool IsMarkerEnabled { get; set; }

        private readonly int gravity;
        private int horizontalInset = 0;
        private int verticalInset = 0;
        private float translationOffsetX;
        private float translationOffsetY;
        private float thumbPosX;
        private float thumbPosY;
        private bool overdraw = false;




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

            ProgressColor = a.GetColor(Resource.Styleable.HoloCircularProgressBar_circular_progress_color, Color.Red);
            ProgressBackgroundColor = a.GetColor(Resource.Styleable.HoloCircularProgressBar_circular_progress_background_color, Color.White);
            Progress = a.GetFloat(Resource.Styleable.HoloCircularProgressBar_circular_progress, 0.0f);
            MarkerProgress = a.GetFloat(Resource.Styleable.HoloCircularProgressBar_circular_marker_progress, 0.0f);
            circleStrokeWidth = (int)a.GetDimension(Resource.Styleable.HoloCircularProgressBar_circular_stroke_width, 10);
            gravity = a.GetInt(Resource.Styleable.HoloCircularProgressBar_circular_gravity, (int)GravityFlags.Center);

            indeterminateInterval = a.GetInteger(Resource.Styleable.HoloCircularProgressBar_circular_indeterminate_interval, indeterminateInterval);
            indeterminate = a.GetBoolean(Resource.Styleable.HoloCircularProgressBar_circular_indeterminate, indeterminate);

            a.Recycle();

            thumbRadius = circleStrokeWidth * 2;

            this.indeterminateInterval = indeterminateInterval;
            Indeterminate = indeterminate;

            isInitializing = false;
        }

        protected override void OnDraw(Canvas canvas)
        {
            // All of our positions are using our internal coordinate system.
            // Instead of translating
            // them we let Canvas do the work for us.
            canvas.Translate(translationOffsetX, translationOffsetY);
            var progressRotation = CurrentRoatation;

            //draw the background
            if (!overdraw)
            {
                canvas.DrawArc(circleBounds, 270, -(360 - progressRotation), false, backgroundColorPaint);
            }

            //draw the progress or a full circle if overdraw is true
            canvas.DrawArc(circleBounds, 270, overdraw ? 360 : progressRotation, false, progressColorPaint);

            //draw the marker at the correct rortated position
            if (IsMarkerEnabled)
            {
                var markerRotation = MarkerRotation;
                canvas.Save();
                canvas.Rotate(markerRotation - 90);
                canvas.DrawLine(thumbPosX + thumbRadius / 2.0f * 1.4f, thumbPosY,
                                thumbPosX - thumbRadius / 2.0f * 1.4f, thumbPosY, markerColorPaint);
                canvas.Restore();
            }

            //draw the thumb square at the correct rotated position
            canvas.Save();
            canvas.Rotate(progressRotation - 90);
            //rotate the square by 45 degrees
            canvas.Rotate(45, thumbPosX, thumbPosY);
            var rect = new RectF();
            rect.Left = thumbPosX - thumbRadius / 3.0f;
            rect.Right = thumbPosX + thumbRadius / 3.0f;
            rect.Top = thumbPosY - thumbRadius / 3.0f;
            rect.Bottom = thumbPosY + thumbRadius / 3.0f;
            canvas.DrawRect(rect, thumbColorPaint);
            canvas.Restore();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var height = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);
            var width = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);
            var min = Math.Min(width, height);

            SetMeasuredDimension(min, height);
            var halfWidth = min * .5f;
            radius = halfWidth - thumbRadius;
            circleBounds.Set(-radius, -radius, radius, radius);
            thumbPosX = (float)(radius * Math.Cos(0));
            thumbPosY = (float)(radius * Math.Sin(0));

            ComputeInsets(width - min, height - min);

            translationOffsetX = halfWidth + horizontalInset;
            translationOffsetY = halfWidth + verticalInset;
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



        readonly Handler indeterminateHandler = new Handler();
        private void UpdateIndeterminate()
        {
            if (indeterminateHandler == null)
                return;

            indeterminateHandler.RemoveCallbacks(IndeterminateRunnable);
            indeterminateHandler.PostDelayed(IndeterminateRunnable, indeterminateInterval);
        }


        private void IndeterminateRunnable()
        {
            var newProgress = (Progress) + .5f;
            if (newProgress > Max)
                newProgress = 0;

            Progress = newProgress;

            if (Indeterminate)
                indeterminateHandler.PostDelayed(IndeterminateRunnable, indeterminateInterval);
        }


        private void ComputeInsets(int dx, int dy)
        {
            var absoluteGravity = gravity;

            //TODO if JB need to check absolute positon
            /*if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN)
            {
                layoutDirection = getLayoutDirection();
                absoluteGravity = Gravity.getAbsoluteGravity(mGravity, layoutDirection);
            }
            */

            switch ((GravityFlags)(absoluteGravity & (long)GravityFlags.HorizontalGravityMask))
            {
                case GravityFlags.Left:
                    horizontalInset = 0;
                    break;
                case GravityFlags.Right:
                    horizontalInset = dx;
                    break;
                case GravityFlags.CenterHorizontal:
                default:
                    horizontalInset = dx / 2;
                    break;
            }

            switch ((GravityFlags)(absoluteGravity & (long)GravityFlags.VerticalGravityMask))
            {
                case GravityFlags.Top:
                    verticalInset = 0;
                    break;
                case GravityFlags.Bottom:
                    verticalInset = dy;
                    break;
                case GravityFlags.CenterVertical:
                default:
                    verticalInset = dy / 2;
                    break;
            }
        }

        /// <summary>
        /// Gets the current rotation
        /// </summary>
        private float CurrentRoatation
        {
            get { return 360 * (progress / Max); }
        }

        /// <summary>
        /// Gets the marker rotation
        /// </summary>
        private float MarkerRotation
        {
            get { return 360 * (markerProgress / Max); }
        }




    }
}