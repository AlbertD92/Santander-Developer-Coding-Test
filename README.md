# Santander Developer Coding Test
A minimal ASP.NET 8 REST API that returns the top **n** Hacker News stories ranked by score.

## How to Run
### Requirements
 - .NET SDK 8.0+
 - Git

### Using the terminal
```bash
git clone https://github.com/AlbertD92/Santander-Developer-Coding-Test.git
```
```bash
cd Santander-Developer-Coding-Test
```
```bash
dotnet run
```

### Running via Visual Studio
Open `Santander Developer Coding Test.sln` in Visual Studio 2022+, and press **F5** or click **Run**.

---
The API should start on `http://localhost:5169` (or `https://localhost:7230`).

Navigate to:

http://localhost:5169/swagger
or
https://localhost:7230/swagger


## Design Decisions & Assumptions
- Used MinimalAPI for simplicity.
- The `beststories.json` endpoint from HackerNews always returns 200 stories. For that reason **n** was limited to that same number.
- The beststories and details for each story are not updated that often. Thus, In-memory caching strategy was applied to reduce calls to the Hacker News API.
  - **Best story Ids** — cached for **5 minutes**.
  - **Individual story details** — cached per story Id for **10 minutes**.
- Each story detail is fetched individually but using concurrent calls with the **Parallel libray** to enhance performance.
- Logger was added when doing calls to the Hacker News API.

## Possible Enhancements
- **Distributed caching**: Replacing `IMemoryCache` with Redis to share the cache across multiple API instances.
- **Configuration**: Move options (like the cache durations) out of the classes, to `appsettings.json` to better read and apply them using **options pattern**.
- **Polly retry/circuit-breaker/Rate Limiter**
- **Testing**: Unit testing the service layer; and Integration test for the endpoint.
- **Proper logging with Serilog**
- **Health checks**: To verify connectivity to the Hacker News API.
- **Proper Input Validation**
