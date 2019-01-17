using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Database;

namespace Capstone.Web.Controllers
{
    public class MealController : BaseController
    {
        private IMealDBService _dal;

        /// <summary>
        /// Getting access to DB
        /// </summary>
        /// <param name="db"></param>
        public MealController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public ActionResult GetMeals()
        {

            List<Meal> meals = new List<Meal>();

            if (IsAuthenticated)
            {
                _nextView = "MealList";
                meals.AddRange(_dal.GetAllMealsByUserId((int)Session[UserKey]));    //add meals in DB to list                
            }

            return GetAuthenticatedView(_nextView, meals);
        }

        [HttpGet]
        public ActionResult GetMeal(int mealId)
        {

            MealDetailViewModel mealDetail = new MealDetailViewModel();

            if (IsAuthenticated)
            {
                _nextView = "MealDetail";
                mealDetail.Meal = _dal.GetMealByMealId(mealId);
                mealDetail.Recipes = _dal.GetRecipesByMealId(mealId);
                foreach (Recipe recipe in mealDetail.Recipes)
                {
                    recipe.FoodImageBase64 = _dal.GetImageByRecipeId(recipe.Id);
                }
            }

            return GetAuthenticatedView(_nextView, mealDetail);
        }

        // GET: Meal
        [HttpGet]
        public ActionResult AddMeal(string name, string desc, int category, int? creator, int? id)
        {
            AddMealViewModel addMeal = new AddMealViewModel();
            
            if (id > 0)
            {
                addMeal.Meal = _dal.GetMealByMealId((int)id);
                addMeal.MealRecipes = _dal.GetRecipesByMealId((int)id);
            }
           
            bool doesExist = _dal.MealExists(name);
            
            if (IsAuthenticated)
            {
                if (!doesExist) //checks if meal exists before adding to DB
                {
                    Meal meal = new Meal();

                    meal.Name = name;
                    meal.Description = desc;
                    meal.UserId = (int)Session[UserKey];
                    meal.CategoryId = category;
                    meal.CreatorId = meal.UserId;
                    meal.Id = _dal.AddMeal(meal);
                    
                    addMeal.Meal = meal;
                }
                else
                {
                    addMeal.Meal = _dal.GetMealByMealName(name);
                }

                addMeal.Recipes = _dal.GetAllRecipesByUserId((int)Session[UserKey]);

                _nextView = "AddMeal";
            }

            return GetAuthenticatedView(_nextView, addMeal);
        }

        // GET: Meal
        [HttpPost]
        public ActionResult AddMeal(int mealId, int recipeId)
        {
            AddMealViewModel addMealView = new AddMealViewModel();

            _dal.AssignRecipeToMeal(mealId, recipeId);
            addMealView.Recipes = _dal.GetAllRecipesByUserId((int)Session[UserKey]);
            addMealView.MealRecipes = _dal.GetRecipesByMealId(mealId);
            addMealView.Meal = _dal.GetMealByMealId(mealId);
            addMealView.Meal.Id = mealId;

            return RedirectToAction("AddMeal", new
            {
                name = addMealView.Meal.Name,
                desc = addMealView.Meal.Description,
                category = addMealView.Meal.CategoryId,
                creator = addMealView.Meal.CreatorId,
                id = (int?)mealId
            });
        }
        

        // GET: Meal
        [HttpGet]
        public ActionResult ModifyMeal(int mealId)
        {
            MealDetailViewModel mealDetail = new MealDetailViewModel();

            mealDetail.Recipes = _dal.GetRecipesByMealId(mealId);
            mealDetail.Meal = _dal.GetMealByMealId(mealId);
            mealDetail.AllRecipes = _dal.GetAllRecipesByUserId((int)Session[UserKey]);

            return View("ModifyMeal", mealDetail);
        }

        [HttpPost]
        public ActionResult ModifyMeal(int id, List<int> recipeIds)
        {
            MealDetailViewModel mealDetail = new MealDetailViewModel();
            List<Recipe> recipes = _dal.GetRecipesByMealId(id);

            _dal.DeletaAllRecipesFromMeal(id);

            foreach (int item in recipeIds)
            {
                if (item != 0)
                {
                    _dal.AssignRecipeToMeal(id, item);
                }
            }

            mealDetail.Recipes = _dal.GetRecipesByMealId(id);
            mealDetail.Meal = _dal.GetMealByMealId(id);

            return RedirectToAction("GetMeal", new { mealId = id });
        }

        // GET: Meal
        [HttpPost]
        public ActionResult DeleteMeal(int id)
        {
            //add DELETE _dal method

            return RedirectToAction("GetMeals");
        }
    }
}