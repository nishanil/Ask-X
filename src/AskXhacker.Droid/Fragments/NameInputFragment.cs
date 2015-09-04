
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace AskXhacker
{
	public class NameInputFragment : Android.Support.V4.App.DialogFragment
	{

		public static NameInputFragment NewInstance (string title)
		{
			var instance = new NameInputFragment ();

			var b = new Bundle ();
			b.PutString("title", title);

			instance.Arguments = b;

			return instance;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		public  EventHandler<DialogClickEventArgs> NameEntered;

		public override Android.App.Dialog OnCreateDialog (Bundle savedInstanceState)
		{
			string title = Arguments.GetString ("title");

			var builder = new AlertDialog.Builder (Activity);
			var dialogView = Activity.LayoutInflater.Inflate (Resource.Layout.fragment_name_dialog, null);
			var userText = dialogView.FindViewById<EditText>(Resource.Id.edit_text_name);

			var dialog = builder.SetView (dialogView).SetPositiveButton ("Save", (s, e) => {

				if (userText != null)
					App.UserName = userText.Text;

				if (NameEntered != null)
					NameEntered (this, new DialogClickEventArgs (e.Which));

			}).SetTitle (title).SetCancelable (false).Create ();

			dialog.SetCanceledOnTouchOutside (false);

			return  dialog;
		}



	}
}

