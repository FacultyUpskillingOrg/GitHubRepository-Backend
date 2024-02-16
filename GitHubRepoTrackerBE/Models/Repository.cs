namespace GitRepositoryTracker.Models
{
    public class Repository
    {
        public string RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PushedAt { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public ICollection<RepositoryTopic> RepositoryTopics { get; set; } = new List<RepositoryTopic>();


    }
}
