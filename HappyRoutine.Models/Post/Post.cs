namespace HappyRoutine.Models.Post
{
    public class Post : PostCreateDto
    {
        public string Username { get; set; }

        public int ApplicationUserId { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}