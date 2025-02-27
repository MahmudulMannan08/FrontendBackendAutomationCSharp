using Dapper;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Base.SqlDatabase;

namespace MarketPlaceWeb.Base.SqlDatabase
{
    public class StatisticDatabase
    {
        DatabaseManager databaseManager;

        public StatisticDatabase(IWebDriver driver, string statisticDbConnectionString)
        {
            databaseManager = new DatabaseManager(driver, statisticDbConnectionString);
        }

        public IList<string> GetEmailLog(string customerEmail, string emailSubject, DateTimeOffset emailDate, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT Subject FROM Statistic.dbo.EmailLog WITH (NOLOCK) WHERE Subject = @EmailSubject AND EmailDate >= @EmailDate AND Attribute.value('(//CustomerInfo/Email)[1]', 'varchar(100)') = @CustomerEmail", new { EmailSubject = emailSubject, EmailDate = emailDate, CustomerEmail = customerEmail }, timeoutInSeconds: timeoutInSeconds);
        }

        public IList<string> GetEmailTopicId(string customerEmail, string emailSubject, DateTimeOffset emailDate, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT TopicID FROM Statistic.dbo.EmailLog WITH (NOLOCK) WHERE Subject = @EmailSubject AND EmailDate >= @EmailDate AND Attribute.value('(//CustomerInfo/Email)[1]', 'varchar(100)') = @CustomerEmail", new { EmailSubject = emailSubject, EmailDate = emailDate, CustomerEmail = customerEmail }, timeoutInSeconds : timeoutInSeconds);
        }

        public IList<string> GetEmailTopicId(string customerEmail, DateTimeOffset emailDate, int expectedDbRows = 1, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT TopicID FROM Statistic.dbo.EmailLog WITH (NOLOCK) WHERE EmailDate >= @EmailDate AND Attribute.value('(//CustomerInfo/Email)[1]', 'varchar(100)') = @CustomerEmail", new { EmailDate = emailDate, CustomerEmail = customerEmail }, expectedDbRows, timeoutInSeconds);
        }

        public IList<string> GetDealerInventoryLeadTopicId(string customerEmail, DateTimeOffset emailDate, int expectedDbRows = 1, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT TopicID FROM Statistic.dbo.EmailLog WITH (NOLOCK) WHERE EmailDate >= @EmailDate AND ReplyTo = @CustomerEmail", new { EmailDate = emailDate, CustomerEmail = customerEmail }, expectedDbRows, timeoutInSeconds);
        }

        public string GetIcoOfferCode(string customerEmail, DateTimeOffset emailDate, int expectedDbRows = 1, int timeoutInSeconds = 300)
        {
            return databaseManager.GetDbQueryResult<string>(@"SELECT Attribute.value('(//CustomerInfo/Attributes/AttributeTypeValue[@Type=""IcoOfferCode""]/@Value)[1]', 'varchar(255)') FROM Statistic.dbo.EmailLog WITH (NOLOCK) WHERE EmailDate >= @EmailDate AND Attribute.value('(//CustomerInfo/Email)[1]', 'varchar(100)') = @CustomerEmail", new { EmailDate = emailDate, CustomerEmail = customerEmail }, expectedDbRows, timeoutInSeconds).FirstOrDefault();
        }

        public void Dispose()
        {
            databaseManager.Dispose();
        }
    }
}
