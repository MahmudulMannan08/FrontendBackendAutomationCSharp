using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlaceWeb.Base;
using System.Text.RegularExpressions;

namespace UIAutomationLibrary.Base.ClientApi3rdParty
{
    public class MailinatorApi : Page
    {
        MailinatorApiManager mailinatorApiManager;

        public MailinatorApi(IWebDriver driver, string mailinatorApiToken)
        {
            mailinatorApiManager = new MailinatorApiManager(driver, mailinatorApiToken);
        }

        /// <summary>
        /// Retrieves SSO registration verification code from email body utilizing regular expression. 
        /// </summary>
        /// <param name="mailinatorPrivateDomainName">Mailinator private domain name.</param>
        /// <param name="mailinatorPrivateInboxName">Mailinator private inbox name. Use random string to create unique inbox name.</param>
        /// <returns>New account registration verification code.</returns>
        public string GetSsoVerificationCode(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName)
        {
            Wait(5);
            bool isVerificationCode = true;
            string html = Task.Run(() => mailinatorApiManager.GetMessageBodyAsync(mailinatorPrivateDomainName, mailinatorPrivateInboxName, isVerificationCode)).GetAwaiter().GetResult();  //Call Synchronously
            string regex = "<div\\s+style=\"font-size:\\s*\\d+px;\\s+line-height:\\s*\\d+px\"\\s*>" + "\\s*(\\d+)\\s*</div>";

            Match match = Regex.Match(html, regex);
            if (match.Success)
            {
                string number = match.Groups[1].Value;
                return number;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, string> GetSsoVerificationCodeAndMsgID(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName)
        {
            Wait(5);
            Dictionary<string, string> keyValuePairs;
            keyValuePairs = Task.Run(() => mailinatorApiManager.GetMessageBodyAndMessageID(mailinatorPrivateDomainName, mailinatorPrivateInboxName)).GetAwaiter().GetResult();  //Call Synchronously
            string regex = "<div\\s+style=\"font-size:\\s*\\d+px;\\s+line-height:\\s*\\d+px\"\\s*>" + "\\s*(\\d+)\\s*</div>";

            Match match = Regex.Match(keyValuePairs["messageBody"], regex);
            if (match.Success)
            {
                string number = match.Groups[1].Value;
                keyValuePairs.Add("Code", number);
                return keyValuePairs;
            }
            else
            {
                return null;
            }
        }




        public async Task DeleteAllMailinatorDomainMessagesAsync(string mailinatorPrivateDomainName)
        {
            await mailinatorApiManager.DeleteAllDomainMessagesAsync(mailinatorPrivateDomainName);
        }

        public async Task DeleteAllInboxMessagesAsync(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName)
        {
            await mailinatorApiManager.DeleteAllInboxMessagesAsync(mailinatorPrivateDomainName, mailinatorPrivateInboxName);
        }

        public async Task DeleteMailinatorMessageAsync(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName, string messageId)
        {
            await mailinatorApiManager.DeleteMailinatorMessageAsync(mailinatorPrivateDomainName, mailinatorPrivateInboxName, messageId);
        }
    }
}
