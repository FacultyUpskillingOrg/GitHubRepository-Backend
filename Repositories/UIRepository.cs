using AutoMapper;
using AutoMapper.QueryableExtensions;
using GitRepositoryTracker.DButil;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace GitRepositoryTracker.Repositories
{
    public class UIRepository : IUIGenericRepository
    {
        private readonly GitRepoContext _context;
        private readonly IMapper _mapper;
        public UIRepository(GitRepoContext gitRepoContext, IMapper mapper)
        {
            _context = gitRepoContext;
            _mapper = mapper;
        }
        public async Task<PagedList<RepositoryDto>> GetAllByDateAsync(int pageNumber, int pageSize)
        {
            var repositories = _context.Repositories
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(rep => rep.UpdatedAt);              

            return await PagedList<RepositoryDto>.CreateAsync(repositories,pageNumber,pageSize);
        } 

        public async Task<PagedList<RepositoryDto>> GetAllByForksAsync(int pageNumber, int pageSize)
        {
            var repositories =_context.Repositories
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(rep => rep.StargazersCount);
            return await PagedList<RepositoryDto>.CreateAsync(repositories, pageNumber, pageSize);
        }

        public async Task<PagedList<RepositoryDto>> GetAllByLanguageAsync(string language, int pageNumber, int pageSize)
        {
            var repositories =_context.Repositories
                .Include(l => l.Language)
                .Where(rep => rep.Language.LanguageName == language)
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider);

            return await PagedList<RepositoryDto>.CreateAsync(repositories, pageNumber,pageSize);
        }

        public async Task<PagedList<RepositoryDto>> GetAllByStarsAsync(int pageNumber, int pageSize)
        {
            var repositories = _context.Repositories
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(rep => rep.StargazersCount);

            return await PagedList<RepositoryDto>.CreateAsync(repositories, pageNumber, pageSize);
        }

        public async Task<PagedList<RepositoryDto>> GetAllByTopicAsync(string topicName, int pageNumber, int pageSize)
        {
            var repositories = _context.Repositories
                .Include(r => r.RepositoryTopics)
                .ThenInclude(rt => rt.Topic)
                .Where(r => r.RepositoryTopics.Any(rt => rt.Topic.TopicName == topicName))
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider);

            return await PagedList<RepositoryDto>.CreateAsync(repositories, pageNumber, pageSize);
        }      

        public async Task<PagedList<LanguageDto>> GetAllLanguagesAsync(int pageNumber, int pageSize)
        {
            var languages = _context.Languages
                .ProjectTo<LanguageDto>(_mapper.ConfigurationProvider);

            return await PagedList<LanguageDto>.CreateAsync(languages, pageNumber, pageSize);
        }     

        public async Task<PagedList<TopicDto>> GetAllTopicsAsync(int pageNumber, int pageSize)
        {
            var topics = _context.Topics
                  .ProjectTo<TopicDto>(_mapper.ConfigurationProvider);
            return await PagedList<TopicDto>.CreateAsync(topics, pageNumber, pageSize);
        }
        public async Task<PagedList<RepositoryDto>> GetAllRepositoriesAsync(int pageNumber, int pageSize)
        {
            var repositories = _context.Repositories
                .ProjectTo<RepositoryDto>(_mapper.ConfigurationProvider);

            return await PagedList<RepositoryDto>.CreateAsync(repositories, pageNumber, pageSize);  

        }
    }
}
