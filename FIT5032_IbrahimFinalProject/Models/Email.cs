using System.ComponentModel.DataAnnotations;

namespace FIT5032_IbrahimFinalProject.Models
{
    public class Email
    {
        public int ID { get; set; }
        [Required]
        [EmailAddress]
        public string From { get; set; }
        [Required]
        [EmailAddress]
        public string To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content
        {
            get; set;
        }
        public string? Path { get; set; }
        public string? FileName { get; set; }
    }
}
