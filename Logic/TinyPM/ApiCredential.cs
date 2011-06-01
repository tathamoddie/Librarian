using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace Librarian.Logic.TinyPM
{
    public class ApiCredential
    {
        [Display(Name = "tinyPM Instance URI")]
        [Required]
        public string InstanceUri { get; set; }

        [Display(Name = "API Key")]
        [Required]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "The API Key must be 32 hexadecimal characters.")]
        public string ApiKey { get; set; }

        public NameValueCollection ToNameValueCollection()
        {
            return new NameValueCollection
            {
                { "InstanceUri", InstanceUri },
                { "ApiKey", ApiKey },
            };
        }

        public static ApiCredential FromNameValueCollection(NameValueCollection data)
        {
            return new ApiCredential
            {
                InstanceUri = data["InstanceUri"],
                ApiKey = data["ApiKey"],
            };
        }
    }
}