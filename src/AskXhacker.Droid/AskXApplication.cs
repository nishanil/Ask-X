using System;
using Android.App;
using System.IO;
using Xamarin;

namespace AskXhacker
{
	#if DEBUG
	[Application(Debuggable=true)]
	#else
	[Application(Debuggable=false)]
	#endif
	public class AskXApplication: Application
	{

		public AskXApplication(IntPtr handle, global::Android.Runtime.JniHandleOwnership transer)
			:base(handle, transer)
		{

		}

		public override void OnCreate()
		{
			base.OnCreate();

			var dbLocation = "askxhacker.db3";

			Insights.Initialize(App.InsightsKey, this);

			var library = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dbLocation = Path.Combine(library, dbLocation);
			AskXDatabase.DatabaseLocation = dbLocation;
			App.Init ();
		}
	}
}

