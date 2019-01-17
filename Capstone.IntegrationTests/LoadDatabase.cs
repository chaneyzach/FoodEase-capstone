using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Web.Helpers;
using Capstone.Web.Database;
using Capstone.Web.Mock;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class LoadDatabase
    {
        [TestMethod]
        public void PopulateDatabase()
        {
            //IVendingService db = new VendingDBService();
            IMealDBService db = new MockMealDBService();

            TestHelper.PopulateDatabaseWithUsers(db);
        }
    }
}
