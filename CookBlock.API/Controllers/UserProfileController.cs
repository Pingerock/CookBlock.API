using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBlock.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBlock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        ApplicationContext db;
        public UserProfileController(ApplicationContext context)
        {
            db = context;
        }

        // GET api/UserProfile/UserId?id=5
        [HttpGet("UserId")]
        public async Task<ActionResult<UserProfile>> Get(int id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            List<Comment> userComments = await db.Comments.Where(x => x.User_Id == user.Id).ToListAsync();
            List<Recipe_Rating> userRatings = await db.Ratings.Where(x => x.User_Id == user.Id).ToListAsync();
            UserProfile userProfile = new UserProfile(user, userComments, userRatings);
            return new ObjectResult(userProfile);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/UserProfile/delete?id=5
        // Удаляет учётную запись пользователя, его список избранных рецептов, оценок и комментариев 
        // из базы данных, НО оставляет созданные им рецепты, заменяя их параметр User_Id на "deletedUser"
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            List<Comment> userComments = await db.Comments.Where(x => x.User_Id == user.Id).ToListAsync();
            List<Recipe_Rating> userRatings = await db.Ratings.Where(x => x.User_Id == user.Id).ToListAsync();
            List<Favourite> userFavourites = await db.Favourites.Where(x => x.User_Id == user.Id).ToListAsync();
            List<Recipe> userRecipes = await db.Recipes.Where(x => x.User_Id == user.Id).ToListAsync();
            foreach (Comment c in userComments)
            {
                db.Comments.Remove(c);
            }
            foreach (Recipe_Rating r in userRatings)
            {
                db.Ratings.Remove(r);
            }
            foreach (Favourite f in userFavourites)
            {
                db.Favourites.Remove(f);
            }
            foreach (Recipe r in userRecipes)
            {
                r.User_Id = 8;
                db.Recipes.Update(r);
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }
    }
}
