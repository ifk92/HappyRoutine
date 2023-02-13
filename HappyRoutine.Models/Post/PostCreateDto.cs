using System.ComponentModel.DataAnnotations;

namespace HappyRoutine.Models.Post
{
    public class PostCreateDto
    {
        public int PostId { get; set; }
    
        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Must be 10-240 characters")]
        [MaxLength(240, ErrorMessage = "Must be 10-240 characters")]
        public string Content { get; set; }

    }
}