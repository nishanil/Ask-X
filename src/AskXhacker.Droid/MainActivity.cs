using System;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Content;

namespace AskXhacker
{
	[Activity (Label = "Ask X", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
	public class MainActivity : AppCompatActivity
	{
		public Toolbar Toolbar {
			get;
			set;
		}

		public static bool FirstRun = true;

		private ActionBarDrawerToggle actionBarToggle;
		private DrawerLayout drawerLayout;

		void DoFirstRun (Android.OS.Bundle savedInstanceState)
		{
			if (App.FirstRun) {
				ShowDialog ();
			}
			else
				if (savedInstanceState == null) {
					SetName ();
					SetFragment (MessagesFragment.NewInstance ());
				}
		}

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
			Toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			if (Toolbar != null) {
				SetSupportActionBar (Toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetHomeButtonEnabled (true);
			}

			var navigationDrawer = this.FindViewById<NavigationView> (Resource.Id.nav_view);
			navigationDrawer.NavigationItemSelected += Nv_NavigationItemSelected;

			// animating menu
			actionBarToggle = new ActionBarDrawerToggle (
				this, 
				drawerLayout, 
				Resource.String.openDrawer,
				Resource.String.closeDrawer
			);



			drawerLayout.SetDrawerListener (actionBarToggle);
			actionBarToggle.SyncState ();
			DoFirstRun (savedInstanceState);
		}

		public void ShowDialog()
		{
			var dialogFragment = NameInputFragment.NewInstance ("Watson wants to know your name!");

			dialogFragment.Show (SupportFragmentManager, "dialog");
			dialogFragment.NameEntered += NameEntered;
		}

		public void SetName()
		{
			var nameHeader = FindViewById<TextView> (Resource.Id.greetingText);
			nameHeader.Text = string.Format ("Hello, {0}!", App.UserName);
		}

		private void NameEntered(object sender, DialogClickEventArgs e)
		{
			SetName ();
			SetFragment (MessagesFragment.NewInstance ());
		}

		void Nv_NavigationItemSelected (object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			switch (e.MenuItem.ItemId) {
			case Resource.Id.nav_first_fragment:
				SetFragment (MessagesFragment.NewInstance ());
				break;
			case Resource.Id.nav_second_fragment:
				SetFragment (WebChatFragment.NewInstance ());
				break;
			case Resource.Id.nav_third_fragment:
				SetFragment (SettingsFragment.NewInstance ());
				break;
			case Resource.Id.nav_fourth_fragment:
				SetFragment (AboutFragment.NewInstance ());
				break;

			}
			e.MenuItem.SetChecked (true);
		}

		private void SetFragment (Android.Support.V4.App.Fragment fragment)
		{
			SupportFragmentManager.BeginTransaction ().Replace (Resource.Id.flContent, fragment).Commit (); 
			drawerLayout.CloseDrawers ();
			actionBarToggle.SyncState ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			// handles home button
			if (actionBarToggle.OnOptionsItemSelected (item))
				return true;
			
			return base.OnOptionsItemSelected (item);
		}
			

		protected override void OnPostCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			actionBarToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			actionBarToggle.OnConfigurationChanged (newConfig);
		}

	}
}

