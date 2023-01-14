using System.Collections.Generic;

namespace NewsApp.Models.AppModels
{
	public class NewsResultModel
	{
		public int TotalResults { get; set; }

		public List<NewsModel> Articles { get; set; }
	}
}