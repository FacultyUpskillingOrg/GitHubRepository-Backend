using AutoMapper;
using GitRepositoryTracker.DButil;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GitRepositoryTracker.Repositories
{
    public class GitAPIRepository : IGitAPIRepository
    {
        private readonly GitRepoContext _context;
        private readonly IMapper _mapper;

        public GitAPIRepository(GitRepoContext gitRepoContext, IMapper mapper)
        {
            _context = gitRepoContext;
            _mapper = mapper;
        }
        public async Task AddTopic(Topic topic)
        {
            await _context.AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        public async Task Addtopics(IEnumerable<Topic> topics)
        {
            await _context.AddAsync(topics);
            await _context.SaveChangesAsync();
        }
        public async Task AddRepositories(IEnumerable<Octokit.Repository> repositories)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    foreach (var repository in repositories)
                    {
                        var repodto = _mapper.Map<RepositoryDto>(repository);
                        var repoentity = _mapper.Map<Models.Repository>(repodto);

                        var repoInDb = await _context.Repositories.FirstOrDefaultAsync(r => r.RepositoryId == repoentity.RepositoryId);

                        if (repoInDb != null)
                        {
                            // Repository already exists, skip it
                            continue;
                        }

                        // Add new language to database
                        var languageName = repository.Language ?? "None";
                        var languageEntity = await _context.Languages
                            .FirstOrDefaultAsync(l => l.LanguageName == languageName);

                        if (languageEntity == null)
                        {
                            languageEntity = new Language
                            {
                                LanguageName = languageName
                            };

                            _context.Languages.Add(languageEntity);
                            await _context.SaveChangesAsync();
                        }

                        repoentity.Language = languageEntity;

                        // Add new repository to database
                        _context.Repositories.Add(repoentity);
                        await _context.SaveChangesAsync();

                        // Add topics to database
                        var topicDtos = repository.Topics?.Select(topic => topic ?? "None")
                            .Select(topic => new TopicDto { TopicName = topic })
                            .Distinct().ToList();
                        var topicEntities = _mapper.Map<IEnumerable<Topic>>(topicDtos);

                        foreach (var topicEntity in topicEntities)
                        {
                            var existingTopic = await _context.Topics
                                .FirstOrDefaultAsync(t => t.TopicName == topicEntity.TopicName);

                            if (existingTopic != null)
                            {
                                topicEntity.TopicId = existingTopic.TopicId;
                            }
                            else
                            {
                                _context.Topics.Add(topicEntity);
                                await _context.SaveChangesAsync();
                            }

                            var existingRepositoryTopic = await _context.RepositoryTopics
                                .FirstOrDefaultAsync(rt => rt.TopicId == topicEntity.TopicId && rt.RepositoryId == repoentity.RepositoryId);

                            if (existingRepositoryTopic == null)
                            {
                                var repositoryTopicentity = new RepositoryTopic
                                {
                                    RepositoryId = repoentity.RepositoryId,
                                    TopicId = topicEntity.TopicId,
                                };

                                _context.RepositoryTopics.Add(repositoryTopicentity);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Debug.WriteLine("An error occurred while adding repositories to the database.", ex.Message);
                    throw;
                }
            });
        }

        public async Task<Models.Repository> GetRepositoryById(string id)
        {
            return await _context.Repositories.FindAsync(id);


        }

        public async Task<Topic> GetTopicById(int id)
        {
            return await _context.Topics.FindAsync(id);
        }

        public async Task AddLanguage(Language language)
        {
            await _context.AddAsync(language);
            await _context.SaveChangesAsync();
        }
    }
}
