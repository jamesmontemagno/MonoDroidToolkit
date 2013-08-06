/*
  * Original By:
  * Copyright 2013 Prateek Srivastava (@f2prateek)
  * Modified for Xamarin.Android:
  * Copyright 2013 James Montemagno (@JamesMontemagno)
  *  
  * Licensed under the Apache License, Version 2.0 (the "License");
  * you may not use this file except in compliance with the License.
  * You may obtain a copy of the License at
  *
  * 	http://www.apache.org/licenses/LICENSE-2.0
  *
  * Unless required by applicable law or agreed to in writing, software
  * distributed under the License is distributed on an "AS IS" BASIS,
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
*/

using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace com.refractored.monodroidtoolkit
{
    public class ProgressButton : CompoundButton
    {
        private int m_Max = 100;
        /// <summary>
        /// Gets or sets the maximum progress, Defaults to 100
        /// </summary>
        public int Max
        {
            get { return m_Max; }
            set 
            { 
                m_Max = value; 
                Invalidate();
            }
        }

        private int m_Progress = 0;
        /// <summary>
        /// Gets or sets the current progress, Defaults to 0.
        /// </summary>
        public int Progress
        {
            get { return m_Progress; }
            set
            {
                if(value > Max || value < 0)
                     throw new ArgumentException(String.Format("Progress ({0}) must be between {1} and {2}", value, 0, Max));
                
                m_Progress = value;
                Invalidate();
            }
        }


        private Drawable m_ShadowDrawable;
        /// <summary>
        /// Gets or sets the drawable used as the shadow
        /// </summary>
        public Drawable ShadowDrawable
        {
            get { return m_ShadowDrawable; }
            set 
            { 
                m_ShadowDrawable = value;
                m_DrawableSize = m_ShadowDrawable.IntrinsicWidth; 
                Invalidate();
            }
        }

        private Drawable m_UnpinnedDrawable;
        /// <summary>
        /// Gets or sets the drawable displayed when the user unpins an item.
        /// </summary>
        public Drawable UnpinnedDrawable
        {
            get { return m_UnpinnedDrawable; }
            set { m_UnpinnedDrawable = value; Invalidate(); }
        }

        private Drawable m_PinnedDrawable;
        /// <summary>
        /// Gets or sets the drawable displayed when the user pins an item.
        /// </summary>
        public Drawable PinnedDrawable {
            get { return m_PinnedDrawable; }
            set { m_PinnedDrawable = value; Invalidate(); }
        }

   
        /// <summary>
        /// Gets or sets the paint for the circle.
        /// </summary>
        private Paint CirclePaint { get; set; }

        public int CircleColor
        {
            get { return CirclePaint == null ? 0 : CirclePaint.Color; }
            set
            {
                if (CirclePaint == null)
                    return;

                CirclePaint.Color = new Color(value);
            }
        }

        public int ProgressColor
        {
            get { return ProgressPaint == null ? 0 : ProgressPaint.Color; }
            set
            {
                if (ProgressPaint == null)
                    return;

                ProgressPaint.Color = new Color(value);
            }
        }

        private Paint ProgressPaint { get; set; }
        private readonly Rect m_TempRect = new Rect();
        private readonly RectF m_TempRectF = new RectF();
        private int m_DrawableSize;
        private int m_InnerSize;

        /// <summary>
        /// Gets or sets the inner isze
        /// </summary>
        public int InnerSize
        {
            get { return m_InnerSize; }
            set 
            { 
                m_InnerSize = value;
                Invalidate();
            }
        }


        public ProgressButton(Context context, IAttributeSet attrs,
            int defStyle)
            : base(context, attrs, defStyle)
        {
            ResourceIdManager.UpdateIdValues();
            Initialize(context, attrs, defStyle);
        }

        public ProgressButton(Context context, IAttributeSet attrs)
            : this(context, attrs, Resource.Attribute.progressButtonStyle)
        {
        }

        public ProgressButton(Context context)
            : this(context, null)
        {
        }

        private void Initialize(Context context, IAttributeSet attrs,
            int defStyle)
        {
            //set defaults first:

            var circleColor = Resources.GetColor(Resource.Color.progress_default_circle_color);
            var progressColor = Resources.GetColor(Resource.Color.progress_default_progress_color);
            var pinnedDrawable = Resource.Drawable.pin_progress_pinned;
            var unpinnedDrawable = Resource.Drawable.pin_progress_unpinned;
            var shadowDrawable = Resource.Drawable.pin_progress_shadow;
            var canChecked = false;
            var canClickable = false;
            var canFocusable = false;
            m_InnerSize = Resources.GetDimensionPixelSize(Resource.Dimension.progress_inner_size);


            if (context != null && attrs != null)
            {
                var a = context.ObtainStyledAttributes(attrs,
                                                       Resource.Styleable.ProgressButton,
                                                       Resource.Attribute.progressButtonStyle,
                                                       Resource.Style.ProgressButton_Pin);

                m_Progress = a.GetInteger(Resource.Styleable.ProgressButton_progress, 0);
                m_Max = a.GetInteger(Resource.Styleable.ProgressButton_max, 100);

                circleColor = a.GetColor(Resource.Styleable.ProgressButton_circleColor, circleColor);
                progressColor = a.GetColor(Resource.Styleable.ProgressButton_progressColor, progressColor);
                pinnedDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_pinnedDrawable, pinnedDrawable);
                unpinnedDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_unpinnedDrawable, unpinnedDrawable);
                shadowDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_shadowDrawable, shadowDrawable);

                m_InnerSize = a.GetDimensionPixelSize(Resource.Styleable.ProgressButton_innerSize, 0);
                if(m_InnerSize == 0)
                    m_InnerSize = Resources.GetDimensionPixelSize(Resource.Dimension.progress_inner_size);

                canChecked = a.GetBoolean(Resource.Styleable.ProgressButton_pinned, canChecked);
                canClickable = a.GetBoolean(Resource.Styleable.ProgressButton_android_clickable, canClickable);
                canFocusable = a.GetBoolean(Resource.Styleable.ProgressButton_android_focusable, canFocusable);

                SetBackgroundDrawable(a.GetDrawable(Resource.Styleable.ProgressButton_android_selectableItemBackground));

                a.Recycle();
            }

            m_PinnedDrawable = Resources.GetDrawable(pinnedDrawable);
            m_PinnedDrawable.SetCallback(this);

            m_UnpinnedDrawable = Resources.GetDrawable(unpinnedDrawable);
            m_UnpinnedDrawable.SetCallback(this);

            m_ShadowDrawable = Resources.GetDrawable(shadowDrawable);
            m_ShadowDrawable.SetCallback(this);

            m_DrawableSize = m_ShadowDrawable.IntrinsicWidth;

            Checked = canChecked;
            Clickable = canClickable;
            Focusable = canFocusable;

            CirclePaint = new Paint {Color = circleColor, AntiAlias = true};
            ProgressPaint = new Paint {Color = progressColor, AntiAlias = true};
        }


        public bool Pinned
        {
            get { return Checked; }
            set { Checked = value; }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            SetMeasuredDimension(ResolveSize(m_DrawableSize, widthMeasureSpec), 
                ResolveSize(m_DrawableSize, heightMeasureSpec));
        }

        protected override void DrawableStateChanged()
        {
            base.DrawableStateChanged();

            if (m_PinnedDrawable.IsStateful)
            {
                m_PinnedDrawable.SetState(GetDrawableState());
            }

            if (m_UnpinnedDrawable.IsStateful)
            {
                m_UnpinnedDrawable.SetState(GetDrawableState());
            }

            if (m_ShadowDrawable.IsStateful)
            {
                m_ShadowDrawable.SetState(GetDrawableState());
            }

            Invalidate();
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            m_TempRect.Set(0, 0, m_DrawableSize, m_DrawableSize);
            m_TempRect.Offset((Width - m_DrawableSize) / 2, (Height - m_DrawableSize) / 2);

            m_TempRectF.Set(-0.5f, -0.5f, m_InnerSize + 0.5f, m_InnerSize + 0.5f);
            m_TempRectF.Offset((Width - m_InnerSize) / 2, (Height - m_InnerSize) / 2);

            canvas.DrawArc(m_TempRectF, 0, 360, true, CirclePaint);
            canvas.DrawArc(m_TempRectF,-90, 360 * Progress / Max, true, ProgressPaint);

            var iconDrawable = Checked ? PinnedDrawable : UnpinnedDrawable;
            iconDrawable.Bounds = m_TempRect;
            iconDrawable.Draw(canvas);

            m_ShadowDrawable.Bounds = m_TempRect;
            m_ShadowDrawable.Draw(canvas);

        }

        public override IParcelable OnSaveInstanceState()
        {
            if (SaveEnabled)
            {
                var bundle = new Bundle();
                bundle.PutParcelable("instanceState", base.OnSaveInstanceState());
                bundle.PutInt("max", m_Max);
                bundle.PutInt("progress", m_Progress);

                return bundle;
            }

            return base.OnSaveInstanceState();
        }

        public override void OnRestoreInstanceState(IParcelable state)
        {
            var bundle = state as Bundle;
            if (bundle != null)
            {
                m_Max = bundle.GetInt("max", 100);
                m_Progress = bundle.GetInt("progress", 0);
                base.OnRestoreInstanceState(bundle.GetParcelable("instanceState") as IParcelable);
                return;
            }

            base.OnRestoreInstanceState(state);
        }

    }
}