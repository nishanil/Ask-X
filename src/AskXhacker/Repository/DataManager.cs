using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskXhacker
{
	public class DataManager
	{
		readonly AskXDatabase db;
		public DataManager ()
		{
			db = new AskXDatabase ();
		}
		public Task<int> SaveMessage(Message message)
		{
			return db.SaveItemAsync<Message> (message);
		}

		public Task<List<Message>> GetAllMessages()
		{
			return db.GetItemsAsync<Message> ();
		}

		public Task DropDatabase ()
		{
			return db.DropAll ();
		}

	}
}

