using AutoMapper;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using System.Diagnostics;

namespace GitRepositoryTracker.Services
{
    public class RepositoryDeserializer : IRepositoryDeserializer
    {
        private readonly IMapper _mapper;

        public RepositoryDeserializer(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<Repository>> DeserializeRepositories(IEnumerable<Octokit.Repository> repositories)
        {
            var result = new List<Repository>();

            Debug.WriteLine($"repositories count: {repositories.Count()}");
            foreach (var repo in repositories)
            {
                Debug.WriteLine($"repo.RepositoryTopics: {repo}");
                var repositoryDto = _mapper.Map<RepositoryDto>(repo);
                Debug.WriteLine($"repositoryDto.RepositoryTopics: {repositoryDto.RepositoryTopics}");
                var repository = _mapper.Map<Repository>(repositoryDto);
                Debug.WriteLine($"repository: {repository.RepositoryTopics}");

                result.Add(repository);
            }
            Debug.WriteLine($"result count: {result.Count}");


            return result;
        }

        //public async Task<IEnumerable<Repository>> DeserializeRepositories(IEnumerable<Octokit.Repository> repositories)
        //{
        //    var result = new List<Repository>();

        //    foreach (var repo in repositories)
        //    {
        //        if (repo != null)
        //        {
        //            var repositoryDto = _mapper.Map<RepositoryDto>(repo);                   
        //            repositoryDto.RepositoryId = repo.NodeId;

        //            var repository = _mapper.Map<Repository>(repositoryDto);

        //            if (repository != null && repository.RepositoryId != null)
        //            {
        //                // Set a default value for the Language property if it is null
        //                repository.Language ??= "None";

        //                var topics = repo.Topics?.Select(t => t ?? "None").Select(t => new TopicDto { TopicName = t }).Distinct().ToList();
        //                if (topics != null && topics.Any())
        //                {
        //                    foreach (var topic in topics)
        //                    {
        //                        var repositoryTopicDto = new RepositoryTopicDto { Repositorydto = repositoryDto, Topicdto = topic };

        //                        var repositoryTopic = _mapper.Map<RepositoryTopic>(repositoryTopicDto);
        //                        repository.RepositoryTopics?.Add(repositoryTopic);
        //                    }
        //                }

        //                result.Add(repository);
        //            }
        //        }
        //    }

        //    return result;
        //}





    }
}
