using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.AzureStorage
{
    public class TdrCatQaStorage
    {
        AzureStorageManager azureStorageManager;

        public TdrCatQaStorage(IWebDriver driver, string qatdrnotificationStorageConnectionString, string qatdrnotificationTableName)
        {
            azureStorageManager = new AzureStorageManager(driver, qatdrnotificationStorageConnectionString, qatdrnotificationTableName);
        }

        public string GetRowKeyByEmail(string email)
        {
            var catDeleteRequest = azureStorageManager.GetTableEntityByQueryFilter($"email eq {email}");

            try
            {
                //If emailIndexTable has more more than one entry , report to dev team
                return catDeleteRequest.Single().GetString("RowKey");
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
