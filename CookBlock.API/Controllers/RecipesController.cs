using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBlock.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CookBlock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        ApplicationContext db;

        public RecipesController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await db.Recipes.ToListAsync();
        }

        // GET api/Recipes/byFoodType?id=5
        [HttpGet("byFoodType")]
        public async Task<IEnumerable<Recipe>> GetRecipesByFoodType(int foodTypeId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.Food_Type_Id == foodTypeId).ToListAsync();
            return recipes;
        }

        // GET api/Recipes/byUserId?userId=5
        [HttpGet("byUserId")]
        public async Task<IEnumerable<Recipe>> GetRecipesByUser(int userId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.User_Id == userId).ToListAsync();
            return recipes;
        }

        [HttpGet("{userId}/Favourites")]
        public async Task<IEnumerable<Recipe>> GetFavouriteRecipes(int userId)
        {
            List<Favourite> recipesIds = await db.Favourites.Where(x => x.User_Id == userId).ToListAsync();
            List<Recipe> favourites = new List<Recipe>();
            foreach (Favourite recipeId in recipesIds)
            {
                Recipe recipe = await db.Recipes.FirstOrDefaultAsync(x => x.Id == recipeId.Recipe_Id);
                favourites.Add(recipe);
            }
            return favourites;
        }

        [HttpGet("Favourites/byUserId")]
        public async Task<IEnumerable<Favourite>> GetFavouritesByUser(int userId)
        {
            List<Favourite> favourites = await db.Favourites.Where(x => x.User_Id==userId).ToListAsync();
            return favourites;
        }

        [HttpGet("Favourites/all")]
        public async Task<IEnumerable<Favourite>> GetAllFavouritesFromMyRecipes(int userId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.User_Id == userId).ToListAsync();
            List<Favourite> allFavourites = new List<Favourite>();
            foreach (Recipe recipe in recipes)
            {
                List<Favourite> recipefavourites = await db.Favourites.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                foreach (Favourite favourite in recipefavourites)
                {
                    allFavourites.Add(favourite);
                }
            }
            return allFavourites;
        }

        [HttpGet("{recipeId}/Comments")]
        public async Task<IEnumerable<Comment>> GetCommentsFromRecipe(int recipeId)
        {
            List<Comment> comments = await db.Comments.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            return comments;
        }

        [HttpGet("Comments/all")]
        public async Task<IEnumerable<Comment>> GetAllCommentsFromMyRecipes(int userId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.User_Id == userId).ToListAsync();
            List<Comment> allComments = new List<Comment>();
            foreach (Recipe recipe in recipes)
            {
                List<Comment> recipeComments = await db.Comments.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                foreach (Comment comment in recipeComments)
                {
                    allComments.Add(comment);
                }
            }
            return allComments;
        }

        [HttpGet("{recipeId}/Ratings")]
        public async Task<IEnumerable<Recipe_Rating>> GetRatingsFromRecipe(int recipeId)
        {
            List<Recipe_Rating> ratings = await db.Ratings.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            return ratings;
        }

        [HttpGet("{recipeId}/Ratings/average")]
        public async Task<double> GetAverageRatingFromRecipe(int recipeId)
        {
            List<Recipe_Rating> ratings = await db.Ratings.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            double average = 0.0;
            foreach (Recipe_Rating rating in ratings)
            {
                average += rating.Rating_Score;
            }
            if (ratings.Count == 0)
            {
                average = 0.0;
            }
            else
            {
                average /= ratings.Count;
            }
            return average;
        }

        [HttpGet("Ratings/all")]
        public async Task<IEnumerable<Recipe_Rating>> GetAllRatingsFromMyRecipes(int userId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.User_Id == userId).ToListAsync();
            List<Recipe_Rating> allRatings = new List<Recipe_Rating>();
            foreach (Recipe recipe in recipes)
            {
                List<Recipe_Rating> recipeRatings = await db.Ratings.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                foreach (Recipe_Rating rating in recipeRatings)
                {
                    allRatings.Add(rating);
                }
            }
            return allRatings;
        }

        [HttpGet("{recipeId}/Ingredients")]
        public async Task<IEnumerable<Recipe_Ingredient>> GetIngredientsFromRecipe(int recipeId)
        {
            List<Recipe_Ingredient> ingredients = await db.Recipe_Ingredients.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            return ingredients;
        }

        [HttpGet("{recipeId}/Instructions")]
        public async Task<IEnumerable<Recipe_Instruction>> GetInstructionsFromRecipe(int recipeId)
        {
            List<Recipe_Instruction> instructions = await db.Recipe_Instructions.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            return instructions;
        }

        [HttpGet("{recipeId}/Full")]
        public async Task<ActionResult<FullRecipe>> GetFullRecipe(int recipeId)
        {
            Recipe recipe = await db.Recipes.FirstOrDefaultAsync(x => x.Id == recipeId);
            if (recipe == null)
            {
                return NotFound();
            }
            List<Recipe_Instruction> instructions = await db.Recipe_Instructions.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            List<Recipe_Ingredient> ingredients = await db.Recipe_Ingredients.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            List<Comment> comments = await db.Comments.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            List<Recipe_Rating> ratings = await db.Ratings.Where(x => x.Recipe_Id == recipeId).ToListAsync();
            Food_Type food_type = await db.Food_Types.FirstOrDefaultAsync(x => x.Id == recipe.Food_Type_Id);
            FullRecipe fullRecipe = new FullRecipe(recipe, ingredients, instructions, comments, ratings, food_type);
            return new ObjectResult(fullRecipe);
        }

        [HttpGet("{userId}/Best")]
        public async Task<ActionResult<FullRecipe>> GetBestFullRecipe(int userId)
        {
            List<Recipe> recipes = await db.Recipes.Where(x => x.User_Id == userId).ToListAsync();
            List<FullRecipe> fullRecipes = new List<FullRecipe>();
            foreach (var recipe in recipes)
            {
                List<Recipe_Instruction> instructions = await db.Recipe_Instructions.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                List<Recipe_Ingredient> ingredients = await db.Recipe_Ingredients.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                List<Comment> comments = await db.Comments.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                List<Recipe_Rating> ratings = await db.Ratings.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
                Food_Type food_type = await db.Food_Types.FirstOrDefaultAsync(x => x.Id == recipe.Food_Type_Id);
                FullRecipe fullRecipe = new FullRecipe(recipe, ingredients, instructions, comments, ratings, food_type);
                fullRecipes.Add(fullRecipe);
            }
            fullRecipes.OrderByDescending(x => x.comments.Count).ThenBy(x => x.ratings.Count);
            if (fullRecipes.Count == 0)
            {
                FullRecipe fullRecipe = new FullRecipe();
                return new ObjectResult(fullRecipe);
            }
            return new ObjectResult(fullRecipes[0]);
        }


        [HttpPost("Add/Recipe/Main")]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                return BadRequest();
            }
            db.Recipes.Add(recipe);
            await db.SaveChangesAsync();
            return Ok(recipe);
        }


        [HttpPost("Add/Recipe/Ingredient")]
        public async Task<ActionResult<Recipe_Ingredient>> PostIngredient(Recipe_Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest();
            }
            db.Recipe_Ingredients.Add(ingredient);
            await db.SaveChangesAsync();
            return Ok(ingredient);
        }

        [HttpPost("Add/Recipe/Instruction")]
        public async Task<ActionResult<Recipe_Instruction>> PostInstruction(Recipe_Instruction instruction)
        {
            if (instruction == null)
            {
                return BadRequest();
            }
            db.Recipe_Instructions.Add(instruction);
            await db.SaveChangesAsync();
            return Ok(instruction);
        }

        [HttpPost(("Add/Favourite"))]
        public async Task<ActionResult<Favourite>> PostFavouriteRecipe(Favourite favourite)
        {
            if (favourite == null)
            {
                return BadRequest();
            }
            db.Favourites.Add(favourite);
            await db.SaveChangesAsync();
            return Ok(favourite);
        }

        [HttpPost("Add/Recipe/Full")]
        public async Task<ActionResult<FullRecipe>> PostRecipe(FullRecipe fullRecipe)
        {
            if (fullRecipe == null)
            {
                return BadRequest();
            }
            db.Recipes.Add(fullRecipe.recipe);
            foreach (var ingredient in fullRecipe.ingredients)
            {
                db.Recipe_Ingredients.Add(ingredient);
            }
            foreach (var instruction in fullRecipe.instructions)
            {
                db.Recipe_Instructions.Add(instruction);
            }
            await db.SaveChangesAsync();
            return Ok(fullRecipe);
        }

        [HttpPost("Add/Rating")]
        public async Task<ActionResult<Recipe_Rating>> PostRating(Recipe_Rating rating)
        {
            if (rating == null)
            {
                return BadRequest();
            }

            db.Ratings.Add(rating);
            await db.SaveChangesAsync();
            return Ok(rating);
        }

        [HttpPost("Add/Comment")]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpPut("Update/Recipe/Full")]
        public async Task<ActionResult<FullRecipe>> PutRecipe(FullRecipe fullRecipe)
        {
            if (fullRecipe == null)
            {
                return BadRequest();
            }
            if (!db.Recipes.Any(x => x.Id == fullRecipe.recipe.Id))
            {
                return NotFound();
            }
            foreach (var ingredient in fullRecipe.ingredients)
            {
                if (!db.Recipes.Any(x => x.Id == fullRecipe.recipe.Id))
                {
                    return NotFound();
                }
            }
            foreach (var instruction in fullRecipe.instructions)
            {
                if (!db.Recipes.Any(x => x.Id == fullRecipe.recipe.Id))
                {
                    return NotFound();
                }
            }

            db.Update(fullRecipe.recipe);
            foreach (var ingredient in fullRecipe.ingredients)
            {
                db.Update(ingredient);
            }
            foreach (var instruction in fullRecipe.instructions)
            {
                db.Update(instruction);
            }
            await db.SaveChangesAsync();
            return Ok(fullRecipe);
        }

        [HttpPut("Update/Comment")]
        public async Task<ActionResult<Comment>> PutComment(Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }
            if (!db.Comments.Any(x => x.Id == comment.Id))
            {
                return NotFound();
            }

            db.Update(comment);
            await db.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpPut("Update/Rating")]
        public async Task<ActionResult<Recipe_Rating>> PutRating(Recipe_Rating rating)
        {
            if (rating == null)
            {
                return BadRequest();
            }
            if (!db.Ratings.Any(x => x.Id == rating.Id))
            {
                return NotFound();
            }

            db.Ratings.Update(rating);
            await db.SaveChangesAsync();
            return Ok(rating);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Recipe>> DeleteRecipe(int id)
        {
            Recipe recipe = await db.Recipes.FirstOrDefaultAsync(x => x.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            List<Recipe_Ingredient> ingredients = await db.Recipe_Ingredients.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
            List<Recipe_Instruction> instructions = await db.Recipe_Instructions.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
            List<Comment> comments = await db.Comments.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
            List<Recipe_Rating> ratings = await db.Ratings.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
            List<Favourite> favourites = await db.Favourites.Where(x => x.Recipe_Id == recipe.Id).ToListAsync();
            foreach (var ingredient in ingredients)
            {
                db.Recipe_Ingredients.Remove(ingredient);
            }
            foreach (var instruction in instructions)
            {
                db.Recipe_Instructions.Remove(instruction);
            }
            foreach (var comment in comments)
            {
                db.Comments.Remove(comment);
            }
            foreach (var rating in ratings)
            {
                db.Ratings.Remove(rating);
            }
            foreach (var favourite in favourites)
            {
                db.Favourites.Remove(favourite);
            }
            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();
            return Ok(recipe);
        }

        [HttpDelete("{recipeId}/Comments")]
        public async Task<ActionResult<Comment>> DeleteComment(int recipeId, int commentId)
        {
            Comment comment = await db.Comments.FirstOrDefaultAsync(x => x.Recipe_Id == recipeId && x.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpDelete("{recipeId}/Ratings")]
        public async Task<ActionResult<Recipe_Rating>> DeleteRating(int recipeId, int ratingId)
        {
            Recipe_Rating rating = await db.Ratings.FirstOrDefaultAsync(x => x.Recipe_Id == recipeId && x.Id == ratingId);
            if (rating == null)
            {
                return NotFound();
            }
            db.Ratings.Remove(rating);
            await db.SaveChangesAsync();
            return Ok(rating);
        }

        [HttpDelete("{userId}/Favourites/{recipeId}")]
        public async Task<ActionResult<Favourite>> DeleteFavourite(int userId, int recipeId)
        {
            Favourite favourite = await db.Favourites.FirstOrDefaultAsync(x => x.User_Id == userId && x.Recipe_Id == recipeId); 
            if (favourite == null)
            {
                return NotFound();
            }
            db.Favourites.Remove(favourite);
            await db.SaveChangesAsync();
            return Ok(favourite);
        }
    }
}
