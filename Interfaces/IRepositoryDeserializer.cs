using GitRepositoryTracker.Models;

namespace GitRepositoryTracker.Interfaces
{
    public interface IRepositoryDeserializer
    {
        Task<IEnumerable<Repository>> DeserializeRepositories(IEnumerable<Octokit.Repository> repositories);
    }
}
