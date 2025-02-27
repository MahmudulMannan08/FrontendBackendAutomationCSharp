using mailinator_csharp_client;
using mailinator_csharp_client.Models.Messages.Entities;
using mailinator_csharp_client.Models.Messages.Requests;
using mailinator_csharp_client.Models.Responses;
using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace UIAutomationLibrary.Base.ClientApi3rdParty
{
    public class MailinatorApiManager : Page
    {
        private MailinatorClient mailinatorClient { get; }
        private const string verificationMessageSubjectEN = "AutoTrader Account Email Verification Code";
        private const string verificationMessageSubjectFR = "Code de vérification du courriel du compte AutoHebdo";

        public MailinatorApiManager(IWebDriver driver, string mailinatorApiToken)
        {
            base.driver = driver;
            mailinatorClient = new MailinatorClient(mailinatorApiToken);
        }

        /// <summary>
        /// Retrieves message ID based on mailinator private domain name and private inbox name. Utilizes message Id to get mailinator message body (email body content). 
        /// </summary>
        /// <param name="mailinatorPrivateDomainName">Mailinator private domain name.</param>
        /// <param name="mailinatorPrivateInboxName">Mailinator private inbox name. Use random string to create unique inbox name</param>
        /// <param name="timeOut">Timeout in seconds</param>
        /// <param name="interval">Polling interval in milliseconds</param>
        /// <returns>Mailinator email body content</returns>
        public async Task<string> GetMessageBodyAsync(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName, bool isVerificationMessage = false, int timeOut = 240, int interval = 250)
        {
            string messageId = null;
            try
            {
                await WaitUntil(async () =>
                {
                    FetchInboxRequest fetchInboxRequest = new FetchInboxRequest() { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName, Skip = 0, Limit = 20, Sort = Sort.asc };
                    await Task.Delay(10000);
                    try
                    {
                        if (isVerificationMessage)
                        {
                            FetchInboxResponse response = await mailinatorClient.MessagesClient.FetchInboxAsync(fetchInboxRequest);
                            foreach (Message message in response.Messages)
                            {
                                if (message.Subject.ToLower().Contains(verificationMessageSubjectEN.ToLower()) || message.Subject.ToLower().Contains(verificationMessageSubjectFR.ToLower()))
                                {
                                    messageId = message.Id;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            messageId = (await mailinatorClient.MessagesClient.FetchInboxAsync(fetchInboxRequest)).Messages.Single().Id;
                        }

                    }
                    catch (Exception) { /*ignore exception*/ }
                    return !string.IsNullOrEmpty(messageId);
                }, timeOut, interval);
            }
            catch (Exception e)
            {
                throw new Exception("Verification code email not sent yet. Message Id not retrieved: " + e.Message);
            }

            FetchMessageRequest fetchMessageRequest = new FetchMessageRequest() { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName, MessageId = messageId };
            var messageBody = (await mailinatorClient.MessagesClient.FetchMessageAsync(fetchMessageRequest)).Parts.Single().Body;
            return messageBody;
        }

        /// <summary>
        /// Deletes all messages (emails) for the specified mailinator private domain. Deletes messages for all inboxes under this domain. 
        /// </summary>
        /// <param name="mailinatorPrivateDomainName">Mailinator private domain name.</param>
        public async Task DeleteAllDomainMessagesAsync(string mailinatorPrivateDomainName)
        {
            DeleteAllDomainMessagesRequest deleteAllDomainMessagesRequest = new DeleteAllDomainMessagesRequest() { Domain = mailinatorPrivateDomainName };
            await mailinatorClient.MessagesClient.DeleteAllDomainMessagesAsync(deleteAllDomainMessagesRequest);
        }

        /// <summary>
        /// Deletes all messages (emails) for the specified mailinator private domain inbox. Deletes all messages for specified inbox. 
        /// </summary>
        /// <param name="mailinatorPrivateDomainName">Mailinator private domain name.</param>
        public async Task DeleteAllInboxMessagesAsync(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName)
        {
            DeleteAllInboxMessagesRequest deleteAllInboxMessagesRequest = new DeleteAllInboxMessagesRequest() { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName };
            await mailinatorClient.MessagesClient.DeleteAllInboxMessagesAsync(deleteAllInboxMessagesRequest);
        }

        public async Task<Dictionary<string, string>> GetMessageBodyAndMessageID(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName, int timeOut = 240, int interval = 250)
        {
            string messageId = null;
            try
            {
                await WaitUntil(async () =>
                {
                    FetchInboxRequest fetchInboxRequest = new FetchInboxRequest() { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName, Skip = 0, Limit = 20, Sort = Sort.asc };
                    await Task.Delay(10000);
                    try
                    {
                        messageId = (await mailinatorClient.MessagesClient.FetchInboxAsync(fetchInboxRequest)).Messages.Single().Id;
                    }
                    catch (Exception) { /*ignore exception*/ }
                    return !string.IsNullOrEmpty(messageId);
                }, timeOut, interval);
            }
            catch (Exception e)
            {
                throw new Exception("Verification code email not sent yet. Message Id not retrieved: " + e.Message);
            }

            FetchMessageRequest fetchMessageRequest = new FetchMessageRequest() { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName, MessageId = messageId };
            var messageBody = (await mailinatorClient.MessagesClient.FetchMessageAsync(fetchMessageRequest)).Parts.Single().Body;
            Dictionary<string, string> inbox = new Dictionary<string, string>();
            inbox.Add("messageId", messageId);
            inbox.Add("messageBody", messageBody);
            return inbox;
        }

        public async Task DeleteMailinatorMessageAsync(string mailinatorPrivateDomainName, string mailinatorPrivateInboxName, string messageId)
        {
            DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest()
            { Domain = mailinatorPrivateDomainName, Inbox = mailinatorPrivateInboxName, MessageId = messageId };
            await mailinatorClient.MessagesClient.DeleteMessageAsync(deleteMessageRequest);
        }
    }
}
