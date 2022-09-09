namespace CookBlock.API.Models
{
    public class FullRecipe
    {
        public Recipe recipe { get; set; }
        public List<Recipe_Ingredient> ingredients { get; set; }
        public List<Recipe_Instruction> instructions { get; set; }
        public List<Comment> comments { get; set; }
        public List<Recipe_Rating> ratings { get; set; }
        public Food_Type food_type { get; set; }

        public FullRecipe(Recipe _recipe, List<Recipe_Ingredient> _ingredients, List<Recipe_Instruction> _instructions,
            List<Comment> _comments, List<Recipe_Rating> _ratings, Food_Type _food_type)
        {
            recipe = _recipe;
            ingredients = _ingredients;
            instructions = _instructions;
            comments = _comments;
            ratings = _ratings;
            food_type = _food_type;
        }

        public FullRecipe()
        {
            recipe = new Recipe();
            ingredients = new List<Recipe_Ingredient>();
            instructions = new List<Recipe_Instruction>();
            comments = new List<Comment>();
            ratings = new List<Recipe_Rating>();
            food_type = new Food_Type();
        }
    }
}
