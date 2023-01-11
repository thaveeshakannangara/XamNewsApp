﻿namespace NewsApp.Constants
{
	public static class AppContants
	{
		public const string DbName = "newsapp.db3";
		public const string DbSecretKey = "NewsAppR1x";

		public const SQLite.SQLiteOpenFlags SQLQueryFlags =
					SQLite.SQLiteOpenFlags.ReadWrite |
					SQLite.SQLiteOpenFlags.SharedCache;

		public const SQLite.SQLiteOpenFlags DbCreateFlags =
							SQLite.SQLiteOpenFlags.ReadWrite |
							SQLite.SQLiteOpenFlags.Create |
							SQLite.SQLiteOpenFlags.SharedCache;
	}
}