using Azure;
using Azure.Data.Tables;
using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.AzureStorage
{
    public class AzureStorageManager : Page
    {
        private TableClient tableClient { get; }

        public AzureStorageManager(IWebDriver driver, string azureConnectionString, string azureTable)
        {
            base.driver = driver;
            tableClient = new TableClient(azureConnectionString, azureTable);
        }

        /// <summary>
        /// Get collection of entities filtered by query on azure storage table. 
        /// </summary>
        /// <param name="queryFilter">FormattableString type query string (i.e. $"ChargeId eq {chargeId}").</param>
        /// <returns>Collection of table entities</returns>
        public Pageable<TableEntity> GetTableEntityByQueryFilter(FormattableString queryFilter, int timeOut = 120, int interval = 100)
        {
            Pageable<TableEntity> tableEntity = null;
            try
            {
                WaitUntil(() =>
                {
                    tableEntity = tableClient.Query<TableEntity>(filter: TableClient.CreateQueryFilter(queryFilter));
                    var keepSessionAlive = driver.Title;
                    return tableEntity != null;
                }, timeOut, interval);

                return tableEntity;
            }
            catch (WebDriverTimeoutException)
            {
                return tableEntity;  //return null for timeout
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while executing query on Azure Storage Table: " + e.Message);
            }
        }

        /// <summary>
        /// Get single table entity filtered by partition key and row key on azure storage table. 
        /// </summary>
        /// <param name="partitionKey">Partitionkey value.</param>
        /// <param name="rowKey">RowKey value.</param>
        /// <returns>Single table entity</returns>
        public TableEntity GetTableEntityByPartitionKeyRowKey(string partitionKey, string rowKey, int timeOut = 120, int interval = 100)
        {
            TableEntity tableEntity = null;
            try
            {
                WaitUntil(() =>
                {
                    tableEntity = tableClient.GetEntity<TableEntity>(partitionKey, rowKey);
                    var keepSessionAlive = driver.Title;
                    return tableEntity != null;
                }, timeOut, interval);

                return tableEntity;
            }
            catch (WebDriverTimeoutException)
            {
                return tableEntity;  //return null for timeout
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while executing query on Azure Storage Table: " + e.Message);
            }
        }

        /// <summary>
        /// Update property value of specific table entity on azure storage table. 
        /// </summary>
        /// <param name="tableEntity">Specific table entity which contains the property to update.</param>
        /// <param name="propertyName">Name of property to be updated.</param>
        /// <param name="propertyValue">Generic type update value for the property (Type depends on specific property of Azure table).</param>
        /// <returns>Single table entity</returns>
        public void UpdateTableEntityByPropertyName<T>(TableEntity tableEntity, string propertyName, T propertyValue)
        {
            tableEntity[propertyName] = propertyValue;
            tableClient.UpdateEntity(tableEntity, tableEntity.ETag);
        }
    }
}
