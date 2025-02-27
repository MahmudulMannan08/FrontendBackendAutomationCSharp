using Azure.Data.Tables;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace UIAutomationLibrary.Base.AzureStorage
{
    public class QatdrNotificationStorage
    {
        AzureStorageManager azureStorageManager;

        public QatdrNotificationStorage(IWebDriver driver, string qatdrnotificationStorageConnectionString, string qatdrnotificationTableName)
        {
            azureStorageManager = new AzureStorageManager(driver, qatdrnotificationStorageConnectionString, qatdrnotificationTableName);
        }

        #region VdpPriceAlert
        public string GetEmailIndexRowKeyByPartitionKey(string email)
        {
            var emailIndexTableEntity = azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {email.ToLower()}");

            try
            {
                //If emailIndexTable has more more than one entry , report to dev team
                return emailIndexTableEntity.Single().GetString("RowKey");
            }
            catch (Exception)
            {
                return default;
            }
        }

        public TableEntity GetProfileTableEntityByPartitionKeyRowKey(string profileId, string rowKey = "_Header")
        {
            return azureStorageManager.GetTableEntityByPartitionKeyRowKey(profileId, rowKey);
        }

        public TableEntity GetSavedAdIndexTableEntityByPartitionKeyRowKey(string adId, string profileId, bool isPrivateAd = false)
        {
            try
            {
                return azureStorageManager.GetTableEntityByPartitionKeyRowKey(isPrivateAd != true ? "5-" + adId : "19-" + adId, profileId);
            }
            catch (Exception)
            {
                return null;
            }
        }
      
        public string GetProfileTableCatId(string profileId)
        {
            var profileTableEntity = azureStorageManager.GetTableEntityByPartitionKeyRowKey(profileId, "_Header");

            try
            {
                return profileTableEntity.GetString("CatId");
            }
            catch (Exception)
            {
                return default;
            }
        }

        public TableEntity GetProfileTableEntityForPriceAlertSavedVehicle(string profileId, string adId, bool isPrivateAd = false)
        {
            try
            {
                return GetProfileTableEntityByPartitionKeyRowKey(profileId, "SV_" + (isPrivateAd != true ? "5-" + adId : "19-" + adId));
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region SavedSearch
        public string GetSavedSearchCriteriaByPartitionKey(string profileId, DateTime startTime)
        {
            var savedSearchTableEntity = azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {profileId} and Timestamp ge {startTime}");
            try
            {
                return savedSearchTableEntity.Single().GetString("Criteria");               
            }
            catch (Exception)
            {
                return default;
            }
        }

        public TableEntity GetSavedSearchIndexTableEntityByPartitionKeyRowKey(string profileId, DateTime startTime)
        {
            try
            {
                return azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {profileId} and Timestamp ge {startTime}").Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetSavedSearchIndexTableRowKey(string profileId, DateTime startTime)
        {
            var savedSearchTableEntity = azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {profileId} and Timestamp ge {startTime}");
            try
            {
                return savedSearchTableEntity.Single().GetString("RowKey");
            }
            catch (Exception)
            {
                return default;
            }
        }

        public TableEntity GetProfileTableEntityForSaveSearch(string profileId, string savedSearchId)
        {
            try
            {
                return GetProfileTableEntityByPartitionKeyRowKey(profileId, "SS_" + savedSearchId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region PreferenceCenter
        public string GetProfileTableHeaderDataSet(string profileId)
        {
            var profileTableEntity = azureStorageManager.GetTableEntityByPartitionKeyRowKey(profileId, "_Header");
            try
            {
                return profileTableEntity.GetString("DataSet");
            }
            catch (Exception)
            {
                return default;
            }
        }
        #endregion

        #region SavedVehicles
        public string RetrieveProfileTablePropertyValue(string profileId, string adId, string propertyName, bool isPrivateAd = false)
        {
            var profileTableEntity = azureStorageManager.GetTableEntityByPartitionKeyRowKey(profileId, "SV_" + (isPrivateAd != true ? "5-" + adId : "19-" + adId));
            try
            {
                return profileTableEntity.GetString(propertyName);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public TableEntity RetrieveProfileTableSVEntityByQueryFilter(string profileId, string adId, bool isPrivateAd = false)
        {
            string rowKey = "SV_" + (isPrivateAd != true ? "5-" + adId : "19-" + adId);
            var profileTableEntity = azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {profileId} and RowKey eq {rowKey}");
            try
            {
                return profileTableEntity.Single();
            }
            catch (Exception)
            {
                return default;
            }
        }
        #endregion
    }




}
