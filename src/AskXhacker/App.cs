using System;
using Refractored.Xam.Settings.Abstractions;
using Refractored.Xam.Settings;
using System.Collections.Generic;

// Refer: http://motzcod.es/post/102974754207/an-improved-cross-platform-settings-library-for 
// https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings
namespace AskXhacker
{
	
	public static class App
	{
		//TODO: Add your settings
		public const string InsightsKey = "";
		public const string ReconfigurationServiceUrl = "";
		public const string DialogUrl = "";
		static readonly Dictionary<string, object> cachedValues = new Dictionary<string, object> ();
		static DataManager dataManager;
		static ISettings AppSettings {
			get {
				return CrossSettings.Current;
			}
		}

		public static DataManager DataManager {
			get {
				return dataManager;
			}
		}

		public static void Init()
		{
			dataManager = new DataManager ();
		}

		const string FirstRunKey = "firstrun_key";
		static readonly bool FirstRunDefault = true;

		public static bool FirstRun {
			get {
				return GetValueOrDefault (FirstRunKey, FirstRunDefault);
			}
			set {
				AddOrUpdateValue (FirstRunKey, value);
			}
		}

		const string ConversationIdKey = "conversation_Id_key";
		private static readonly int ConversationIdDefault = 0;

		public static int ConversationId {
			get {
				return GetValueOrDefault (ConversationIdKey, ConversationIdDefault);
			}
			set {
				AddOrUpdateValue (ConversationIdKey, value);
			}
		}

		const string ClientIdKey = "client_Id_key";
		private static readonly int ClientIdDefault = 0;
		public static int ClientId {
			get {
				return GetValueOrDefault (ClientIdKey, ClientIdDefault);
			}
			set {
				AddOrUpdateValue (ClientIdKey, value);
			}
		}

		const string UserNameKey = "username_key";
		private static readonly string UserNameDefault = string.Empty;
		public static string UserName {
			get {
				return GetValueOrDefault (UserNameKey, UserNameDefault);
			}
			set {
				AddOrUpdateValue (UserNameKey, value);
			}
		}

		const string DialogIdKey = "dialogId_key";
		private static readonly string DialogIdKeyDefault = string.Empty;
		public static string DialogId {
			get {
				return GetValueOrDefault (DialogIdKey, DialogIdKeyDefault);
			}
			set {
				AddOrUpdateValue (DialogIdKey, value);
			}
		}

		const string DialogUserIdKey = "dialog_UserId_key";
		private static readonly string DialogUserIdKeyDefault = string.Empty;
		public static string DialogUserId {
			get {
				return GetValueOrDefault (DialogUserIdKey, DialogUserIdKeyDefault);
			}
			set {
				AddOrUpdateValue (DialogUserIdKey, value);
			}
		}

		const string DialogPasswordKey = "dialog_Password_key";
		private static readonly string DialogPasswordKeyDefault = string.Empty;
		public static string DialogPassword {
			get {
				return GetValueOrDefault (DialogPasswordKey, DialogPasswordKeyDefault);
			}
			set {
				AddOrUpdateValue (DialogPasswordKey, value);
			}
		}
			
		private static bool AddOrUpdateValue<T> (string key, T value)
		{
			lock (cachedValues) {
				cachedValues [key] = value;
				return AppSettings.AddOrUpdateValue (key, value);
			}
		}

		private static T GetValueOrDefault<T> (string key, T defaultValue = default(T))
		{
			lock (cachedValues) {
				object valueFromCache;
				if (cachedValues.TryGetValue (key, out valueFromCache)) {
					return (T)valueFromCache;
				}
				T value = AppSettings.GetValueOrDefault (key, defaultValue);
				cachedValues [key] = value;
				return value;
			}
		}
	}
}



