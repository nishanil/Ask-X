using System;
using SQLite;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace AskXhacker
{
	public class AskXDatabase
	{
		readonly SQLiteAsyncConnection database;

		public static string DatabaseLocation {
			get;
			set;
		}

		public AskXDatabase ()
		{
			database = new SQLiteAsyncConnection (DatabaseLocation);
			// Hack: NULLReference exception
			// Since ViewModels race to get messages, this one throws up!
			// System.Collections.Generic.Dictionary`2<string, SQLite.TableMapping>.Insert (string,SQLite.TableMapping,bool) <0x004b0>
			Create ().Wait();
		}

		public Task Create ()
		{	
			return Task.Run (()=>
				database.CreateTableAsync<Message> ());
		}

		public async Task DropAll()
		{
			await database.DropTableAsync<Message> ();
		}


		public Task<List<T>> GetItemsAsync<T> (Expression<Func<T, bool>> predicate = null) where T : EntityBase, new()
		{
			if (predicate == null)
			{
				return database.Table<T> ().ToListAsync ();
			}
			return database.Table<T>().Where(predicate).ToListAsync();
		}

		public Task<T> GetItemAsync<T> (int id) where T : EntityBase, new()
		{
			return database.Table<T> ().Where (x => x.Id == id).FirstOrDefaultAsync ();
		}

		public Task<T> GetItemAsync<T>(Expression<Func<T, bool>> predicate) where T : EntityBase, new()
		{
			return database.Table<T>().Where(predicate).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync<T> (T item) where T : EntityBase
		{
			if (item.Id != 0) {
				return database.UpdateAsync (item);
			} else {
				return database.InsertAsync (item);
			}
		}

		public async Task SaveItemsAsync<T>(IEnumerable<T> items) where T : EntityBase
		{
			var withId = items.Where(i => i.Id != 0).ToList();
			var withoutId = items.Where(i => i.Id == 0).ToList();

			if (withId.Count > 0)
			{
				await database.UpdateAllAsync(withId);
			}
			if (withoutId.Count > 0)
			{
				await database.InsertAllAsync(withoutId);
			}
		}

		public Task<int> DeleteItemAsync<T> (T item) where T : EntityBase, new()
		{
			return database.DeleteAsync (item);
		}

		public SQLiteAsyncConnection Database { get { return database; } }
	}
}

