

using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace MonoDroidToolkit
{
    public class ProgressButton : CompoundButton
    {
        private int max = 100;
        /// <summary>
        /// Gets or sets the maximum progress, Defaults to 100
        /// </summary>
        public int Max
        {
            get { return max; }
            set
            {
                if (value < 0)
                    max = 0;
                else
                    max = value; 

                Invalidate();
            }
        }

        private int progress = 0;
        /// <summary>
        /// Gets or sets the current progress, Defaults to 0.
        /// </summary>
        public int Progress
        {
            get { return progress; }
            set
            {
                if (value > Max)
                    progress = Max;
                else if (value < 0)
                    progress = 0;
                else
                    progress = value;

                Invalidate();
            }
        }

        private int indeterminateInterval = 100;
        public int IndeterminanteInterval
        {
            get { return indeterminateInterval; }
            set 
            { 
                indeterminateInterval = value;
                if (indeterminateInterval < 50)
                    indeterminateInterval = 50;

            }
        }

        private bool indeterminate = false;
        /// <summary>
        /// Get or set if the Spinner is indeterminante
        /// </summary>
        public bool Indeterminante
        {
            get { return indeterminate; }
            set
            {
                
                indeterminate = value;
                if (indeterminate)
                {
                    //start
                    UpdateIndeterminante();
                }
                else
                {
                    //stop
                    if (indeterminanteHandler == null)
                        return;

                    indeterminanteHandler.RemoveCallbacks(IndeterminanteRunnable);
                }
            }
        }

        Handler indeterminanteHandler = new Handler();
        private void UpdateIndeterminante()
        {
            if (indeterminanteHandler == null)
                return;

            indeterminanteHandler.RemoveCallbacks(IndeterminanteRunnable);
            indeterminanteHandler.PostDelayed(IndeterminanteRunnable, indeterminateInterval);
        }


        private void IndeterminanteRunnable()
        {
            var newProgress = Progress + 1;
            if (newProgress > Max)
                newProgress = 0;

            Progress = newProgress;

            if(Indeterminante)
                indeterminanteHandler.PostDelayed(IndeterminanteRunnable, indeterminateInterval);
        }



        private Drawable shadowDrawable;
        /// <summary>
        /// Gets or sets the drawable used as the shadow
        /// </summary>
        public Drawable ShadowDrawable
        {
            get { return shadowDrawable; }
            set 
            { 
                shadowDrawable = value;
                drawableSize = shadowDrawable.IntrinsicWidth; 
                Invalidate();
            }
        }

        private Drawable unpinnedDrawable;
        /// <summary>
        /// Gets or sets the drawable displayed when the user unpins an item.
        /// </summary>
        public Drawable UnpinnedDrawable
        {
            get { return unpinnedDrawable; }
            set { unpinnedDrawable = value; Invalidate(); }
        }

        private Drawable pinnedDrawable;
        /// <summary>
        /// Gets or sets the drawable displayed when the user pins an item.
        /// </summary>
        public Drawable PinnedDrawable {
            get { return pinnedDrawable; }
            set { pinnedDrawable = value; Invalidate(); }
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
        private readonly Rect tempRect = new Rect();
        private readonly RectF tempRectF = new RectF();
        private int drawableSize;
        private int innerSize;

        /// <summary>
        /// Gets or sets the inner isze
        /// </summary>
        public int InnerSize
        {
            get { return innerSize; }
            set 
            { 
                innerSize = value;
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
            var indeterminate = false;
            var indeterminateInterval = Resources.GetInteger(Resource.Integer.progressbutton_indeterminent_interval);

            var canChecked = false;
            var canClickable = false;
            var canFocusable = false;
            innerSize = Resources.GetDimensionPixelSize(Resource.Dimension.progress_inner_size);


            if (context != null && attrs != null)
            {
                var a = context.ObtainStyledAttributes(attrs,
                                                       Resource.Styleable.ProgressButton,
                                                       Resource.Attribute.progressButtonStyle,
                                                       Resource.Style.ProgressButton_Pin);

                progress = a.GetInteger(Resource.Styleable.ProgressButton_progress, 0);
                max = a.GetInteger(Resource.Styleable.ProgressButton_max, 100);

                circleColor = a.GetColor(Resource.Styleable.ProgressButton_circleColor, circleColor);
                progressColor = a.GetColor(Resource.Styleable.ProgressButton_progressColor, progressColor);
                pinnedDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_pinnedDrawable, pinnedDrawable);
                unpinnedDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_unpinnedDrawable, unpinnedDrawable);
                shadowDrawable = a.GetResourceId(Resource.Styleable.ProgressButton_shadowDrawable, shadowDrawable);

                innerSize = a.GetDimensionPixelSize(Resource.Styleable.ProgressButton_innerSize, 0);
                if(innerSize == 0)
                    innerSize = Resources.GetDimensionPixelSize(Resource.Dimension.progress_inner_size);

                canChecked = a.GetBoolean(Resource.Styleable.ProgressButton_pinned, canChecked);
                canClickable = a.GetBoolean(Resource.Styleable.ProgressButton_android_clickable, canClickable);
                canFocusable = a.GetBoolean(Resource.Styleable.ProgressButton_android_focusable, canFocusable);

                SetBackgroundDrawable(a.GetDrawable(Resource.Styleable.ProgressButton_android_selectableItemBackground));
                indeterminateInterval = a.GetInteger(Resource.Styleable.ProgressButton_indeterminate_interval, indeterminateInterval);
                indeterminate = a.GetBoolean(Resource.Styleable.ProgressButton_indeterminate, indeterminate);

                a.Recycle();
            }

            this.pinnedDrawable = base.Resources.GetDrawable(pinnedDrawable);
            this.pinnedDrawable.SetCallback(this);

            this.unpinnedDrawable = base.Resources.GetDrawable(unpinnedDrawable);
            this.unpinnedDrawable.SetCallback(this);

            this.shadowDrawable = base.Resources.GetDrawable(shadowDrawable);
            this.shadowDrawable.SetCallback(this);

			drawableSize = this.shadowDrawable.IntrinsicWidth;

            Checked = canChecked;
            Clickable = canClickable;
            Focusable = canFocusable;

            CirclePaint = new Paint {Color = circleColor, AntiAlias = true};
            ProgressPaint = new Paint {Color = progressColor, AntiAlias = true};

            this.indeterminateInterval = indeterminateInterval;
            Indeterminante = indeterminate;
        }


        public bool Pinned
        {
            get { return Checked; }
            set { Checked = value; }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            SetMeasuredDimension(ResolveSize(drawableSize, widthMeasureSpec), 
                ResolveSize(drawableSize, heightMeasureSpec));
        }

        protected override void DrawableStateChanged()
        {
            base.DrawableStateChanged();

            if (pinnedDrawable.IsStateful)
            {
                pinnedDrawable.SetState(GetDrawableState());
            }

            if (unpinnedDrawable.IsStateful)
            {
                unpinnedDrawable.SetState(GetDrawableState());
            }

            if (shadowDrawable.IsStateful)
            {
                shadowDrawable.SetState(GetDrawableState());
            }

            Invalidate();
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            tempRect.Set(0, 0, drawableSize, drawableSize);
            tempRect.Offset((Width - drawableSize) / 2, (Height - drawableSize) / 2);

            tempRectF.Set(-0.5f, -0.5f, innerSize + 0.5f, innerSize + 0.5f);
            tempRectF.Offset((Width - innerSize) / 2, (Height - innerSize) / 2);

            canvas.DrawArc(tempRectF, 0, 360, true, CirclePaint);
            canvas.DrawArc(tempRectF,-90, 360 * Progress / Max, true, ProgressPaint);

            var iconDrawable = Checked ? PinnedDrawable : UnpinnedDrawable;
            iconDrawable.Bounds = tempRect;
            iconDrawable.Draw(canvas);

            shadowDrawable.Bounds = tempRect;
            shadowDrawable.Draw(canvas);

        }

        public override IParcelable OnSaveInstanceState()
        {
            if (SaveEnabled)
            {
                var bundle = new Bundle();
                bundle.PutParcelable("instanceState", base.OnSaveInstanceState());
                bundle.PutInt("max", max);
                bundle.PutInt("progress", progress);

                return bundle;
            }

            return base.OnSaveInstanceState();
        }

        public override void OnRestoreInstanceState(IParcelable state)
        {
            var bundle = state as Bundle;
            if (bundle != null)
            {
                max = bundle.GetInt("max", 100);
                progress = bundle.GetInt("progress", 0);
                base.OnRestoreInstanceState(bundle.GetParcelable("instanceState") as IParcelable);
                return;
            }

            base.OnRestoreInstanceState(state);
        }

    }
}