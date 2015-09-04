using System;

namespace AskXhacker
{
	public enum MessageStatus
	{
		Sending,
		Sent,
		Delivered,
		Seen
	}

	public class Message : EntityBase
	{
		private MessageStatus status;

		public MessageStatus Status {
			get{ return status;}
			set { status = value; RaisePropertyChanged (); }
		}

		private bool isIncoming;
		public bool IsIncoming {
			get{ return isIncoming; }
			set{ isIncoming = value; RaisePropertyChanged (); }
		}

		private string text;
		public string Text {
			get{ return text; }
			set{ text = value; RaisePropertyChanged (); }
		}

		private DateTime timestamp;
		public DateTime Timestamp {
			get{ return timestamp; }
			set{ timestamp = value; RaisePropertyChanged (); }
		}


	}
}

