using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Budget.API.Models.Food;
using Budget.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Budget.Core;


namespace Budget.API 
{

    [Authorize]
    [Route("api/food/meal-plans")]
    public class MealPlanController: ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly ILogger _logger;
        public MealPlanController (DatabaseContext context, ILogger<MealPlanController> logger)
        {
            _context = context;
            _logger = logger;
            
        }
        
        [HttpGet("meal-plan/{id}")]
        public IActionResult GetMealPlan (int id)
        {
            return Ok(
                _context.MealPlans.FirstOrDefault(o => o.id == id)
            );
        }

        [HttpGet("user/{name}")]
        public IActionResult GetMealPlans (string name)
        {
            bool isAll = name == "All" ? true : false;
            return Ok(
                _context.MealPlans.Where(o => isAll || o.user == name)
            );
        }

        [HttpGet("recipes/{id}")]
        public IActionResult GetMealPlanRecipes (int id)
        {
            return Ok(
                _context.MealPlanRecipes.Where(o => o.mealPlanId == id)
            );
        }

        [HttpGet("recipe/{id}")]
        public IActionResult GetMealPlanRecipe (int id)
        {
            return Ok(
                _context.MealPlanRecipes.FirstOrDefault(o => o.id == id)
            );
        }

        [HttpGet("groceries/{id}")]
        public IActionResult GetMealPlanGroceries (int id)
        {
            return Ok(
                _context.MealPlanGroceries.Where(o => o.mealPlanId == id).OrderBy(o => o.name)
            );
        }

        [HttpPost("meal-plan")]
        public IActionResult InsertMealPlan([FromBody] MealPlan item)
        {
            if (!ModelState.IsValid) {
                _logger.LogWarning(LoggingEvents.InsertItemBadRequest, $"Insert BAD REQUEST");
                return BadRequest();
            }
            try {
               _context.Database.ExecuteSqlCommand(
                    @"INSERT INTO MealPlansView(name, user, days)
                    VALUES 
                        ({0}, {1}, {2});
                    ",
                    item.name, item.user, item.days);
            }
            catch (Exception ex){
                _logger.LogWarning(LoggingEvents.InsertItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(_context.MealPlans.FirstOrDefault(o => o.name  == item.name));
        }

        [HttpPut("meal-plan/{id}")]
        public IActionResult UpdateMealPlan(int id, [FromBody] MealPlan item)
        {
            if (!ModelState.IsValid || item == null || item.id != id) {
                _logger.LogWarning(LoggingEvents.UpdateItemBadRequest, $"UPDATE({id}) BAD REQUEST");
                return BadRequest();
            }

            var mealPlan = _context.MealPlans.FirstOrDefault(t => t.id == id);
            if (mealPlan == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, $"UPDATE(id) NOT FOUND");
                return NotFound();
            }

 
            try {
                _context.Database.ExecuteSqlCommand(
                    @"UPDATE MealPlansView SET
                        name = {0},
                        user = {1},
                        days = {2}
                    WHERE id = {3};
                    ",
                    item.name, item.user, item.days, item.id);
            }
            catch (Exception ex){
                _logger.LogError(LoggingEvents.UpdateItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [HttpDelete("meal-plan/{id}")]
        public IActionResult DeleteMealPlan(int id)
        {
            var mealPlan = _context.MealPlans.FirstOrDefault(t => t.id == id);
            if (mealPlan == null)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemNotFound, $"DELETE(id) NOT FOUND");
                return NotFound();
            }

            try {
                _context.Database.ExecuteSqlCommand(@"DELETE FROM MealPlansView WHERE id = {0};", mealPlan.id);
            }
            catch (Exception ex){
                _logger.LogError(LoggingEvents.DeleteItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
 
        [HttpPost("recipes")]
        public IActionResult InsertMealPlanRecipes([FromBody] MealPlanRecipeList list)
        {
            if (!ModelState.IsValid || list.recipes.Count == 0) {
                _logger.LogWarning(LoggingEvents.InsertItemBadRequest, $"Insert BAD REQUEST");
                return BadRequest();
            }
            try {

                foreach (MealPlanRecipe item in list.recipes) {
                    _context.Database.ExecuteSqlCommand(
                        @"INSERT INTO MealPlanRecipesView(meal_plan_name, name, count)
                        VALUES 
                            ({0}, {1}, {2});
                        ",
                        item.mealPlanName, item.name, item.count);
                }
            }
            catch (Exception ex){
                _logger.LogWarning(LoggingEvents.InsertItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }


        [HttpPost("recipe")]
        public IActionResult InsertMealPlanRecipe([FromBody] MealPlanRecipe item)
        {
            if (!ModelState.IsValid) {
                _logger.LogWarning(LoggingEvents.InsertItemBadRequest, $"Insert BAD REQUEST");
                return BadRequest();
            }
            try {
               _context.Database.ExecuteSqlCommand(
                    @"INSERT INTO MealPlanRecipesView(meal_plan_name, name, count)
                    VALUES 
                        ({0}, {1}, {2});
                    ",
                    item.mealPlanName, item.name, item.count);
            }
            catch (Exception ex){
                _logger.LogWarning(LoggingEvents.InsertItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [HttpPut("recipe/{id}")]
        public IActionResult UpdateMealPlanRecipe(int id, [FromBody] MealPlanRecipe item)
        {
            if (!ModelState.IsValid || item == null || item.id != id) {
                _logger.LogWarning(LoggingEvents.UpdateItemBadRequest, $"UPDATE({id}) BAD REQUEST");
                return BadRequest();
            }

            var mealPlan = _context.MealPlanRecipes.FirstOrDefault(t => t.id == id);
            if (mealPlan == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, $"UPDATE(id) NOT FOUND");
                return NotFound();
            }

 
            try {
                _context.Database.ExecuteSqlCommand(
                    @"UPDATE MealPlanRecipesView SET
                        meal_plan_name = {0},
                        name = {1},
                        count = {2}
                    WHERE id = {3};
                    ",
                    item.mealPlanName, item.name, item.count, item.id);
            }
            catch (Exception ex){
                _logger.LogError(LoggingEvents.UpdateItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [HttpDelete("recipe/{id}")]
        public IActionResult DeleteMealPlanRecipe(int id)
        {
            var mealPlanRecipe = _context.MealPlanRecipes.FirstOrDefault(t => t.id == id);
            if (mealPlanRecipe == null)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemNotFound, $"DELETE(id) NOT FOUND");
                return NotFound();
            }

            try {
                _context.Database.ExecuteSqlCommand(@"DELETE FROM MealPlanRecipesView WHERE id = {0};", mealPlanRecipe.id);
            }
            catch (Exception ex){
                _logger.LogError(LoggingEvents.DeleteItemApplicationError, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
 
 
    }
}
