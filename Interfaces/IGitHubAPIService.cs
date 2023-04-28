//using GitRepositoryTracker.Models;

using Octokit;

namespace GitRepositoryTracker.Interfaces
{
    public interface IGitHubAPIService
    {
        Task<IEnumerable<Repository>> GetAllRepositoriesBySize(int size = 2000, int page = 2, int perPage = 10);

    }
}
