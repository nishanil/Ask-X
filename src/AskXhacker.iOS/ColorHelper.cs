using System;
using UIKit;

namespace AskXhacker.iOS
{
	public static class ColorHelper
	{
		public static UIColor FromHex(int color, float? alpha=null)
		{
			byte[] rgb = BitConverter.GetBytes (color);

			var rgbColor = UIColor.FromRGB (rgb [2], rgb [1], rgb [0]);
			if (alpha == null)
				return rgbColor;
			else
				return rgbColor.ColorWithAlpha (alpha.Value);
		}

	}
}

