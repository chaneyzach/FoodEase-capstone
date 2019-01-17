using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using Capstone.Web.Helpers;
using Capstone.Web.Database;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Mock
{
    public class MockMealDBService : IMealDBService
    {
        public MockMealDBService()
        {
            if (_users.Count == 0)
            {
                TestHelper.PopulateDatabaseWithUsers(this);
            }
        }

        private static Dictionary<int, User> _users = new Dictionary<int, User>();
        private static Dictionary<int, Ingredient> _ingredients = new Dictionary<int, Ingredient>();
        private static Dictionary<int, Recipe> _recipes = new Dictionary<int, Recipe>();

        private static int _userId = 1;
        private static int _ingredientId = 1;
        private static int _recipeId = 1;

        public int AddUser(User user)
        {
            user.Id = _userId++;
            _users.Add(user.Id, user.Clone());
            return user.Id;
        }

        public User GetUser(int userId)
        {
            User user = null;

            if (_users.ContainsKey(userId))
            {
                user = _users[userId];
            }
            else
            {
                throw new Exception("User does not exist.");
            }

            return user.Clone();
        }

        public User GetUser(string username)
        {
            User user = null;

            foreach (var item in _users)
            {
                if (item.Value.Username == username)
                {
                    user = item.Value;
                    break;
                }
            }

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            return user.Clone();
        }

        public int AddIngredient(Ingredient ingredient)
        {
            ingredient.Id = _ingredientId++;
            _ingredients.Add(ingredient.Id, ingredient.Clone());
            return ingredient.Id;
        }

        public List<Ingredient> GetAllIngredientsByUserId(int userId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var item in _ingredients)
            {
                if (item.Value.UserId == userId)
                {
                    ingredients.Add(item.Value.Clone());
                }
            }
            return ingredients;
        }

        public Ingredient GetIngredientByUserId(int userId)
        {
            Ingredient ingredient = null;

            foreach (var item in _ingredients)
            {
                if (item.Value.UserId == userId)
                {
                    ingredient = item.Value;
                    break;
                }
            }

            if (ingredient == null)
            {
                throw new Exception("Ingredient does not exist.");
            }

            return ingredient.Clone();
        }

        public int AddRecipe(Recipe recipe)
        {
            recipe.Id = _recipeId++;
            _recipes.Add(recipe.Id, recipe.Clone());
            return recipe.Id;
        }

        public List<Recipe> GetAllRecipesByUserId(int userId)
        {
            List<Recipe> recipes = new List<Recipe>();
            foreach (var item in _recipes)
            {
                if (item.Value.CreatorId == userId)
                {
                    recipes.Add(item.Value.Clone());
                }
            }
            return recipes;
        }

        public Recipe GetRecipeByRecipeId(int recipeId)
        {
            Recipe recipe = null;

            foreach (var item in _recipes)
            {
                if (item.Value.Id == recipeId)
                {
                    recipe = item.Value;
                    break;
                }
            }

            if (recipe == null)
            {
                throw new Exception("Recipe does not exist.");
            }

            return recipe.Clone();
        }

        public bool IngredientExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            bool didWork = false;

            return didWork;
        }

        public bool AssignRecipeImage(int recipeId, byte[] image_byte)
        {
            throw new NotImplementedException();
        }

        public bool AssignRecipeIngredient(int RecipeId, int IngredientId, string quantity)
        {
            throw new NotImplementedException();
        }

        RecipeDetailViewModel IMealDBService.GetRecipeByRecipeId(int recipeId)
        {
            throw new NotImplementedException();
        }

        public int GetIngredientIdByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRecipeIngredient(int recipeId, int ingredientId)
        {
            throw new NotImplementedException();
        }

        public List<Meal> GetAllMealsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Meal GetMealByMealId(int mealId)
        {
            throw new NotImplementedException();
        }

        public bool AssignRecipeToMeal(int mealId, int recipeId)
        {
            throw new NotImplementedException();
        }

        public int AddMeal(Meal meal)
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetRecipesByMealId(int mealId)
        {
            throw new NotImplementedException();
        }

        public bool MealExists(string mealName)
        {
            throw new NotImplementedException();
        }

        public List<Plan> GetAllPlansByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Meal> GetMealsByPlanId(int planId)
        {
            throw new NotImplementedException();
        }

        public Plan GetPlanByPlanId(int planId)
        {
            throw new NotImplementedException();
        }

        public int AddPlan(Plan plan)
        {
            throw new NotImplementedException();
        }

        public bool AssignMealToPlan(int planId, int mealId)
        {
            throw new NotImplementedException();
        }

        public bool PlanExists(string planName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRecipeFromMeal(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetAllIngredientsInRecipe(int recipeId)
        {
            throw new NotImplementedException();
        }

        public bool DeletaAllRecipesFromMeal(int mealId)
        {
            throw new NotImplementedException();
        }

        public bool AssignMealToPlan(int planId, int mealId, int dayId)
        {
            throw new NotImplementedException();
        }

        public List<Meal> GetMealsOnDay(int planId, int dayId)
        {
            throw new NotImplementedException();
        }

        public string GetImageByRecipeId(int recipeId)
        {
            throw new NotImplementedException();
        }

        public Meal GetMealByMealName(string name)
        {
            throw new NotImplementedException();
        }
    }
}