namespace CookBlock.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
