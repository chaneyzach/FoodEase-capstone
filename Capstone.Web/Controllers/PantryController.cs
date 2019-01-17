using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Helpers;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Database;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class PantryController : BaseController
    {
        private IMealDBService _dal;

        public PantryController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        // GET: Pantry
        [HttpGet]
        public ActionResult Index()
        {
            List<Ingredient> ingredients = null;
            if (IsAuthenticated)
            {
                _nextView = "Pantry";
                ingredients = _dal.GetAllIngredientsByUserId((int)Session[UserKey]);
                //return View("Pantry", ingredients);
            }
            return GetAuthenticatedView(_nextView, ingredients);
        }

        [HttpGet]
        public ActionResult AddNewIngredient()
        {
            if (IsAuthenticated)
            {
                _nextView = "Pantry";
            }
            return GetAuthenticatedView(_nextView);
            //return View("Pantry");      
        }

        [HttpPost]
        public ActionResult AddNewIngredient(List<string> ingredients)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                foreach (string item in ingredients)
                {
                    try
                    {
                        bool doesExist = _dal.IngredientExists(item);
                        if (!doesExist && item.Length > 0)
                        {
                            Ingredient newIngredient = new Ingredient()
                            {
                                Name = item,
                                UserId = (int)Session[UserKey]
                            };
                            _dal.AddIngredient(newIngredient);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                List<Ingredient> ingredientList = new List<Ingredient>();

                ingredientList = _dal.GetAllIngredientsByUserId((int)Session[UserKey]);

                TempData["StatusMessage"] = "Ingredients added!";
                result = RedirectToAction("Index", "Pantry", ingredientList);
            }
            catch (Exception)
            {
                result = View("Pantry");
            }

            return result;
         }
    }
}