using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class MealDetailViewModel
    {
        public List<Recipe> Recipes { get; set; }
        public List<Recipe> AllRecipes { get; set; }
        public Meal Meal { get; set; }
        public List<string> _categories = new List<string>
        {
            "",
            "Breakfast",
            "Lunch",
            "Dinner",
            "Snack",
            "Dessert"
        };
    }
}