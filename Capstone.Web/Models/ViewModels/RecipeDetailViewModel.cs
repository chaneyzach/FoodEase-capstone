using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class RecipeDetailViewModel
    {
        public Recipe Recipe { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Quantities { get; set; } = new List<string>();

    }
}