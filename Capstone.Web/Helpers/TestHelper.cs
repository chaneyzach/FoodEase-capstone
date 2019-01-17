using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using Capstone.Web.Database;
using System.Transactions;

namespace Capstone.Web.Helpers
{
    public static class TestHelper
    {
        public static void PopulateDatabaseWithUsers(IMealDBService db)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                PasswordHelper passHelper = new PasswordHelper("a");

                User user = new User()
                {
                    FirstName = "Mister",
                    LastName = "Pink",
                    Username = "mp",
                    Email = "mp@gmail.com",
                };
                user.Hash = passHelper.Hash;
                user.Salt = passHelper.Salt;
                user.Id = db.AddUser(user);

                scope.Complete();
            }
        }

        public static void PopulateDatabaseWithIngredients(IMealDBService db)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Ingredient ingredient1 = new Ingredient()
                {
                    Name = "butter",
                    UserId = 1
                };
                

                Ingredient ingredient2 = new Ingredient()
                {
                    Name = "flour",
                    UserId = 1
                };

                scope.Complete();
            }
        }

        public static void PopulateDatabaseWithRecipess(IMealDBService db)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Recipe recipe1 = new Recipe()
                {
                    Name = "Tiny Lasagna",
                    Description = "Teeny Tiny Lasagna",
                    Instructions = "Cook very well, with much skill.",
                    CookTime = 45,
                    PrepTime = 25,
                    CreatorId = 1
                };
                db.AddRecipe(recipe1);

                Recipe recipe2 = new Recipe()
                {
                    Name = "Tiny Eggs",
                    Description = "Teeny Tiny Eggs",
                    Instructions = "Cook very well, with much skill.",
                    CookTime = 5,
                    PrepTime = 5,
                    CreatorId = 1
                };
                db.AddRecipe(recipe2);

                scope.Complete();
            }
        }
    }
}