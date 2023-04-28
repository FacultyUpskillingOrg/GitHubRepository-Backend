using GitRepositoryTracker.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Polly;

namespace GitRepositoryTracker.Services
{
    public class GitHubAPIService : IGitHubAPIService
    {

        private readonly IGitHubClient _gitHubClient;
        private IMemoryCache _memoryCache;
        
        public GitHubAPIService(IGitHubClient gitHubClient, IMemoryCache memoryCache)
        {
            _gitHubClient = gitHubClient;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesBySize(int size, int page, int perPage)
        {

            var repositories = new List<Repository>();
            var searchRequest = new SearchRepositoriesRequest
            {
                Size = Octokit.Range.GreaterThanOrEquals(size),
                Page = page,
                PerPage = perPage,
                SortField = RepoSearchSort.Stars
            };

            var searchResult = await SearchRepositoriesWithRetry(searchRequest);
            repositories.AddRange(searchResult.Items);

            while (searchResult?.IncompleteResults == false)
            {
                searchRequest.Page++;
                searchResult = await SearchRepositoriesWithRetry(searchRequest);
                repositories.AddRange(searchResult.Items);
            }

            return repositories;
        }

        private async Task<SearchRepositoryResult> SearchRepositoriesWithRetry(SearchRepositoriesRequest searchRequest)
        {
            var retryPolicy = Policy
                .Handle<SecondaryRateLimitExceededException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var cacheKey = $"RepoSearch:{searchRequest.Size}:{searchRequest.Page}:{searchRequest.PerPage}:{searchRequest.SortField}";

            SearchRepositoryResult searchResult;
            // Try to get the search result from the cache
            if (!_memoryCache.TryGetValue(cacheKey, out searchResult))
            {
                // If the result is not in the cache, execute the search request with the retry policy
                searchResult = await retryPolicy.ExecuteAsync(async () => await _gitHubClient.Search.SearchRepo(searchRequest));
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Expire if not accessed for 5 minutes
                _memoryCache.Set(cacheKey, searchResult, cacheEntryOptions);
            }

            return searchResult;
        }

    }
}











