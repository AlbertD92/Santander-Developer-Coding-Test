using SantanderDeveloperCodingTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IHackerNewsClient, HackerNewsClient>(client =>
{
	client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/beststories", async (int n, IHackerNewsService service) =>
{
	if (n <= 0) { return Results.BadRequest("Parameter 'n' should be grater than 0"); }

	if (n > 200) { n = 200; }

	var stories = await service.GetBestStoriesAsync(n);
	return Results.Ok(stories);
})
	.WithSummary("Returns the best n stories by Score")
	.WithDescription("parameter 'n' is capped to 200 results.");

app.Run();