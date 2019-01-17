using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Recipe : Base
    {
        #region Properties
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int CookTime { get; set; }
        public int PrepTime { get; set; }
        public int CreatorId { get; set; }
        public string FoodImageBase64 { get; set; }

        #endregion

        public Recipe Clone()
        {
            Recipe recipe = new Recipe();
            recipe.Id = Id;
            recipe.Name = Name;
            recipe.Description = Description;
            recipe.Instructions = Instructions;
            recipe.CookTime = CookTime;
            recipe.PrepTime = PrepTime;
            recipe.CreatorId = CreatorId;
            recipe.FoodImageBase64 = FoodImageBase64;
            return recipe;
        }
    }
}