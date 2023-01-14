﻿using System.Collections.Generic;

namespace NewsApp.Constants
{
	public static class AppContants
	{
		public const string DbName = "newsapp.db3";
		public const string DbSecretKey = "NewsAppR1x";
		public const string NewsApiKey = "5c82c93da1224984a0d90e3629439712";

		public const SQLite.SQLiteOpenFlags SQLQueryFlags =
					SQLite.SQLiteOpenFlags.ReadWrite |
					SQLite.SQLiteOpenFlags.SharedCache;

		public const SQLite.SQLiteOpenFlags DbCreateFlags =
							SQLite.SQLiteOpenFlags.ReadWrite |
							SQLite.SQLiteOpenFlags.Create |
							SQLite.SQLiteOpenFlags.SharedCache;
		
		private static readonly List<string> list = new List<string>(new string[] { "Healthy", "Technology", "Finance", "Arts", "Sports" });
		public static List<string> FilterOptions { get => list; }
	}
}