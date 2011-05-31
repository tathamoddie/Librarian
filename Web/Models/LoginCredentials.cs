using System.ComponentModel.DataAnnotations;

namespace Librarian.Web.Models
{
    public class LoginCredentials
    {
        [Display(Name = "tinyPM Instance URI")]
        [Required]
        public string InstanceUri { get; set; }

        [Display(Name = "API Key")]
        [Required]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "The API Key must be 32 hexadecimal characters.")]
        public string ApiKey { get; set; }
    }
}