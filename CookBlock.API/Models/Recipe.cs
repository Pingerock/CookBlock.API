namespace CookBlock.API.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string? Picture_base64 { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Food_Type_Id { get; set; }
    }
}
