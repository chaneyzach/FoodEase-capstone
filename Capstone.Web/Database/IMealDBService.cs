using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Database
{
    public interface IMealDBService
    {
        //User
        int AddUser(User user);
        User GetUser(int userId);
        User GetUser(string username);

        List<Ingredient> GetAllIngredientsByUserId(int userId);
        List<Ingredient> GetAllIngredientsInRecipe(int recipeId);
        int AddIngredient(Ingredient ingredient);
        bool IngredientExists(string name);
        int GetIngredientIdByName(string name);

        List<Recipe> GetAllRecipesByUserId(int userId);
        RecipeDetailViewModel GetRecipeByRecipeId(int recipeId);
        string GetImageByRecipeId(int recipeId);
        int AddRecipe(Recipe recipe);
        bool AssignRecipeIngredient(int RecipeId, int IngredientId, string quantity);
        bool AssignRecipeImage(int recipeId, byte[] image_byte);
        bool DeleteRecipeIngredient(int recipeId, int ingredientId);
        bool UpdateRecipe(Recipe recipe);
        bool DeleteRecipe(Recipe recipe);
        bool DeleteRecipeFromMeal(Recipe recipe);
        bool DeletaAllRecipesFromMeal(int mealId);

        List<Meal> GetAllMealsByUserId(int userId);
        List<Recipe> GetRecipesByMealId(int mealId);
        Meal GetMealByMealId(int mealId);
        Meal GetMealByMealName(string name);
        int AddMeal(Meal meal);
        bool AssignRecipeToMeal(int mealId, int recipeId);
        bool MealExists(string mealName);

        List<Plan> GetAllPlansByUserId(int userId);
        List<Meal> GetMealsByPlanId(int planId);
        Plan GetPlanByPlanId(int planId);
        int AddPlan(Plan plan);
        bool AssignMealToPlan(int planId, int mealId, int dayId);
        List<Meal> GetMealsOnDay(int planId, int dayId);
        bool PlanExists(string planName);
    }
}
