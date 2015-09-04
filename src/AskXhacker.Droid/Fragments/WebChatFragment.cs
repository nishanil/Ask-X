
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
using Android.Webkit;

namespace AskXhacker
{
	public class WebChatFragment : Android.Support.V4.App.Fragment
	{

		public static WebChatFragment NewInstance ()
		{
			var instance = new WebChatFragment ();

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
			
			var rootView = inflater.Inflate (Resource.Layout.fragment_web_view, container, false);

			var webview = rootView.FindViewById<WebView> (Resource.Id.localWebView);
			webview.SetWebViewClient (new WebChatClient (Activity));
			webview.Settings.JavaScriptEnabled = true;
			webview.Settings.DomStorageEnabled = true;
			webview.LoadUrl ("https://scrollback.io/xhackers/all");
			return rootView;

		}

		public override void OnResume ()
		{
			base.OnResume ();
			((MainActivity)Activity).SupportActionBar.Title = "Live Chat";
		}

	}

	public class WebChatClient : WebViewClient
	{
		Activity activity;

		public WebChatClient (Activity activity)
		{
			this.activity = activity;
		}

		public override void OnPageStarted (WebView view, string url, Android.Graphics.Bitmap favicon)
		{
			AndroidHUD.AndHUD.Shared.Show (activity, "Please wait", maskType: AndroidHUD.MaskType.Clear);

		}

		public override void OnPageFinished (WebView view, string url)
		{
			AndroidHUD.AndHUD.Shared.Dismiss (activity);

		}
	}
}

