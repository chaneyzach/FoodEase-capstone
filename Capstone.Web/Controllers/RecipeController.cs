using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Helpers;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Models;
using Capstone.Web.Database;
using System.IO;

namespace Capstone.Web.Controllers
{
    public class RecipeController : BaseController
    {
        private IMealDBService _dal;

        public object recipes { get; private set; }

        public RecipeController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// Get all recipes by user ID for the recipe list page
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();

            if (IsAuthenticated)
            {
                _nextView = "RecipeList";
                recipes.AddRange(_dal.GetAllRecipesByUserId((int)Session[UserKey]));    //add recipes in DB to list
                foreach (Recipe recipe in recipes)
                {
                    recipe.FoodImageBase64 = _dal.GetImageByRecipeId(recipe.Id);
                }
            }

            return GetAuthenticatedView(_nextView, recipes);
        }

        /// <summary>
        /// Get single recipe with ingredients/quantities for the recipe detail page
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRecipe(int recipeId)
        {
            RecipeDetailViewModel recipe = new RecipeDetailViewModel();
            if (IsAuthenticated)
            {
                _nextView = "RecipeDetail";
                recipe = _dal.GetRecipeByRecipeId(recipeId);
                recipe.Recipe.FoodImageBase64 = _dal.GetImageByRecipeId(recipeId);
            }

            return GetAuthenticatedView(_nextView, recipe);
        }

        [HttpGet]
        public ActionResult AddRecipe()
        {
            if (IsAuthenticated)
            {
                _nextView = "AddRecipe";
            }

            return GetAuthenticatedView(_nextView);
        }

        [HttpPost]
        public ActionResult AddNewRecipe(Recipe recipe, List<string> ingredients, List<string> quantities, HttpPostedFileBase FoodImage)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                recipe.CreatorId = (int)Session[UserKey];

                recipe.Id = _dal.AddRecipe(recipe);

                if (FoodImage != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        FoodImage.InputStream.CopyTo(ms);
                        byte[] imageBytes = ms.GetBuffer();
                        _dal.AssignRecipeImage(recipe.Id, imageBytes);
                    }
                    recipe.FoodImageBase64 = _dal.GetImageByRecipeId(recipe.Id);
                }

                for (int i = 0; i < ingredients.Count; i++)
                {
                    try
                    {
                        bool doesExist = _dal.IngredientExists(ingredients[i]);
                        int newIngredientId = 0;

                        if (ingredients[i].Length > 0)
                        {
                            Ingredient newIngredient = new Ingredient()
                            {
                                Name = ingredients[i],
                                UserId = (int)Session[UserKey]
                            };
                            if (!doesExist)
                            {
                                newIngredientId = _dal.AddIngredient(newIngredient);
                            }
                            else
                            {
                                newIngredientId = _dal.GetIngredientIdByName(newIngredient.Name);
                            }

                            _dal.AssignRecipeIngredient(recipe.Id, newIngredientId, quantities[i]);
                        }

                    }
                    catch (Exception)
                    {
                    }
                }

                TempData["RecipeSuccess"] = "Recipe added!";
                result = RedirectToAction("GetRecipe", new { recipeId = recipe.Id });
            }

            catch (Exception)
            {
                TempData["RecipeFail"] = "Error: Recipe not added!";
                result = RedirectToAction("GetRecipes");
            }

            return result;
        }

        [HttpGet]
        public ActionResult ModifyRecipe(int recipeId)
        {
            RecipeDetailViewModel recipe = _dal.GetRecipeByRecipeId(recipeId);

            if (IsAuthenticated)
            {
                _nextView = "ModifyRecipe";
            }

            return GetAuthenticatedView(_nextView, recipe);
        }

        [HttpPost]
        public ActionResult ModifyRecipe(Recipe recipe, List<string> ingredients, List<string> quantities, HttpPostedFileBase FoodImage)
        {
            ActionResult result = null;
            List<Ingredient> ingredientsInRecipe = _dal.GetAllIngredientsInRecipe(recipe.Id);

            foreach (Ingredient ingredient in ingredientsInRecipe)
            {
                _dal.DeleteRecipeIngredient(recipe.Id, ingredient.Id);
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                bool didUpdate = _dal.UpdateRecipe(recipe);
                int ingredientId = 0;

                for (int i = 0; i < ingredients.Count; i++)
                {
                    ingredientId = _dal.GetIngredientIdByName(ingredients[i]);
                    _dal.DeleteRecipeIngredient(recipe.Id, ingredientId);
                }

                for (int i = 0; i < ingredients.Count; i++)
                {
                    try
                    {
                        bool doesExist = _dal.IngredientExists(ingredients[i]);
                        int newIngredientId = 0;

                        if (ingredients.Count > 0)
                        {
                            Ingredient newIngredient = new Ingredient()
                            {
                                Name = ingredients[i],
                                UserId = (int)Session[UserKey]
                            };
                            if (!doesExist)
                            {
                                newIngredientId = _dal.AddIngredient(newIngredient);
                            }
                            else
                            {
                                newIngredientId = _dal.GetIngredientIdByName(newIngredient.Name);
                            }

                            _dal.AssignRecipeIngredient(recipe.Id, newIngredientId, quantities[i]);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (FoodImage != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        FoodImage.InputStream.CopyTo(ms);
                        byte[] imageBytes = ms.GetBuffer();
                        _dal.AssignRecipeImage(recipe.Id, imageBytes);
                    }
                    
                }

                TempData["RecipeSuccess"] = "Recipe added!";
                result = RedirectToAction("GetRecipe", new { recipeId = recipe.Id });
            }

            catch (Exception)
            {
                TempData["RecipeFail"] = "Error: Recipe not added!";
                result = RedirectToAction("GetRecipes");
            }

            return result;
        }

        [HttpGet]
        public ActionResult DeleteRecipe(int recipeId)
        {
            RecipeDetailViewModel recipeDetail = _dal.GetRecipeByRecipeId(recipeId);

            for (int i = 0; i < recipeDetail.Ingredients.Count; i++)
            {
                _dal.DeleteRecipeIngredient(recipeDetail.Recipe.Id, recipeDetail.Ingredients[i].Id);
            }

            //_dal.DeleteRecipeFromMeal(recipeDetail.Recipe); **redundant after DeleteRecipe being updated**
            _dal.DeleteRecipe(recipeDetail.Recipe);

            return RedirectToAction("GetRecipes");
        }
    }
}