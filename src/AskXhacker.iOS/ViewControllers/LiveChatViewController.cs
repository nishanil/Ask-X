using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using FlyoutNavigation;
using CoreGraphics;

namespace AskXhacker.iOS
{
	partial class LiveChatViewController : UIViewController
	{
		public LiveChatViewController (IntPtr handle) : base (handle)
		{
		}

		public FlyoutNavigationController Navigation {
			get;
			set;
		}
		public LiveChatViewController(FlyoutNavigationController navigation)
		{
			this.Navigation = navigation;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Live Chat";

			var menuButtonItem = new UIBarButtonItem (UIImage.FromBundle ("Menu"), UIBarButtonItemStyle.Plain, delegate {
				Navigation.ToggleMenu ();
			});

			menuButtonItem.TintColor = Theme.PrimaryColor;
			NavigationItem.LeftBarButtonItem = menuButtonItem;
			NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes (){ 
				Font = UIFont.PreferredHeadline,
				ForegroundColor = Theme.PrimaryColor
			};
			var spinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			spinner.Frame = new CGRect(0, 0, 25, 25);
			spinner.Tag = 1;
			spinner.Color = Theme.PrimaryColor;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (spinner);

			LiveChatView.LoadStarted += (object sender, EventArgs e) => spinner.StartAnimating ();
			LiveChatView.LoadFinished += (object sender, EventArgs e) => spinner.StopAnimating ();

			LiveChatView.LoadRequest (new NSUrlRequest(new NSUrl("https://scrollback.io/xhackers/all")));
		}
	}
}
