namespace CookBlock.API.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }
    }
}
