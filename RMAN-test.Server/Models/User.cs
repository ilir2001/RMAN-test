using System.ComponentModel.DataAnnotations;

namespace RMAN_test.Server.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
