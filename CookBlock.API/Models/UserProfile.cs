namespace CookBlock.API.Models
{
    public class UserProfile
    {
        public User user { get; set; }
        public List<Comment> comments { get; set; }
        public List<Recipe_Rating> ratings { get; set; }

        public UserProfile(User user, List<Comment> comments, List<Recipe_Rating> ratings)
        {
            this.user = user;
            this.comments = comments;
            this.ratings = ratings;
        }
    }
}
