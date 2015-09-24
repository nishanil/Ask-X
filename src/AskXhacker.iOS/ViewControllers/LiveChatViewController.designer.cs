// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AskXhacker.iOS
{
	[Register ("LiveChatViewController")]
	partial class LiveChatViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIWebView LiveChatView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LiveChatView != null) {
				LiveChatView.Dispose ();
				LiveChatView = null;
			}
		}
	}
}
