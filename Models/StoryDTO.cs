namespace SantanderDeveloperCodingTest.Models
{
	public class StoryDTO
	{
		public string Title { get; set; }
		public string Uri { get; set; }
		public string PostedBy {  get; set; }
		public string Time { get; set; }
		public int Score { get; set; }
		public int CommentCount { get; set; }

		public static StoryDTO FromItem(HackerNewsItem item)
		{
			return new StoryDTO
			{
				Title = item.Title ?? "",
				Uri = item.Url ?? "",
				PostedBy = item.By ?? "",
				Time = DateTimeOffset.FromUnixTimeSeconds(item.Time ?? 0).ToString("O"),
				Score = item.Score ?? 0,
				CommentCount = item.Descendants ?? 0
			};
		}
	}
}
