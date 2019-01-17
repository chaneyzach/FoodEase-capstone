using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Ingredient : Base
    {
        #region Properties
        
        public string Name { get; set; }
        public int UserId { get; set; }

        #endregion

        public Ingredient Clone()
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Id = Id;
            ingredient.Name = Name;
            ingredient.UserId = UserId;
            return ingredient;
        }
    }
}