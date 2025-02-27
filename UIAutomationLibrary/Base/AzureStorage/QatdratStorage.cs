using Azure;
using Azure.Data.Tables;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.AzureStorage
{
    public class QatdratStorage
    {
        AzureStorageManager azureStorageManager;

        public QatdratStorage(IWebDriver driver, string qatdratStorageConnectionString, string qatdratTableName)
        {
            azureStorageManager = new AzureStorageManager(driver, qatdratStorageConnectionString, qatdratTableName);
        }

        #region ReservationStatus
        public DateTime GetReservationDateByChargeId(string chargeId)
        {
            var reservationStatusEntity = azureStorageManager.GetTableEntityByQueryFilter($"ChargeId eq {chargeId}");
            try
            {
                return (DateTime)reservationStatusEntity.First().GetDateTime("ReservationDate");
            }
            catch (Exception)
            {
                return default;
            }
        }

        public void UpdateExpirationDateByChargeId(string chargeId, DateTime expirationDateUpdate)
        {
            var reservationStatusEntity = azureStorageManager.GetTableEntityByQueryFilter($"ChargeId eq {chargeId}");
            azureStorageManager.UpdateTableEntityByPropertyName(reservationStatusEntity.First(), "ExpirationDate", expirationDateUpdate);
        }
        #endregion


        #region PriceAlert

        public string GetEmailIndexRowKey(string email)
        {
            var reservationStatusEntity = azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {email}");
            try
            {
                return reservationStatusEntity.First().GetString("RowKey");
            }
            catch (Exception)
            {
                return default;
            }
        }

        public Boolean IsProfileRowKeyFoundForTheSubscribedAd(string partitionKey,string adid)
        {

            var row = azureStorageManager.GetTableEntityByQueryFilter($"RowKey eq {"SV_" + adid} and PartitionKey eq {partitionKey}");

            try
            {
                return row.First().GetString("RowKey").Contains(adid);
            }
            catch (Exception)
            {
                return default;
            }

        }

        #endregion

        #region InstantCashOffer
        public Pageable<TableEntity> GetIcoOfferTableEntityByPartitionKey(string icoOfferCode)
        {
            return azureStorageManager.GetTableEntityByQueryFilter($"PartitionKey eq {icoOfferCode}");
        }

        public List<string> GetPartitionKeyByConsumerEmailandPhone(string consumerEmail, string consumerPhone)
        {
            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"ConsumerEmail eq {consumerEmail} and ConsumerCellPhone eq {consumerPhone}");

            List<string> PartitionKey = tableEntities.Select(o => o.GetString("PartitionKey")).ToList();

            return PartitionKey;
        }
        #endregion
    }
}
