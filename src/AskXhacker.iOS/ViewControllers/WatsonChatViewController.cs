// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using FlyoutNavigation;
using MonoTouch.Dialog;
using JSQMessagesViewController;
using CoreGraphics;

namespace AskXhacker.iOS
{
	public partial class WatsonChatViewController : MessagesViewController
	{
		public readonly MessageViewModel ViewModel = new MessageViewModel ();

		public WatsonChatViewController (IntPtr handle) : base (handle)
		{
		}

		MessagesBubbleImage outgoingBubbleImageData, incomingBubbleImageData;
		FlyoutNavigationController navigation;
		public WatsonChatViewController(FlyoutNavigationController navigation)
		{
			this.navigation = navigation;

//			UIApplication.Notifications.ObserveContentSizeCategoryChanged (delegate {
//			
//				CollectionView.CollectionViewLayout.MessageBubbleFont = UIFont.PreferredBody;
//				CollectionView.UpdateConstraints();
//			});
		}
		UIActivityIndicatorView spinner;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			var menuButtonItem = new UIBarButtonItem (UIImage.FromBundle ("Menu"), UIBarButtonItemStyle.Plain, delegate {
				navigation.ToggleMenu ();
			});
			Title = "Watson Chat";

			menuButtonItem.TintColor = Theme.PrimaryColor;
			NavigationItem.LeftBarButtonItem = menuButtonItem;
			NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes (){ 
				Font = UIFont.PreferredHeadline,
				ForegroundColor = Theme.PrimaryColor
			};

			//TODO: App.UserName
			SenderId = "user";
			SenderDisplayName = "user";

			var bubbleFactory = new MessagesBubbleImageFactory ();
			outgoingBubbleImageData = bubbleFactory.CreateOutgoingMessagesBubbleImage (Theme.PrimaryColor);
			incomingBubbleImageData = bubbleFactory.CreateIncomingMessagesBubbleImage (UIColorExtensions.MessageBubbleLightGrayColor);

			CollectionView.CollectionViewLayout.MessageBubbleFont = UIFont.PreferredBody;

			this.InputToolbar.ContentView.LeftBarButtonItem = null;
			this.InputToolbar.ContentView.TextView.PlaceHolder = "Message";
			this.InputToolbar.ContentView.RightBarButtonItem.SetTitleColor(Theme.PrimaryColor, UIControlState.Normal);
			this.InputToolbar.ContentView.RightBarButtonItem.SetTitleColor(Theme.DarkPrimaryColor, UIControlState.Highlighted);

			spinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			spinner.Frame = new CGRect(0, 0, 25, 25);
			spinner.Tag = 1;
			spinner.Color = Theme.PrimaryColor;
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (spinner);
		}


		public override void PressedSendButton (UIButton button, string text, string senderId, string senderDisplayName, NSDate date)
		{
			ViewModel.SendMessage (text);
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			CollectionView.CollectionViewLayout.SpringinessEnabled = true;

			ViewModel.PropertyChanged += ViewModel_PropertyChanged;
			ViewModel.Messages.CollectionChanged += ViewModel_Messages_CollectionChanged;

			if (App.FirstRun) {
				var textInputAlertController = UIAlertController.Create("Watson Chat", "What's your name?", UIAlertControllerStyle.Alert);
				textInputAlertController.AddTextField(textField => {
				});
				var okayAction = UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, alertAction => { 
					App.UserName = textInputAlertController.TextFields[0].Text;
					HomeNavigationController.UpdateName();
				});
				textInputAlertController.AddAction(okayAction);
				PresentViewControllerAsync(textInputAlertController, true);
				ViewModel.SayHello ();
				App.FirstRun = false;
			}

		}

		void ViewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			InvokeOnMainThread (()=>{
				switch (e.PropertyName) {
				case "IsBusy":
					if (ViewModel.IsBusy)
						spinner.StartAnimating();
					else
						spinner.StopAnimating();
					break;
				}
			});
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return ViewModel.Messages.Count;
		}

		void ViewModel_Messages_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
				foreach (Message newItem in e.NewItems) {
					if (newItem.IsIncoming) {
						SystemSoundPlayer.PlayMessageReceivedSound ();
						this.FinishReceivingMessage ();
					} else {
						SystemSoundPlayer.PlayMessageSentSound();
						this.FinishSendingMessage ();
					}
				}
			}
		}

		public override IMessageData GetMessageData (MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			var message = ViewModel.Messages [indexPath.Row];
			var senderId = (message.IsIncoming) ? "watson" : "user";
			return JSQMessagesViewController.Message.Create (senderId, senderId, message.Text);
		}

		public override IMessageBubbleImageDataSource GetMessageBubbleImageData (MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			var message = ViewModel.Messages [indexPath.Row];
			if(!message.IsIncoming)
				return outgoingBubbleImageData;
			return incomingBubbleImageData;
		}

		public override IMessageAvatarImageDataSource GetAvatarImageData (MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			var message = ViewModel.Messages [indexPath.Row];
			if (!message.IsIncoming)
				return MessagesAvatarImageFactory.CreateAvatarImage (UIImage.FromBundle ("default_user"), 30);
			else
				return MessagesAvatarImageFactory.CreateAvatarImage (UIImage.FromBundle ("default_watson"), 30);
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath) 
		{
			var cell =  base.GetCell (collectionView, indexPath) as MessagesCollectionViewCell;
			var message = ViewModel.Messages [indexPath.Row];
			if(!message.IsIncoming)
				cell.TextView.TextColor = UIColor.White;
			else
				cell.TextView.TextColor = UIColor.Black;
			return cell;
		}
			
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ViewModel.Messages.CollectionChanged -= ViewModel_Messages_CollectionChanged;
			ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
		}
	}
}
