using Microsoft.Extensions.Caching.Memory;
using SantanderDeveloperCodingTest.Models;
using System.Collections.Concurrent;

namespace SantanderDeveloperCodingTest.Services
{
	public interface IHackerNewsService
	{
		Task<IEnumerable<StoryDTO>> GetBestStoriesAsync(int n);
	}

	public class HackerNewsService(
		IHackerNewsClient client,
		IMemoryCache cache,
		ILogger<HackerNewsService> logger) : IHackerNewsService
	{
		private const string IdsCacheKey = "beststories";
		private const string ItemCachePrefix = "item_";

		private static readonly TimeSpan IdsCacheTTL = TimeSpan.FromMinutes(5);
		private static readonly TimeSpan ItemCacheTTL = TimeSpan.FromMinutes(10);

		public async Task<IEnumerable<StoryDTO>> GetBestStoriesAsync(int n)
		{
			if (!cache.TryGetValue(IdsCacheKey, out int[]? ids) || ids == null)
			{
				try
				{
					logger.LogInformation("Fetching BestStoryIds");
					ids = await client.GetBestStoryIdsAsync();
					if (ids.Length > 0)
					{
						cache.Set(IdsCacheKey, ids, IdsCacheTTL);
					}
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Failed to fetch BestStoryIds");
					ids = [];
				}
			}

			var hackerNewsItems = new ConcurrentBag<HackerNewsItem>();

			await Parallel.ForEachAsync(ids, async (id, token) =>
			{
				var itemKey = ItemCachePrefix + id;
				if (!cache.TryGetValue(itemKey, out HackerNewsItem? item))
				{
					try
					{
						logger.LogInformation("Fetching item {Id}", id);
						item = await client.GetStoryDetailsAsync(id, token);
					}
					catch (Exception ex)
					{
						logger.LogError(ex, "Failed to fetch item {Id}", id);
					}
				}
				if (item != null)
				{
					cache.Set(itemKey, item, ItemCacheTTL);
					hackerNewsItems.Add(item);
				}
			});

			return hackerNewsItems
				.OrderByDescending(x => x.Score)
				.Take(n)
				.Select(x => StoryDTO.FromItem(x));
		}
	}
}
