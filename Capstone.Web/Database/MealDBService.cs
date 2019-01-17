using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Database
{
    public class MealDBService : IMealDBService
    {
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;

        public MealDBService()
        {
            
        }

        public MealDBService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddUser(User user)
        {
            const string sql = "INSERT Users (firstname, lastname, username, email, hash, salt) " +
                               "VALUES (@firstName, @lastName, @userName, @email, @hash, @salt);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);
                cmd.Parameters.AddWithValue("@userName", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@hash", user.Hash);
                cmd.Parameters.AddWithValue("@salt", user.Salt);

                user.Id = (int)cmd.ExecuteScalar();
            }

                return user.Id;
        }

        public User GetUser(int userId)
        {
            User user = null;
            const string sql = "SELECT * FROM Users WHERE Users.user_id = @userId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = GetUserFromReader(reader);
                }

                if (user == null)
                {
                    throw new Exception("User does not exist.");
                }
            }

            return user;
        }

        public User GetUser(string username)
        {
            User user = null;
            const string sql = "SELECT * FROM Users WHERE Users.username = @userName;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", username);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = GetUserFromReader(reader);
                }

                if (user == null)
                {
                    throw new Exception("User does not exist.");
                }
            }

            return user;
        }

        private User GetUserFromReader(SqlDataReader reader)
        {
            User user = new User();

            user.Id = Convert.ToInt32(reader["user_id"]);
            user.FirstName = Convert.ToString(reader["firstname"]);
            user.LastName = Convert.ToString(reader["lastname"]);
            user.Username = Convert.ToString(reader["username"]);
            user.Email = Convert.ToString(reader["email"]);
            user.Salt = Convert.ToString(reader["salt"]);
            user.Hash = Convert.ToString(reader["hash"]);

            return user;
        }

        public List<Ingredient> GetAllIngredientsByUserId(int userId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            const string sql = "SELECT * FROM Ingredients WHERE user_id = @userId ORDER BY ingredient_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ingredients.Add(GetIngredientFromReader(reader));
                }
            }

            return ingredients;
        }

        public List<Ingredient> GetAllIngredientsInRecipe(int recipeId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            const string sql = "SELECT * FROM Ingredients " +
                               "INNER JOIN RecipesIngredients ON Ingredients.ingredient_id = RecipesIngredients.ingredient_id " +
                               "WHERE RecipesIngredients.recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ingredients.Add(GetIngredientFromReader(reader));
                }
            }

            return ingredients;
        }
       
        public int AddIngredient(Ingredient ingredient)
        {
            const string sql = "INSERT INTO Ingredients (ingredient_name, user_id) VALUES (@ingredientName, @userId);";
            int ingredientId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@ingredientName", ingredient.Name);
                cmd.Parameters.AddWithValue("@userId", ingredient.UserId);

                ingredientId = (int)cmd.ExecuteScalar();
            }

            return ingredientId;
        }

        public bool IngredientExists(string name)
        {
            bool doesExist = false;
            int scalar = 0;
            const string sql = "SELECT COUNT(*) FROM Ingredients WHERE ingredient_name = @name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name.ToLower());

                scalar = (int)cmd.ExecuteScalar();

                if (scalar != 0)
                {
                    doesExist = true;
                }
            }

                return doesExist;
        }

        public int GetIngredientIdByName(string name)
        {
            int ingredientId = 0;
            string sql = "SELECT ingredient_id FROM Ingredients WHERE ingredient_name = @name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);

                ingredientId = Convert.ToInt32(cmd.ExecuteScalar());
            }

                return ingredientId;
        }

        private Ingredient GetIngredientFromReader(SqlDataReader reader)
        {
            Ingredient ingredient = new Ingredient();

            ingredient.Id = Convert.ToInt32(reader["ingredient_id"]);
            ingredient.Name = Convert.ToString(reader["ingredient_name"]);
            ingredient.UserId = Convert.ToInt32(reader["user_id"]);            

            return ingredient;
        }

        public List<Recipe> GetAllRecipesByUserId(int userId)
        {
            List<Recipe> recipes = new List<Recipe>();
            string sql = "SELECT * FROM Recipes WHERE creator_id = @userId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(GetRecipeFromReader(reader));
                }
            }

            return recipes;
        }

        public string GetImageByRecipeId(int recipeId)
        {
            string image = "";

            string sql = "SELECT * FROM Images " +
                         "INNER JOIN Recipes ON Recipes.recipe_id = Images.recipe_id " +
                         "WHERE Recipes.recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    image = Convert.ToBase64String((byte[])reader["image_byte"]);
                }

            }

            return image;
        }

        public RecipeDetailViewModel GetRecipeByRecipeId(int recipeId)
        {
            RecipeDetailViewModel recipeDetail = new RecipeDetailViewModel();
            Ingredient ingredient = new Ingredient();

            string sql = "SELECT * FROM Recipes " +
                         "INNER JOIN RecipesIngredients ON Recipes.recipe_id = RecipesIngredients.recipe_id " +
                         "INNER JOIN Ingredients ON RecipesIngredients.ingredient_id = Ingredients.ingredient_id " +
                         "WHERE Recipes.recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipeDetail.Recipe = GetRecipeFromReader(reader);
                    recipeDetail.Ingredients.Add(ingredient = GetIngredientFromReader(reader));
                    recipeDetail.Quantities.Add(Convert.ToString(reader["quantity"]));
                }
                
            }

            return recipeDetail;
        }

        public int AddRecipe(Recipe recipe)
        {
            int recipeId = 0;
            string sql = "INSERT INTO Recipes (recipe_name, description, instructions, cook_time, prep_time, creator_id) " +
                         "VALUES (@recipeName, @description, @instructions, @cookTime, @prepTime, @creatorId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@recipeName", recipe.Name);
                cmd.Parameters.AddWithValue("@description", recipe.Description);
                cmd.Parameters.AddWithValue("@instructions", recipe.Instructions);
                cmd.Parameters.AddWithValue("@cookTime", recipe.CookTime);
                cmd.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                cmd.Parameters.AddWithValue("@creatorId", recipe.CreatorId);

                recipeId = (int)cmd.ExecuteScalar();
            }
            
            return recipeId;
        }

        public bool AssignRecipeIngredient(int recipeId, int ingredientId, string quantity)
        {
            bool added = false;
            int rowsAffected = 0;
            string sql = "INSERT into RecipesIngredients (recipe_id, ingredient_id, quantity) " +
                          "VALUES(@recipeId, @ingredientId, @quantity);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@ingredientId", ingredientId);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    added = true;
                }
            }

            return added;
        }

        public bool AssignRecipeImage(int recipeId, byte[] image_byte)
        {
            bool added = false;
            int rowsAffected = 0;
            string sql = "INSERT into Images (recipe_id, image_byte) " +
                          "VALUES(@recipeId, @image_byte);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@image_byte", image_byte);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    added = true;
                }
            }

            return added;
        }

        public bool DeleteRecipeIngredient(int recipeId, int ingredientId)
        {
            bool deleted = false;
            int rowsAffected = 0;
            string sql = "DELETE FROM RecipesIngredients WHERE recipe_id = @recipeId AND ingredient_id = @ingredientId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@ingredientId", ingredientId);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    deleted = true;
                }
            }

            return deleted;
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            bool updated = false;
            int rowsAffected = 0;
            string sql = "UPDATE Recipes SET recipe_name = @name, description = @description, " +
                         "instructions = @instructions, cook_time = @cookTime, prep_time = @prepTime WHERE recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@name", recipe.Name);
                cmd.Parameters.AddWithValue("@description", recipe.Description);
                cmd.Parameters.AddWithValue("@instructions", recipe.Instructions);
                cmd.Parameters.AddWithValue("@cookTime", recipe.CookTime);
                cmd.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                cmd.Parameters.AddWithValue("@recipeId", recipe.Id);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    updated = true;
                }
            }

            return updated;
        }

        public bool DeleteRecipe(Recipe recipe)
        {
            bool deleted = false;
            int rowsAffected = 0;
            string sql = "DELETE MealsRecipes FROM MealsRecipes " +
                         "INNER JOIN Recipes on Recipes.recipe_id = MealsRecipes.recipe_id " +
                         "WHERE MealsRecipes.recipe_id = @recipeId; " +
                         "DELETE RecipesIngredients from RecipesIngredients " +
                         "INNER JOIN Recipes on Recipes.recipe_id = RecipesIngredients.recipe_id " +
                         "WHERE RecipesIngredients.recipe_id = @recipeId; " +
                         "DELETE Images FROM Images " +
                         "INNER JOIN Recipes on Recipes.recipe_id = Images.recipe_id " +
                         "WHERE Images.recipe_id = @recipeId; " +
                         "DELETE FROM Recipes " +
                         "WHERE recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipe.Id);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    deleted = true;
                }
            }

            return deleted;
        }

        public bool DeleteRecipeFromMeal(Recipe recipe)
        {
            bool deleted = false;
            int rowsAffected = 0;
            string sql = "DELETE Recipes FROM Recipes " +
                         "INNER JOIN MealsRecipes ON Recipes.recipe_id = MealsRecipes.recipe_id " +
                         "WHERE Recipes.recipe_id = @recipeId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipe.Id);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    deleted = true;
                }
            }

            return deleted;
        }

        private Recipe GetRecipeFromReader(SqlDataReader reader)
        {
            Recipe recipe = new Recipe();

            recipe.Id = Convert.ToInt32(reader["recipe_id"]);
            recipe.Name = Convert.ToString(reader["recipe_name"]);
            recipe.Description = Convert.ToString(reader["description"]);
            recipe.Instructions = Convert.ToString(reader["instructions"]);
            recipe.CookTime = Convert.ToInt32(reader["cook_time"]);
            recipe.PrepTime = Convert.ToInt32(reader["prep_time"]);
            recipe.CreatorId = Convert.ToInt32(reader["creator_id"]);
            
            return recipe;
        }

        public List<Meal> GetAllMealsByUserId(int userId)
        {
            List<Meal> meals = new List<Meal>();
            const string sql = "SELECT * FROM Meals WHERE user_id = @userId ORDER BY meal_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meals.Add(GetMealFromReader(reader));
                }
            }

            return meals;
        }

        public Meal GetMealByMealId(int mealId)
        {
            Meal meal = new Meal();
            const string sql = "SELECT * FROM Meals WHERE meal_id = @mealId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meal = GetMealFromReader(reader);
                }
            }

            return meal;
        }

        public Meal GetMealByMealName(string name)
        {
            Meal meal = new Meal();
            const string sql = "SELECT * FROM Meals WHERE meal_name = @mealName;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealName", name);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meal = GetMealFromReader(reader);
                }
            }

            return meal;
        }

        public List<Recipe> GetRecipesByMealId(int mealId)
        {
            List<Recipe> recipes = new List<Recipe>();
            const string sql = "SELECT * FROM Recipes " +
                               "INNER JOIN MealsRecipes ON Recipes.recipe_id = MealsRecipes.recipe_id " +
                               "INNER JOIN Meals ON MealsRecipes.meal_id = Meals.meal_id " +
                               "WHERE Meals.meal_id = @mealId ORDER BY recipe_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(GetRecipeFromReader(reader));
                }
            }

            return recipes;
        }

        public int AddMeal(Meal meal)
        {
            const string sql = "INSERT INTO Meals (meal_name, meal_description, user_id, category_id, creator_id) " +
                               "VALUES (@mealName, @mealDesc, @userId, @categoryId, @creatorId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@mealName", meal.Name);
                cmd.Parameters.AddWithValue("@mealDesc", meal.Description);
                cmd.Parameters.AddWithValue("@userId", meal.UserId);
                cmd.Parameters.AddWithValue("@categoryId", meal.CategoryId);
                cmd.Parameters.AddWithValue("@creatorId", meal.CreatorId);

                meal.Id = (int)cmd.ExecuteScalar();
            }

            return meal.Id;
        }

        public bool AssignRecipeToMeal(int mealId, int recipeId)
        {
            bool added = false;
            int rowsAffected = 0;
            string sql = "INSERT into MealsRecipes (meal_id, recipe_id) " +
                          "VALUES (@mealId, @recipeId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    added = true;
                }
            }

            return added;
        }

        public bool MealExists(string mealName)
        {
            bool doesExist = false;
            int scalar = 0;
            const string sql = "SELECT COUNT(*) FROM Meals WHERE meal_name = @mealName;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealName", mealName);

                scalar = (int)cmd.ExecuteScalar();

                if (scalar != 0)
                {
                    doesExist = true;
                }
            }

            return doesExist;
        }

        public bool DeletaAllRecipesFromMeal(int mealId)
        {
            bool deleted = false;
            int rowsAffected = 0;
            string sql = "DELETE FROM MealsRecipes " +
                         "WHERE meal_id = @mealId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    deleted = true;
                }
            }

            return deleted;
        }

        //public bool DeleteMeal(int mealId)
        //{
        //    bool deleted = false;
        //    int rowsAffected = 0;

        //    if (rowsAffected > 0)
        //    {
        //        deleted = true;
        //    }

        //    return deleted;
        //}

        #region Meal Plan Methods

        private Meal GetMealFromReader(SqlDataReader reader)
        {
            Meal meal = new Meal();

            meal.Id = Convert.ToInt32(reader["meal_id"]);
            meal.Name = Convert.ToString(reader["meal_name"]);
            meal.Description = Convert.ToString(reader["meal_description"]);
            meal.UserId = Convert.ToInt32(reader["user_id"]);
            meal.CategoryId = Convert.ToInt32(reader["category_id"]);
            meal.CreatorId = Convert.ToInt32(reader["creator_id"]);

            return meal;
        }

        public List<Plan> GetAllPlansByUserId(int userId)
        {
            List<Plan> plans = new List<Plan>();
            const string sql = "SELECT * FROM Plans WHERE user_id = @userId ORDER BY plan_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    plans.Add(GetPlanFromReader(reader));
                }
            }

            return plans;
        }

        public List<Meal> GetMealsByPlanId(int planId)
        {
            List<Meal> meals = new List<Meal>();
            const string sql = "SELECT * FROM Meals " +
                   "INNER JOIN MealsPlans ON Meals.meal_id = MealsPlans.meal_id " +
                   "INNER JOIN Plans ON MealsPlans.plan_id = Plans.plan_id " +
                   "WHERE MealsPlans.plan_id = @planId ORDER BY meal_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@planId", planId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meals.Add(GetMealFromReader(reader));
                }
            }

            return meals;
        }

        private Plan GetPlanFromReader(SqlDataReader reader)
        {
            Plan plan = new Plan();

            plan.PlanId = Convert.ToInt32(reader["plan_id"]);
            plan.PlanName = Convert.ToString(reader["plan_name"]);
            plan.UserId = Convert.ToInt32(reader["user_id"]);

            return plan;
        }

        public Plan GetPlanByPlanId(int planId)
        {
            Plan plan = new Plan();
            const string sql = "SELECT * FROM Plans WHERE plan_id = @planId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@planId", planId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    plan = GetPlanFromReader(reader);
                }
            }

            return plan;
        }

        public int AddPlan(Plan plan)
        {
            const string sql = "INSERT INTO Plans (plan_name, user_id) " +
                               "VALUES (@planName, @userId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@planName", plan.PlanName);
                cmd.Parameters.AddWithValue("@userId", plan.UserId);

                plan.Id = (int)cmd.ExecuteScalar();
                plan.PlanId = plan.Id;
            }

            return plan.Id;
        }

        public bool AssignMealToPlan(int planId, int mealId, int dayId)
        {
            bool added = false;
            int rowsAffected = 0;
            string sql = "INSERT into MealsPlans (plan_id, meal_id, day_id) " +
                          "VALUES (@planId, @mealId, @dayId);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@planId", planId);
                cmd.Parameters.AddWithValue("@mealId", mealId);
                cmd.Parameters.AddWithValue("@dayId", dayId);

                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    added = true;
                }
            }

            return added;
        }

        public List<Meal> GetMealsOnDay(int planId, int dayId)
        {
            List<Meal> meals = new List<Meal>();
            const string sql = "SELECT * FROM Meals " +
                   "INNER JOIN MealsPlans ON Meals.meal_id = MealsPlans.meal_id " +
                   "WHERE MealsPlans.plan_id = @planId AND MealsPlans.day_id = @dayId ORDER BY meal_name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@planId", planId);
                cmd.Parameters.AddWithValue("@dayId", dayId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meals.Add(GetMealFromReader(reader));
                }
            }

            return meals;
        }

        public bool PlanExists(string planName)
        {
            bool doesExist = false;
            int scalar = 0;
            const string sql = "SELECT COUNT(*) FROM Plans WHERE plan_name = @planName;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@planName", planName);

                scalar = (int)cmd.ExecuteScalar();

                if (scalar != 0)
                {
                    doesExist = true;
                }
            }

            return doesExist;
        }
        #endregion

    }
}