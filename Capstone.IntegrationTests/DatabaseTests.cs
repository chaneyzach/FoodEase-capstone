using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Web.Database;
using Capstone.Web.Mock;
using System.Transactions;
using Capstone.Web.Helpers;
using Capstone.Web.Models;
using System.Collections.Generic;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class DatabaseTests
    {
        //Used to begin a transaction during initialize and rollback during cleanup
        private TransactionScope _tran;
        //private IMealDBService _db = new MealDBService();
        private IMealDBService _db = new MockMealDBService();

        private int _recipe1Id = 0;
        private User _user = new User();
        private Recipe _recipe1 = new Recipe();
        private Recipe _recipe2 = new Recipe();
        private List<Recipe> _recipes = new List<Recipe>();

        [TestInitialize]
        public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            _tran = new TransactionScope();

            PasswordHelper passHelper = new PasswordHelper("Abcd!234");
            if (_user.Id == Base.InvalidId)
            {
                // Add temp user
                _user.Id = 2;
                _user.FirstName = "Mister";
                _user.LastName = "White";
                _user.Username = "mw";
                _user.Hash = passHelper.Hash;
                _user.Salt = passHelper.Salt;
                _user.Email = "mw@gmail.com";
                _db.AddUser(_user);

            }
            if (_recipe1.Id == Base.InvalidId)
            {
                // Add temp recipes
                _recipe1.Name = "Tiny Pancakes";
                _recipe1.Description = "Teeny tiny pancakes";
                _recipe1.Instructions = "Cook very well, with skill.";
                _recipe1.CookTime = 8;
                _recipe1.PrepTime = 10;
                _recipe1.CreatorId = 2;
                _recipes.Add(_recipe1);
                _recipe1Id = _db.AddRecipe(_recipe1);
            }
            if (_recipe2.Id == Base.InvalidId)
            {
                _recipe2.Name = "Tiny Steak";
                _recipe2.Description = "Teeny tiny steak";
                _recipe2.Instructions = "Cook very well, with skill.";
                _recipe2.CookTime = 7;
                _recipe2.PrepTime = 5;
                _recipe2.CreatorId = 2;
                _recipes.Add(_recipe2);
                _db.AddRecipe(_recipe2);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
            _user = new User();
            _recipe1 = new Recipe();
            _recipe2 = new Recipe();
            _recipes.Clear();
    }

        [TestMethod]
        public void UserTest()
        {
            // Test get user by Id
            User userGetById = _db.GetUser(_user.Id);
            Assert.AreEqual(_user.Id, userGetById.Id);
            Assert.AreEqual(_user.FirstName, userGetById.FirstName);
            Assert.AreEqual(_user.LastName, userGetById.LastName);
            Assert.AreEqual(_user.Username, userGetById.Username);
            Assert.AreEqual(_user.Hash, userGetById.Hash);
            Assert.AreEqual(_user.Salt, userGetById.Salt);
            Assert.AreEqual(_user.Email, userGetById.Email);

            // Test get user by username
            User userGetByName = _db.GetUser(_user.Username);
            Assert.AreEqual(_user.Id, userGetByName.Id);
            Assert.AreEqual(_user.FirstName, userGetByName.FirstName);
            Assert.AreEqual(_user.LastName, userGetByName.LastName);
            Assert.AreEqual(_user.Username, userGetByName.Username);
            Assert.AreEqual(_user.Hash, userGetByName.Hash);
            Assert.AreEqual(_user.Salt, userGetByName.Salt);
            Assert.AreEqual(_user.Email, userGetByName.Email);
        }

        [TestMethod]
        public void RecipeTest()
        {
            // Test get all recipes by user Id
            List<Recipe> recipes = _db.GetAllRecipesByUserId(_user.Id);

            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(_recipes[i].Id, recipes[i].Id);
                Assert.AreEqual(_recipes[i].Name, recipes[i].Name);
                Assert.AreEqual(_recipes[i].Description, recipes[i].Description);
                Assert.AreEqual(_recipes[i].Instructions, recipes[i].Instructions);
                Assert.AreEqual(_recipes[i].CookTime, recipes[i].CookTime);
                Assert.AreEqual(_recipes[i].PrepTime, recipes[i].PrepTime);
                Assert.AreEqual(_recipes[i].CreatorId, recipes[i].CreatorId);
            }

            // Test get recipe by recipe Id
            //Recipe recipe = _db.GetRecipeByRecipeId(_recipe1Id);

            //Assert.AreEqual(_recipe1.Id, recipe.Id);
            //Assert.AreEqual(_recipe1.Name, recipe.Name);
            //Assert.AreEqual(_recipe1.Description, recipe.Description);
            //Assert.AreEqual(_recipe1.Instructions, recipe.Instructions);
            //Assert.AreEqual(_recipe1.CookTime, recipe.CookTime);
            //Assert.AreEqual(_recipe1.PrepTime, recipe.PrepTime);
            //Assert.AreEqual(_recipe1.CreatorId, recipe.CreatorId);
        }
    }
}
