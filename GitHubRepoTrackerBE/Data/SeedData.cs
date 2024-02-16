using GitRepositoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GitRepositoryTracker.Data
{
    public static class SeedData
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Seed topics
            var topics = new List<Topic>
                {
                new Topic { TopicId = -1,TopicName = "C#"},
                new Topic { TopicId = -2,TopicName = "javaScript"},
                new Topic { TopicId = -3,TopicName = "python"},
                new Topic { TopicId = -4,TopicName = "careers" },
                new Topic { TopicId = -5,TopicName = "certification"},
                new Topic { TopicId = -6,TopicName = "community" },
                new Topic { TopicId = -7,TopicName = "curriculum"},
                new Topic { TopicId = -8,TopicName = "education" },
                new Topic { TopicId = -9,TopicName = "freecodecamp"},
                new Topic { TopicId = -10,TopicName = "hacktoberfest" },
                new Topic { TopicId = -12,TopicName = "learn-to-code" }

            };
            modelBuilder.Entity<Topic>().HasData(topics);

            // Seed repositories
            var repositories = new List<Repository>
                {
                new Repository
                {
                    RepositoryId="MGEwOlJlcG9zaXRvcnkyODQ1NzgyMz==",
                    RepositoryName = "Example RepositoryDto 1",
                    Description = "This is an example repository",
                    Url = "https://github.com/example/repository1",
                    StargazersCount = 10,
                    ForksCount = 2,
                    CreatedAt = new DateTime(2022, 1, 1),
                    UpdatedAt = new DateTime(2022, 1, 2),
                    PushedAt = new DateTime(2022, 1, 3)
                },
                new Repository
                {
                    RepositoryId="MGFyOlJlcG9zaXRvcnkyODQ1NzgyMt==",
                    RepositoryName = "Example RepositoryDto 2",
                    Description = "This is another example repository",
                    Url = "https://github.com/example/repository2",
                    StargazersCount = 5,
                    ForksCount = 1,
                    CreatedAt = new DateTime(2022, 1, 1),
                    UpdatedAt = new DateTime(2022, 1, 2),
                    PushedAt = new DateTime(2022, 1, 3)
                }
            };
            modelBuilder.Entity<Repository>().HasData(repositories);

            // Seed repository topics
            var repositoryTopics = new List<RepositoryTopic>
            {
                new RepositoryTopic
                {
                    RepositoryId = repositories[0].RepositoryId,
                    TopicId = topics.First(t => t.TopicName == "C#").TopicId
                },
                new RepositoryTopic
                {
                    RepositoryId = repositories[0].RepositoryId,
                    TopicId = topics.First(t => t.TopicName == "javaScript").TopicId
                },
                new RepositoryTopic
                {
                    RepositoryId = repositories[1].RepositoryId,
                    TopicId = topics.First(t => t.TopicName == "python").TopicId
                }
            };

            modelBuilder.Entity<RepositoryTopic>().HasData(repositoryTopics);

        }
    }
}



