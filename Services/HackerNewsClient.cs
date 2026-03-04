using SantanderDeveloperCodingTest.Models;
using System.Text.Json;

namespace SantanderDeveloperCodingTest.Services
{
	public interface IHackerNewsClient
	{
		Task<int[]> GetBestStoryIdsAsync(CancellationToken token = default);
		Task<HackerNewsItem?> GetStoryDetailsAsync(int id, CancellationToken token = default);
	}
	public class HackerNewsClient(HttpClient http) : IHackerNewsClient
	{
		public async Task<int[]> GetBestStoryIdsAsync(CancellationToken token = default)
		{
			return await http.GetFromJsonAsync<int[]>("beststories.json", token) ?? [];
		}

		public async Task<HackerNewsItem?> GetStoryDetailsAsync(int id, CancellationToken token = default)
		{
			return await http.GetFromJsonAsync<HackerNewsItem>($"item/{id}.json", token);
		}
	}
}
