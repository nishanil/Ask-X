using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;

namespace AskXhacker
{
	public class BluemixDialogService : IBluemixDialogService
	{
		public async Task<Message> SendMessage (Message message)
		{
			using (var client = new HttpClient ()) {

				client.DefaultRequestHeaders.Accept.Clear ();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Basic", GetBase64CredentialString ());

				var param = new List<KeyValuePair<string,string>> ();
				param.Add (new KeyValuePair<string, string> ("input", message.Text));
				if (App.ConversationId != 0) {
					param.Add (new KeyValuePair<string, string> ("conversation_id", App.ConversationId.ToString ()));
					param.Add (new KeyValuePair<string, string> ("client_id", App.ConversationId.ToString ()));
				}

				var data = await client.PostAsync (string.Format (App.DialogUrl, App.DialogId), new FormUrlEncodedContent (param)).ConfigureAwait (false);
				return await HandleResponse (data);
			}
		}

		public BluemixDialogService ()
		{
			
		}

		public void ConfigureDefaultService ()
		{
			//TODO: Add DialogService details
			App.FirstRun = true;
			App.DialogUserId = "";
			App.DialogPassword = "";
			App.DialogId = "";
		}

		static void HandleConfigureServiceResponse (string data)
		{
			var dialogConfig = JsonConvert.DeserializeObject<DialogConfiguration> (data);
			App.DialogId = dialogConfig.Dialog [0].Id;
			App.DialogUserId = dialogConfig.Dialog [0].Credentials.Username;
			App.DialogPassword = dialogConfig.Dialog [0].Credentials.Password;
			App.ConversationId = 0;
			App.ClientId = 0;
			new DataManager ().DropDatabase ().Wait ();
			App.Init ();
		}

		public async Task ReConfigureService ()
		{
			using (var client = new HttpClient ()) {
				
				var data = await client.GetStringAsync (App.ReconfigurationServiceUrl).ConfigureAwait (false);
				HandleConfigureServiceResponse (data);
			}
		}

		private async Task<Message> HandleResponse (HttpResponseMessage response)
		{
			var data = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
			return await Task.Run (() => {
				var watsonData = JsonConvert.DeserializeObject<WatsonResponse> (data);
				if (watsonData == null) {
					return null;
				}
				App.ClientId = watsonData.ClientId;
				App.ConversationId = watsonData.ConversationId;
				StringBuilder msgBuilder = new StringBuilder ();
				foreach (var item in watsonData.Response) {
					msgBuilder.AppendLine (item);
				}

				var message = new Message {
					Text = msgBuilder.ToString (),
					Timestamp = DateTime.UtcNow,
					IsIncoming = true,
					Status = MessageStatus.Delivered
				};


				return message;
			});

		}

		private string GetBase64CredentialString ()
		{
			var auth = string.Format ("{0}:{1}", App.DialogUserId, App.DialogPassword);
			return Convert.ToBase64String (Encoding.UTF8.GetBytes (auth));
		}
	}

	public class WatsonResponse
	{

		[JsonProperty ("conversation_id")]
		public int ConversationId { get; set; }

		[JsonProperty ("client_id")]
		public int ClientId { get; set; }

		[JsonProperty ("input")]
		public string Input { get; set; }

		[JsonProperty ("confidence")]
		public string Confidence { get; set; }

		[JsonProperty ("response")]
		public IList<string> Response { get; set; }
	}

	public class Credentials
	{

		[JsonProperty ("url")]
		public string Url { get; set; }

		[JsonProperty ("username")]
		public string Username { get; set; }

		[JsonProperty ("password")]
		public string Password { get; set; }
	}

	public class Dialog
	{

		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("label")]
		public string Label { get; set; }

		[JsonProperty ("plan")]
		public string Plan { get; set; }

		[JsonProperty ("credentials")]
		public Credentials Credentials { get; set; }
	}

	public class DialogConfiguration
	{

		[JsonProperty ("dialog")]
		public IList<Dialog> Dialog { get; set; }
	}
		
}

