
using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;

namespace AskXhacker
{
	public class SettingsFragment : Android.Support.V4.App.Fragment
	{
		public static SettingsFragment NewInstance ()
		{
			var instance = new SettingsFragment ();
			var b = new Bundle ();
			instance.Arguments = b;
			return instance;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		EditText userName, userId, dialogId, serverUrl;

		void SetValues ()
		{
			userName.Text = App.UserName;
			userId.Text = App.DialogUserId;
			dialogId.Text = App.DialogId;
			serverUrl.Text = string.Format(App.DialogUrl, App.DialogId);

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var root = inflater.Inflate (Resource.Layout.fragment_settings, container, false);
			var button = root.FindViewById<Button> (Resource.Id.reconfigureButton);
			userName = root.FindViewById<EditText> (Resource.Id.edit_text_settings_name);

			userId = root.FindViewById<EditText> (Resource.Id.edit_text_settings_userId);
			dialogId = root.FindViewById<EditText> (Resource.Id.edit_text_settings_dialogId);
			serverUrl = root.FindViewById<EditText> (Resource.Id.edit_text_settings_server);

			SetValues ();
			HasOptionsMenu = true;

			button.Click += async (object sender, EventArgs e) => {
				BluemixDialogService bluemixService;
				bool isSuccess = false;
				try {
					AndroidHUD.AndHUD.Shared.Show (Activity, "Reconfiguring");
					bluemixService = new BluemixDialogService ();
					await bluemixService.ReConfigureService ();
					isSuccess = true;
				} catch (Exception ex) {
					bluemixService = new BluemixDialogService ();
					bluemixService.ConfigureDefaultService();
					System.Diagnostics.Debug.WriteLine ("Ask X: Exception Occurred: " + ex.Message);
					Xamarin.Insights.Report (ex);
				} finally {
					SetValues ();
					AndroidHUD.AndHUD.Shared.Dismiss ();
					if (isSuccess)
						Snackbar.Make (View, "Configuration updated successfully!", Snackbar.LengthLong).Show ();
					else
						Snackbar.Make (View, "Oops! Configuration failed. Set to default.", Snackbar.LengthLong).Show ();

				}
			};

			return root;
		}

		public override void OnResume ()
		{
			base.OnResume ();
			((MainActivity)Activity).SupportActionBar.Title = "Settings";

		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.menu_actions, menu);
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Resource.Id.action_save && userName != null) {
				App.UserName = userName.Text;
				((MainActivity)Activity).SetName ();
				Snackbar.Make (View, App.UserName + " Saved!", Snackbar.LengthLong).Show ();
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}

