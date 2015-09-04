using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AskXhacker
{
	public class MessageViewModel : ViewModelBase
	{
		ObservableCollection<Message> messages;

		readonly IBluemixDialogService dialogService;

		public ObservableCollection<Message> Messages {
			get { return messages; }
			set {
				messages = value;
				RaisePropertyChanged ();
			}
		}


		public MessageViewModel ()
		{
			Title = "Watson Chat";
			dialogService = new BluemixDialogService ();
			if (App.FirstRun) {
				dialogService.ConfigureDefaultService ();
			}
			Messages = new ObservableCollection<Message> ();
			App.DataManager.GetAllMessages ().ContinueWith (m => {
				var msgs = m.Result;
				foreach (var item in msgs) {
					Messages.Add (item);
				}
			});
		}


		public async Task SayHello ()
		{
			if (IsBusy)
				return;
			try {
				IsBusy = true;
				// send an empty message to start a conversation
				// with watson
				await SendMessage ();
			} catch (Exception ex) {
				Debug.WriteLine ("Ask X: Exception occurred: " + ex);
			}
			finally{
				IsBusy = false;
				App.FirstRun = false;
			}
		}

		public async Task SendMessage (string message = null)
		{
			Message messageItem;
			if (!string.IsNullOrEmpty (message)) {
				messageItem = new Message {
					IsIncoming = false,
					Text = message,
					Status = MessageStatus.Sent,
					Timestamp = DateTime.UtcNow,
				};
				await App.DataManager.SaveMessage (messageItem);
				Messages.Add (messageItem);

			} else
				messageItem = new Message{ Text = "Hello!" };

			var incomingMessage = await dialogService.SendMessage (messageItem);
			await App.DataManager.SaveMessage (incomingMessage);
			messageItem.Status = MessageStatus.Delivered;
			Messages.Add (incomingMessage);

		}
	}

}

