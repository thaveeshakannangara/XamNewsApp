using SQLite;
using System;

namespace NewsApp.Models.DataModels
{
	public class UserDataModel
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime CreatedDateTime { get; set; }
	}
}