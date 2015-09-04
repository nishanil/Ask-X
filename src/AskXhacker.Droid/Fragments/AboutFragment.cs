
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Reflection;
using Android.Text.Method;

namespace AskXhacker
{
	public class AboutFragment : Android.Support.V4.App.Fragment
	{

		public static AboutFragment NewInstance ()
		{
			var instance = new AboutFragment ();
			var b = new Bundle ();
			instance.Arguments = b;
			return instance;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			var root = inflater.Inflate (Resource.Layout.fragment_about, container, false);
			var version = root.FindViewById<TextView> (Resource.Id.about_version);
			var nish = root.FindViewById<TextView> (Resource.Id.about_nish_twitter);
			nish.MovementMethod = LinkMovementMethod.Instance;

			var ajay = root.FindViewById<TextView> (Resource.Id.about_ajay_twitter);
			ajay.MovementMethod = LinkMovementMethod.Instance;

			var xhackers = root.FindViewById<TextView> (Resource.Id.about_xhackers_desc);
			xhackers.MovementMethod = LinkMovementMethod.Instance;


			version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			return root;
		}

		public override void OnResume ()
		{
			base.OnResume ();
			((MainActivity)Activity).SupportActionBar.Title = "About";
		}
	}
}

