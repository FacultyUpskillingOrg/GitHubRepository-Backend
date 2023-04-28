namespace GitRepositoryTracker.Models
{
    public class Language
    {
        public int LanguageId { get;set; }
        public string LanguageName { get;set; }
        public ICollection<Repository> Repositories { get; set; }
    }
}
