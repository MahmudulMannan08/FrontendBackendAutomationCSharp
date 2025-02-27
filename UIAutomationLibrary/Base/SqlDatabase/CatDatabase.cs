using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Base.SqlDatabase;

namespace MarketPlaceWeb.Base.SqlDatabase
{
    public class CatDatabase
    {
        DatabaseManager databaseManager;

        public CatDatabase(IWebDriver driver, string statisticDbConnectionString)
        {
            databaseManager = new DatabaseManager(driver, statisticDbConnectionString);
        }

        public IList<string> GetVehicleId(string catId, int expectedDbRows = 1, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT VehicleID FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID", new { CatID = catId }, expectedDbRows: expectedDbRows, timeoutInSeconds: timeoutInSeconds);
        }

        public IList<string> GetVehicleModel(string catId, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT Model FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID", new { CatID = catId }, timeoutInSeconds: timeoutInSeconds);
        }

        public IList<string> GetVehicleTrim(string catId, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT Trim FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID", new { CatID = catId }, timeoutInSeconds: timeoutInSeconds);
        }

        public IList<string> GetVehicleVin(string catId, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT VIN FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID", new { CatID = catId }, timeoutInSeconds: timeoutInSeconds);
        }

        public IList<string> GetVehicleMileage(string catId, int timeoutInSeconds = 300)
        {
            const string query = @"SELECT Mileage FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID";
            return databaseManager.GetDbQueryResult<string>(query, new { CatID = catId }, timeoutInSeconds: timeoutInSeconds);
        }
        public IList<string> GetVehicleColor(string catId, int timeoutInSeconds = 300)
        {
            const string query = @"SELECT Color FROM [MyV].[MyVehicle] WITH (NOLOCK) WHERE CatID = @CatID";
            return databaseManager.GetDbQueryResult<string>(query, new { CatID = catId }, timeoutInSeconds: timeoutInSeconds);
        }

        public void Dispose()
        {
            databaseManager?.Dispose();
        }
    }
}
