using System;
using UIKit;

namespace AskXhacker.iOS
{
	public static class Theme
	{

		public static UIColor PrimaryColor {
			get{ return ColorHelper.FromHex (0x9C27B0); }
		}

		public static UIColor AccentColor {
			get{ return ColorHelper.FromHex (0x009688); }
		}

		public static UIColor LightPrimaryColor {
			get{ return ColorHelper.FromHex (0xE1BEE7); }
		}

		public static UIColor DarkPrimaryColor {
			get{ return ColorHelper.FromHex (0x7B1FA2); }
		}

	}
}

