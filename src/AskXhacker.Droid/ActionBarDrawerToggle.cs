using System;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace AskXhacker
{
	public class ActionBarDrawerToggle : Android.Support.V7.App.ActionBarDrawerToggle
	{
		public ActionBarDrawerToggle (AppCompatActivity host, DrawerLayout drawerLayout, int openedResource, int closedResource)
			: base (host, drawerLayout, openedResource, closedResource)
		{
		}

		public override void OnDrawerOpened (Android.Views.View drawerView)
		{
			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0) {
				base.OnDrawerOpened (drawerView);
			}
		}

		public override void OnDrawerClosed (Android.Views.View drawerView)
		{
			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0) {
				base.OnDrawerClosed (drawerView);
			}
		}

		public override void OnDrawerSlide (Android.Views.View drawerView, float slideOffset)
		{
			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0) {
				base.OnDrawerSlide (drawerView, slideOffset);
			}
		}

	}
}

