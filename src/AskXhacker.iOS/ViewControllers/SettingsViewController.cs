using System;
using MonoTouch.Dialog;
using FlyoutNavigation;
using UIKit;
using CoreGraphics;
using System.Threading.Tasks;

namespace AskXhacker.iOS
{
	
	public class SettingsViewController : DialogViewController
	{

		public FlyoutNavigationController Navigation {
			get;
			set;
		}

		UIActivityIndicatorView spinner;
		StringElement url, dialogId, userId;
		EntryElement nameElement;
		public SettingsViewController (FlyoutNavigationController navigation) : base(new RootElement("Settings"))
		{
			Navigation = navigation;
			nameElement = new EntryElement ("Name", null, null);
			nameElement.EntryEnded += (object sender, EventArgs e) => { 
				App.UserName = nameElement.Value;
				HomeNavigationController.UpdateName();
			};

			url = new StringElement ("Url");
			dialogId = new StringElement ("Dialog Id");
			userId = new StringElement ("User Id");


			Root.Add (new Section ("USER INFO"){ nameElement });
			Root.Add(
				new Section ("WATSON CONFIGURATION") { 
					url,
					dialogId,
					userId,
				});
			

			var reconfigureElement = new StringElement ("RECONFIGURE", async () => {
				spinner.StartAnimating ();
				BluemixDialogService bluemixService;
				UIAlertController alert;
				bool isSuccess = false;
				bluemixService = new BluemixDialogService ();

				try {
					await bluemixService.ReConfigureService ();
					isSuccess = true;
				} catch (Exception ex) {
					bluemixService.ConfigureDefaultService();
					System.Diagnostics.Debug.WriteLine ("Ask X: Exception Occurred: " + ex.Message);
					Xamarin.Insights.Report (ex);
				} finally {
					SetValues (true);
					spinner.StopAnimating ();
					if (isSuccess)
						alert = UIAlertController.Create("Watson Configuration", "Updated successfully!", UIAlertControllerStyle.Alert);
					else
						alert = UIAlertController.Create("Watson Configuration", "Oops! Configuration failed. Set to default.", UIAlertControllerStyle.Alert);
					alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, null));
					PresentViewControllerAsync(alert, true);
				}
			});

			reconfigureElement.Alignment = UITextAlignment.Center;
			Root.Add (new Section(null, "New configuration will be updated from the server."){reconfigureElement});

		}

		private void SetValues(bool reloadData=false)
		{
			url.Value = string.Format (App.DialogUrl, App.DialogId);
			dialogId.Value = App.DialogId;
			userId.Value = App.DialogUserId;
			nameElement.Value = App.UserName;
			if(reloadData)
				this.ReloadData ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var menuButtonItem = new UIBarButtonItem (UIImage.FromBundle ("Menu"), UIBarButtonItemStyle.Plain, delegate {
				Navigation.ToggleMenu ();
			});

			menuButtonItem.TintColor = Theme.PrimaryColor;
			NavigationItem.LeftBarButtonItem = menuButtonItem;
			NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes (){ 
				Font = UIFont.PreferredHeadline,
				ForegroundColor = Theme.PrimaryColor
			};

			spinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			spinner.Frame = new CGRect(0, 0, 25, 25);
			spinner.Tag = 1;
			spinner.Color = Theme.PrimaryColor;
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (spinner);


		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			SetValues (true);
		}
	}
}
	

