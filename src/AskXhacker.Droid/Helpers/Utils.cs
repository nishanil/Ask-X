using System;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace AskXhacker
{
	public static class Utils
	{

		public static Bitmap DrawableToBitmap(this Drawable drawable)
		{
			if (drawable is BitmapDrawable)
			{
				return ((BitmapDrawable)drawable).Bitmap;
			}

			if (drawable.IntrinsicHeight == -1 || drawable.IntrinsicHeight == -1)
				return null;

			Bitmap bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(bitmap);
			drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
			drawable.Draw(canvas);

			return bitmap;
		}

		public static string ToMomentString(this DateTime value)
		{
			var localTime = value.ToLocalTime();
			var dt = DateTime.Now - localTime;

			switch (dt.Days)
			{
			case 0:
				return localTime.ToString("t");

			case 1:
				return "YESTERDAY";

			default:
				return localTime.ToString("MMM dd");
			}
		}
	}
}

