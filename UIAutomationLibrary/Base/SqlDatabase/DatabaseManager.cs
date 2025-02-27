using Dapper;
using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.SqlDatabase
{
    public class DatabaseManager : Page, IDisposable
    {
        private SqlConnection SqlConnection { get; }

        public DatabaseManager(IWebDriver driver, string sqlConnectionString)
        {
            base.driver = driver;
            SqlConnection = new SqlConnection(sqlConnectionString);
            SqlConnection.Open();
        }

        public void Dispose()
        {
            SqlConnection?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get query result from SQL Server Database. 
        /// </summary>
        /// <param name="sqlQuery">SQL Query to execute for Database.</param>
        /// <param name="param">Optional object type paramer.</param>
        /// <returns>Query result in generic list format containing each row</returns>
        public IList<T> GetDbQueryResult<T>(string sqlQuery, object param = null, int expectedDbRows = 1, int timeoutInSeconds = 300)
        {
            List<T> result = null;
            try
            {
                WaitUntil(() =>
                {
                    result = SqlConnection.Query<T>(sqlQuery, param).ToList();
                    var keepSessionAlive = driver.Title;
                    return result.Count >= expectedDbRows;
                }, timeoutInSeconds, 5000);

                return result;
            }
            catch (WebDriverTimeoutException)
            {
                return result;  //return empty list for timeout
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while executing query on Database: " + e.Message);
            }
        }
    }
}
