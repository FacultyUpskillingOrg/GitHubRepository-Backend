using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Services;

namespace GitRepositoryTracker.Interfaces
{
    public interface IUIGenericRepository
    {

        Task<PagedList<RepositoryDto>> GetAllByTopicAsync(string topicName,int pageNumber, int pageSize);
        Task<PagedList<RepositoryDto>> GetAllByLanguageAsync(string language, int pageNumber, int pageSize);
        Task<PagedList<RepositoryDto>> GetAllByStarsAsync(int pageNumber, int pageSize);
        Task<PagedList<RepositoryDto>> GetAllByForksAsync(int pageNumber, int pageSize);
        Task<PagedList<RepositoryDto>> GetAllByDateAsync(int pageNumber, int pageSize);
        Task<PagedList<RepositoryDto>> GetAllRepositoriesAsync(int pageNumber, int pageSize);
        Task<PagedList<TopicDto>> GetAllTopicsAsync(int pageNumber, int pageSize);
        Task<PagedList<LanguageDto>> GetAllLanguagesAsync(int pageNumber, int pageSize);

    }
}
