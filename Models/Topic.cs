



namespace GitRepositoryTracker.Models
{
    public class Topic
    {

        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public ICollection<RepositoryTopic> RepositoryTopics { get; set; } = new List<RepositoryTopic>();
    }
}
