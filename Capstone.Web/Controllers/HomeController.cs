using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Helpers;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Database;

namespace Capstone.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IMealDBService _dal;

        /// <summary>
        /// Getting access to DB
        /// </summary>
        /// <param name="db"></param>
        public HomeController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            ActionResult result = View("Index");
            if (IsAuthenticated)
            {
                result = RedirectToAction("GetRecipes", "Recipe");
            }
            return result;
        }
    }
}