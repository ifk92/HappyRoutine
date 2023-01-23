namespace HappyRoutine.Models.Comment
{
    public class CommentCreate
    {
        public int CommentId { get; set; }

        public int? ParentCommentId { get; set; }

        public int PostId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Must be 10-300 characters")]
        [MaxLength(300, ErrorMessage = "Must be 10-300 characters")]
        public string Content { get; set; }
    }
}