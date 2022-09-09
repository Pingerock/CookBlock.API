using Microsoft.EntityFrameworkCore;

namespace CookBlock.API.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Food_Type> Food_Types { get; set; }
        public DbSet<Recipe_Rating> Ratings { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredients { get; set; }
        public DbSet<Recipe_Instruction> Recipe_Instructions { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
