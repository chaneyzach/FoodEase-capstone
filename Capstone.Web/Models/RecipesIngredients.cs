using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class RecipesIngredients
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Notes { get; set; }
        public string Qty { get; set; }
    }
}