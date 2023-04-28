namespace GitRepositoryTracker.Models
{
    public class RepositoryTopic
    {
        public string RepositoryId { get; set; }
        public Repository? Repository { get; set; }

        public int TopicId { get; set; }
        public Topic? Topic { get; set; }

    }
}
