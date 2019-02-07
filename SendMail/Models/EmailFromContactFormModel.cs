using System.ComponentModel.DataAnnotations;

namespace SendMail.Models
{
    public class EmailFromContactFormModel
    {
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Your name should only consist of letters")]
        [StringLength(50, MinimumLength = 2)]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please provide a valid email")]
        [Required(ErrorMessage = "You're missing your email")]
        public string Email { get; set; }

        [MinLength(1)]
        [Required]
        public string Message { get; set; }
    }
}
