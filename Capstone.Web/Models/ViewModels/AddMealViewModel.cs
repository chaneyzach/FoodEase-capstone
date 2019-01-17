using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class AddMealViewModel
    {
        public List<Recipe> Recipes { get; set; }
        public List<Recipe> MealRecipes { get; set; }
        public Meal Meal { get; set; }
    }
}