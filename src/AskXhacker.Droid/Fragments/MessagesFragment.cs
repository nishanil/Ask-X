using System;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace AskXhacker
{
	public class MessagesFragment : Android.Support.V4.App.Fragment
	{
		public readonly MessageViewModel ViewModel = new MessageViewModel ();

		public static MessagesFragment NewInstance ()
		{
			var instance = new MessagesFragment ();
		
			var b = new Bundle ();
			instance.Arguments = b;

			return instance;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}


		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var root = inflater.Inflate (Resource.Layout.fragment_messages_list, container, false);
			var listView = root.FindViewById<ListView> (Resource.Id.messages_list_view);
			var sendButton = root.FindViewById<ImageButton> (Resource.Id.send_button);
			var messageText = root.FindViewById<EditText> (Resource.Id.input_text);
			var typingText = root.FindViewById<TextView> (Resource.Id.typing_text);
			listView.Adapter = new ObservableMessagesAdapter (Activity, ViewModel.Messages);
			sendButton.Click += (sender, e) => {
				if (!string.IsNullOrEmpty (messageText.Text)) {
					ViewModel.SendMessage (messageText.Text);
					messageText.Text = string.Empty;
				}
			};
			return root;
		}

		void ViewModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Activity.RunOnUiThread (() => {
				switch (e.PropertyName) {
				case "IsBusy":
					if (ViewModel.IsBusy)
						AndroidHUD.AndHUD.Shared.Show (Activity, "Please wait", maskType: AndroidHUD.MaskType.Clear);
					else
						AndroidHUD.AndHUD.Shared.Dismiss (Activity);
					break;
				}
			});
		}

		public override void OnResume ()
		{
			base.OnResume ();
			((MainActivity)Activity).SupportActionBar.Title = ViewModel.Title;
			ViewModel.PropertyChanged += ViewModelPropertyChanged;
			if (App.FirstRun) {
				ViewModel.SayHello ();
				App.FirstRun = false;
			}
		}

		public override void OnStop ()
		{
			base.OnStop ();
			ViewModel.PropertyChanged -= ViewModelPropertyChanged;
		}


	}
}

