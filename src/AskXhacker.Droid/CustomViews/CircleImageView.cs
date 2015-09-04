using System;
using Android.Widget;
using Android.Util;
using Android.Runtime;
using Android.Graphics.Drawables;
using Android.Content;

namespace AskXhacker
{
	public class CircleImageView : ImageView
	{
		public CircleImageView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		public CircleImageView(Context context)
			: base(context)
		{
		}
		protected CircleImageView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public override void SetImageDrawable(Drawable drawable)
		{
			var inputBitmap = drawable.DrawableToBitmap();
			var drawable2 = new CircleDrawable(inputBitmap);
			base.SetImageDrawable(drawable2);
		}

		public override void SetImageBitmap(Android.Graphics.Bitmap bm)
		{

			var drawable = new CircleDrawable(bm);
			base.SetImageDrawable(drawable);

		}

		public override void SetImageResource(int resId)
		{
			var inputBitmap = Resources.GetDrawable(resId).DrawableToBitmap();

			var drawable = new CircleDrawable(inputBitmap);
			base.SetImageDrawable(drawable);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension (MeasuredWidth, MeasuredWidth);
		}
	}
}

