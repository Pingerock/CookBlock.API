namespace CookBlock.API.Models
{
    public class Recipe_Rating
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }
        public int Rating_Score { get; set; }
    }
}
