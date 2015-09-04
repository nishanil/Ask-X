using System;
using Android.Widget;
using Android.Content;
using Object = Java.Lang.Object;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.Views;

namespace AskXhacker
{
	/// <summary>
	/// Messages adapter that handles Observable collection as a source
	/// </summary>
	public class ObservableMessagesAdapter : BaseAdapter
	{
		readonly Context context;
		readonly ObservableCollection<Message> messageItems;
		readonly List<JavaObjectWrapper<Message>> wrappedMessageItems;

		#region implemented abstract members of BaseAdapter

		public override Java.Lang.Object GetItem (int position)
		{
			return wrappedMessageItems [position];
		}

		public override long GetItemId (int position)
		{
			return wrappedMessageItems[position].GetHashCode();
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var row = convertView;
			var messageItem = wrappedMessageItems[position].Value;
			var layoutResourceId = messageItem.IsIncoming ? Resource.Layout.item_incomging_msg : Resource.Layout.item_outgoing_msg;

			if (row == null || (int)row.Tag != layoutResourceId )
			{
				var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
				row = inflater.Inflate(layoutResourceId, parent, false);
			}

			var msgText = row.FindViewById<TextView>(Resource.Id.msg_text);
			msgText.Text = messageItem.Text;

			var msgDate = row.FindViewById<TextView>(Resource.Id.msg_timestamp);
			msgDate.Text = messageItem.Timestamp.ToMomentString();

			var msgImage = row.FindViewById<ImageView> (Resource.Id.msg_photo);

			if (!messageItem.IsIncoming) {
				var msgStatus = row.FindViewById<ImageView>(Resource.Id.msg_status);
				switch (messageItem.Status)
				{
				case MessageStatus.Sending:
					msgStatus.SetImageResource(Resource.Drawable.msg_status_sending);
					break;
				case MessageStatus.Sent:
					msgStatus.SetImageResource(Resource.Drawable.msg_status_sent);
					break;
				case MessageStatus.Delivered:
					msgStatus.SetImageResource(Resource.Drawable.msg_status_delivered);
					break;
				case MessageStatus.Seen:
					msgStatus.SetImageResource(Resource.Drawable.msg_status_seen);
					break;
				}

				msgImage.SetImageResource (Resource.Drawable.default_photo);

			}else
				msgImage.SetImageResource (Resource.Drawable.default_watson);
			

			return row;
		}

		public override int Count {
			get {
				return wrappedMessageItems.Count;
			}
		}

		#endregion

		public ObservableMessagesAdapter (Context context, ObservableCollection<Message> messageItems)
		{
			this.context = context;
			this.messageItems = messageItems;
			wrappedMessageItems = new List<JavaObjectWrapper<Message>> (messageItems.Count);
			foreach (var item in messageItems)
			{
				item.PropertyChanged += Item_PropertyChanged;;
				wrappedMessageItems.Add(new JavaObjectWrapper<Message>(item));
			}
			this.messageItems.CollectionChanged += MessageItems_CollectionChanged;;

		}
			
		void Item_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			
			NotifyDataSetChanged();
		}

		void MessageItems_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				var startingIndex = e.NewStartingIndex;
				foreach (Message newItem in e.NewItems)
				{
					var newWrapper = new JavaObjectWrapper<Message>(newItem);
					newItem.PropertyChanged += Item_PropertyChanged;
					if (startingIndex >= 0)
					{
						wrappedMessageItems.Insert(startingIndex, newWrapper);
						startingIndex++;
					}
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (Message oldItem in e.OldItems)
				{
					foreach (var item in wrappedMessageItems)
					{
						if (oldItem.Equals(item.Value))
						{
							wrappedMessageItems.Remove(item);
							item.Value.PropertyChanged -= Item_PropertyChanged;
						}
					}
				}
				break;
			case NotifyCollectionChangedAction.Replace:
			case NotifyCollectionChangedAction.Move:
				throw new NotImplementedException();
			case NotifyCollectionChangedAction.Reset:
				wrappedMessageItems.Clear();
				break;
			}
			NotifyDataSetChanged();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			messageItems.CollectionChanged -= MessageItems_CollectionChanged;
		}
	}

	public class JavaObjectWrapper<T> : Object
	{
		public T Value { get; set; }

		public JavaObjectWrapper(T value)
		{
			Value = value;
		}
	}
}

