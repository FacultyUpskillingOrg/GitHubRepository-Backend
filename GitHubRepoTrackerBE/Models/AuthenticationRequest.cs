using System.ComponentModel.DataAnnotations;

namespace GitRepositoryTracker.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
