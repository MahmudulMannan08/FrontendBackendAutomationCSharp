using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;


namespace UIAutomationLibrary.Base.AzureStorage
{
    public class QatdrTbybStorage
    {

        AzureStorageManager azureStorageManager;


        public QatdrTbybStorage(IWebDriver driver, string qatdrtbybStorageConnectionString, string qatdrtbybTableName)
        {
            azureStorageManager = new AzureStorageManager(driver, qatdrtbybStorageConnectionString, qatdrtbybTableName);
        }

        #region EmailMasking
        public List<string> GetProxyEmails(string emailAddress)
        {

            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"UserOriginalEmail eq {emailAddress}");

            var proxyEmails = tableEntities.Select(o => o.GetString("ProxyEmail")).Distinct().ToList();

            if (proxyEmails.Count == 1)
            {
                return new List<string>(proxyEmails.Take(1));
            }

            return proxyEmails;
        }

        public List<string> GetUserOriginalEmails(string emailAddress)
        {

            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"UserOriginalEmail eq {emailAddress}");

            List<string> originalEmail = tableEntities.Select(o => o.GetString("UserOriginalEmail")).ToList();

            return originalEmail;
        }
        #endregion

        #region PhoneMasking
        public List<string> GetOriginalPhoneNumber(string phoneNo)
        {
            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"UserOriginalPhone eq {phoneNo}");

            List<string> originalPhoneNumber = tableEntities.Select(o => o.GetString("UserOriginalPhone")).ToList();

            return originalPhoneNumber;
        }


        public List<string> GetProxyPhoneNumber(string phoneNo)
        {
            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"UserOriginalPhone eq {phoneNo}");

            List<string> proxyPhones = tableEntities.Select(o => o.GetString("ProxyPhone")).ToList();

            List<string> output = new List<string>();

            if (proxyPhones.Any(x => x == null))
            {
                int nullCount = proxyPhones.Count(s => s == null);
                var additionalPhoneCall = tableEntities.Select(o => o.GetBoolean("AdditionalPhonePurchaseCallMade")).ToList();
                int trueCount = additionalPhoneCall.Count(c => (bool)c);

                if (nullCount != trueCount)
                {
                    throw new Exception("The number of null values in proxyPhones does not match the number of additional phone purchase calls made.");
                }
            }

            foreach (string phoneNum in proxyPhones)
            {
                if (phoneNum == null)
                {
                    output.Add(null);
                }
                else
                {
                    output.Add(phoneNum);
                }
            }

            return output;
        }
        #endregion


        public List<string> GetCouponId(string PartitionKey)
        {
            var tableEntities = azureStorageManager.GetTableEntityByQueryFilter($"CouponId eq {PartitionKey}");

            List<string> Coupon = tableEntities.Select(o => o.GetString("CouponId")).ToList();

            return Coupon;
        }

    }
}
