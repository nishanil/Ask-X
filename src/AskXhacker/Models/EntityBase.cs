using System;
using SQLite;

namespace AskXhacker
{
	public class EntityBase : ObservableObject
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
	}
}

