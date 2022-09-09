namespace CookBlock.API.Models
{
    public class Recipe_Ingredient
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
