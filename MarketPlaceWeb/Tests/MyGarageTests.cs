using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.SRP;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using UIAutomationLibrary.Base.AzureStorage;
using System.Collections.Generic;
using System.Text.Json;
using MarketPlaceWeb.Base.SqlDatabase;
using System.Linq;
using UIAutomationLibrary.Base.RestApi;
using System.Security.Policy;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    class MyGarageTests : Page
    {        
        HPMain homePage;
        MyGarageMain myGarage;
        VDPMain vdp;
        SRPMain srp;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        SsoLogin ssoLogin;
        Mailinator mailinator;
        SsoRegistration ssoRegistration;
        dynamic testDataFile;
        string testcaseId;
        string profileId, catId, vdpTitle = null, catDeleterecord;        
        bool isIosDevice;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            testDataFile = GetTestDataFile(baseURL);
            isIosDevice = (azureConfig.isAzureEnabled) ? (azureConfig.config == "ipad-safari-small" || azureConfig.config == "ios-safari-xs") : (localConfig.config == "ipad-safari-small" || localConfig.config == "ios-safari-xs");
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();            
            homePage = new HPMain(driver, viewport, language);
            myGarage = new MyGarageMain(driver, viewport, language);
            vdp = new VDPMain(driver, viewport, language);
            testcaseId = (string)TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            srp = new SRPMain(driver, viewport, language);
        }

        [TearDown]
        public void CleanUp()
        {
            ResultState resultState = TestContext.CurrentContext.Result.Outcome;
            if (resultState == ResultState.Error || resultState == ResultState.Failure)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
                if (!string.IsNullOrEmpty(localConfig.config) && !localConfig.config.ToLower().Contains("local")) MarketPlaceWeb.Driver.BrowserStackExtensions.MarkBSFailedStatus(driver);
            }
            driver.Quit();
        }

        #region SSO
        [Test, Property("TestCaseId", "11370")]
        public void VerifyUserLogsInSuccessfully()
        {            
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.userName"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.password"),
                FirstName = GetTestData(testDataFile, "commonTestData.featureSSO.firstName"),
                LastName = GetTestData(testDataFile, "commonTestData.featureSSO.lastName")
            };
            url = new Uri(baseURL);
            Open();
            
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            Assert.IsTrue(myGarage.VerifyAccountDetailsAreCorrect(ssoLogin.LocalAccountEmail, ssoLogin.FirstName, ssoLogin.LastName), "My garage home page is not open successfully");
            homePage.LogoutFromSsoAccount();
            Assert.IsTrue(homePage.IsMyGarageSignedOut(), "My garage home page is not signed out successfully");
        }

        [Test, Property("TestCaseId", "11385")]
        public void VerifyLoginOptions()
        {
            url = new Uri(baseURL);
            Open();
            
            homePage.LaunchSsoModal();
            Assert.IsTrue(homePage.IsFaceBookLoginAvailable(), "Facebook login button is NOT available");                        
            Assert.IsTrue(homePage.IsGoogleLoginAvailable(), "Google login button is NOT available");         
            Assert.IsTrue(homePage.IsAppleLoginAvailable(), "Apple login button is NOT available");
        }

        [Test, Property("TestCaseId", "11356")]
        public void VerifyInvalidUserNamePassword()
        {
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.userName"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.password"),
                FirstName = GetTestData(testDataFile, "commonTestData.featureSSO.firstName"),
                LastName = GetTestData(testDataFile, "commonTestData.featureSSO.lastName"),
                InvalidUser = GetTestData(testDataFile, "commonTestData.featureSSO.invalidUser"),
                InvalidPassword = GetTestData(testDataFile, $"commonTestData.featureSSO.invalidPassword")
            };            
           
            string invalidUserText = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.invalidUserText");
            string invalidPasswordText = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.invalidPasswordText");           
            
            url = new Uri(baseURL);
            Open();

            // No username and password entered
            homePage.LaunchSsoModal();
            Assert.True(homePage.IsLoginBtnDisabledOnEmptyCredential(), "Log in button is not disabled when Email / Password fields are empty");
            
            //Enter invalid username and password
            homePage.EnterSsoLoginEmailPassword(ssoLogin.InvalidUser, ssoLogin.InvalidPassword);            
            homePage.ClickLogin();
            Assert.AreEqual(invalidUserText,
                homePage.VerifyLoginFieldsValidationsForInvalidDetails(), "Invalid username & invalid password validations are not fired ");
            
            //Enter invalid password only
            homePage.EnterSsoLoginEmailPassword(ssoLogin.LocalAccountEmail, ssoLogin.InvalidPassword);            
            homePage.ClickLogin();
            Assert.AreEqual(invalidPasswordText,
                homePage.VerifyLoginFieldsValidationsForInvalidDetails(), "Invalid Password validation text Not fired");
        }

        [Test, Property("TestCaseId", "10951")]
        public void VerifyUserCanResetPasswordByForgotYourPasswordLink()
        {
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName"),
                MailinatorPrivateInboxName = (viewport.ToString() == "Large") ? GetTestData(testDataFile, $"{testcaseId}.mailinatorInbox1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"{testcaseId}.mailinatorInbox2") : GetTestData(testDataFile, $"{testcaseId}.mailinatorInbox3")
            };
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                UpdatedPassword = "P" + Extensions.GenerateRandomString(7).ToLower() + Extensions.GenerateRandomNumber(3) + "@"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.ResetPasswordWithForgotPassword(ssoLogin, mailinator);
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.UpdatedPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Password reset was unsuccessful. Can't login with updated password.");
        }
        #endregion

        #region SavedSearch
        [Test, Property("TestCaseId", "11361")]
        public void VerifySaveSearchForAnonymousUserRegisterAfterSubscribe()
        {
            #region Variables
            string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
            string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
            #endregion

            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken3"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName3"),
                MailinatorPrivateInboxName = "savedSearch." + Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL + srpURL);
            Open();

            srp.SubscribeSaveSearch(ssoRegistration.NewSsoAccountEmail);
            Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
            srp.CloseSavedSearchSuccessModal();
            Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after performing saved search for profileId: " + profileId);
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing Saved Search for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "11384")]
        public void VerifySaveSearchForAnonymousUserRegisterBeforeSubscribe()
        {
            #region Variables
            string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
            string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
            #endregion

            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken3"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName3"),
                MailinatorPrivateInboxName = "savedSearch." + Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL + srpURL);
            Open();
            
            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            url = new Uri(baseURL + srpURL);
            Open();

            srp.SubscribeSaveSearch(ssoRegistration.NewSsoAccountEmail);
            Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
            srp.CloseSavedSearchSuccessModal();
            Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after performing Saved Search for profileId: " + profileId);
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing Saved Search for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "11489")]
        public void VerifySaveSearchRedirectToSRPRegisteredUserNotLoggedIn()
        {
            #region Variables
            string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
            string srpSubURL = new Uri(baseURL + srpURL).GetLeftPart(UriPartial.Path);
            string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
            string makeCriteria = GetTestData(testDataFile, $"{testcaseId}.makeCriteria");
            string modelCriteria = GetTestData(testDataFile, $"{testcaseId}.modelCriteria");
            #endregion
            
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = (viewport.ToString() == "Large") ? GetTestData(testDataFile, $"{testcaseId}.localAccountEmail1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"{testcaseId}.localAccountEmail2") : GetTestData(testDataFile, $"{testcaseId}.localAccountEmail3"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");
            
            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            url = new Uri(baseURL + srpURL);
            Open();

            DateTime startTime = DateTime.UtcNow;
            srp.SubscribeSaveSearch(ssoLogin.LocalAccountEmail);
            Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
            srp.CloseSavedSearchSuccessModal();
            Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after subscribe save search");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after subscribe save search");
            
            QatdrNotificationStorage savedSearchIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedSearchIndex");
            string savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            bool isSavedSearchExistsInTable = savedSearchCriteria!=null ? savedSearchCriteria.Contains(makeCriteria) && savedSearchCriteria.Contains(modelCriteria) : false;
            Assert.True(isSavedSearchExistsInTable, "Azure storage entity not created in savedSearchIndex table after subscribe save search");

            string savedSearchId = savedSearchIndexTable.GetSavedSearchIndexTableRowKey(profileId, startTime);
            Assert.True(profileTable.GetProfileTableEntityForSaveSearch(profileId, savedSearchId) != null, "Azure storage entity (SS_" + savedSearchId + ") not created in profile table after performing save search subscription for profileId: " + profileId);

            // redirect and verify
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            
            myGarage.RedirectToSRPFromSavedSearch(searchName);
            Assert.True(srp.IsRedirectedToCorrectSRPFromMyGarage(srpSubURL,searchName), "Saved search does not redirect to correct SRP from My Garage");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            myGarage.ClickEditSavedSearchLink(searchName);
            myGarage.DeleteSelectedSavedSearch();
            Assert.False(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not deleted from saved search list of My Gargage");

            savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            Assert.IsNull(savedSearchCriteria, "Azure storage entity is not removed from savedSearchIndex table after un-subsubscribe save search");
        }

        [Test, Property("TestCaseId", "11440")]
        public void VerifyUpdateSaveSearchForRegisteredUserLoggedIn()
        {
            #region Variables
            string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
            string updateSaveSearchMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.updateSaveSearchMsg");
            string deleteSaveSearchMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.deleteSaveSearchMsg");
            string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
            string makeCriteria = GetTestData(testDataFile, $"{testcaseId}.makeCriteria");
            string modelCriteria = GetTestData(testDataFile, $"{testcaseId}.modelCriteria");
            string newSearchName = GetTestData(testDataFile, $"{testcaseId}.newSearchName");
            #endregion

            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = (viewport.ToString() == "Large") ? GetTestData(testDataFile, $"{testcaseId}.localAccountEmail1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"{testcaseId}.localAccountEmail2") : GetTestData(testDataFile, $"{testcaseId}.localAccountEmail3"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            url = new Uri(baseURL);
            Open();
            
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");
            
            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            url = new Uri(baseURL + srpURL);
            Open();          
            if (viewport != Viewport.XS) { srp.CloseSurveyCampaignDialog(); }

            //subscribe save search
            DateTime startTime = DateTime.UtcNow;
            srp.SubscribeSaveSearch(ssoLogin.LocalAccountEmail);
            Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
            srp.CloseSavedSearchSuccessModal();
            Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");
            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after subscribe save search");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after subscribe save search");
            
            QatdrNotificationStorage savedSearchIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedSearchIndex");
            string savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            bool isSavedSearchExistsInTable = savedSearchCriteria != null ? savedSearchCriteria.Contains(makeCriteria) && savedSearchCriteria.Contains(modelCriteria) : false;
            Assert.True(isSavedSearchExistsInTable, "Azure storage entity not created in savedSearchIndex table after subscribe save search");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);

            //update save search
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            myGarage.ClickEditSavedSearchLink(searchName);
            Assert.True(myGarage.IsEditSavedSearchDisplayed(), "Edit Saved search is not displayed");
            myGarage.UpdateSelectedSavedSearch(newSearchName);
            Assert.AreEqual(updateSaveSearchMsg, myGarage.GetToasterMessage(ToasterSelector.SaveSearch.ToString()), "Toaster message for update saved search doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.SaveSearch.ToString());
            Assert.True(myGarage.IsSavedSearchAvailable(newSearchName),"Updated Saved search is not displayed in saved search list of My Gargage");

            savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            isSavedSearchExistsInTable = savedSearchCriteria != null ? savedSearchCriteria.Contains(makeCriteria) && savedSearchCriteria.Contains(modelCriteria) : false;
            Assert.True(isSavedSearchExistsInTable, "Azure storage entity not exist in savedSearchIndex table after subscribe save search update");

            //delete save search
            myGarage.ClickEditSavedSearchLink(newSearchName);
            myGarage.DeleteSelectedSavedSearch();
            Assert.AreEqual(deleteSaveSearchMsg, myGarage.GetToasterMessage(ToasterSelector.SaveSearch.ToString()), "Toaster message for remove search doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.SaveSearch.ToString());
            Assert.False(myGarage.IsSavedSearchAvailable(newSearchName), "Updated Saved search is not deleted from saved search list of My Gargage");

            savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            Assert.IsNull(savedSearchCriteria, "Azure storage entity is not removed from savedSearchIndex table after delete save search");
        }

        [Test, Property("TestCaseId", "11360")]
        [Ignore("Social Account Out of scope for Automated Regression pipeline")]
        public void VerifySavedSearchOnSocialFBAccount()
        {
            if (!isIosDevice)
            {
                string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
                string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
                ssoLogin = new SsoLogin
                {
                    accountType = SsoLogin.SsoAccountType.SocialAccountFB,
                    SocialAccountFBEmail = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBEmail"),
                    SocialAccountFBPassword = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBPassword")
                };

                url = new Uri(baseURL);
                Open();

                homePage.LaunchSsoModal();
                homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.SocialAccountFBEmail, ssoLogin.SocialAccountFBPassword);
                Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");


                QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
                profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.SocialAccountFBEmail);
                Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");
                QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
                catId = profileTable.GetProfileTableCatId(profileId);
                Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");


                url = new Uri(baseURL + srpURL);
                Open();
                srp.SubscribeSaveSearch(ssoLogin.SocialAccountFBEmail);
                Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
                srp.CloseSavedSearchSuccessModal();
                Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");

                Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.SocialAccountFBEmail), "Profile Id is not same after subscribe save search");
                Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after subscribe save search");


                homePage.NavigateToMyGarage();
                myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
                Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");

                myGarage.ClickEditSavedSearchLink(searchName);
                Assert.True(myGarage.IsEditSavedSearchDisplayed(), "Edit Saved search is not displayed");
                myGarage.DeleteSelectedSavedSearch();
                myGarage.WaitForToasterMsgNotVisible(ToasterSelector.SaveSearch.ToString());
                Assert.False(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not deleted from saved search list of My Gargage");
            }
        }

        //Uncomment once undo link is visible in Beta
        /*
        [Test, Property("TestCaseId", "11442")]
        public void UndoDeletedSearchForRegUserLoggedIn()
        {
            string srpURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpURL");
            string deleteSaveSearchMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.deleteSaveSearchMsg"); 
            string undoDeleteSaveSearchMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.undoDeleteSaveSearchMsg");
            string searchName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.searchName");
            string makeCriteria = GetTestData(testDataFile, $"{testcaseId}.makeCriteria");
            string modelCriteria = GetTestData(testDataFile, $"{testcaseId}.modelCriteria");
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            //login
            url = new Uri(baseURL);
            Open();
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoginSuccess(), "Login to local account was unsuccessful");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");
            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            //subscribe save search
            url = new Uri(baseURL + srpURL);
            Open();
            if (viewport != Viewport.XS) { srp.CloseSurveyCampaignDialog(); }
            DateTime startTime = DateTime.UtcNow;
            srp.SubscribeSaveSearch(ssoLogin.LocalAccountEmail);
            Assert.True(srp.IsSavedSearchSuccessDisplayed(), "Saved search success modal is not displayed");
            srp.CloseSavedSearchSuccessModal();
            Assert.True(srp.GetSubscribeButtonStatus(), "Subscribe button is visible after subscription");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after subscribe save search");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after subscribe save search");
            QatdrNotificationStorage savedSearchIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedSearchIndex");
            string savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            bool isSavedSearchExistsInTable = savedSearchCriteria != null ? savedSearchCriteria.Contains(makeCriteria) && savedSearchCriteria.Contains(modelCriteria) : false;
            Assert.True(isSavedSearchExistsInTable, "Azure storage entity not created in savedSearchIndex table after subscribe save search");

            //undo deleted save search
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            myGarage.ClickEditSavedSearchLink(searchName);
            Assert.True(myGarage.IsEditSavedSearchDisplayed(), "Edit Saved search is not displayed");
            myGarage.DeleteSelectedSavedSearch();
            Assert.AreEqual(deleteSaveSearchMsg, myGarage.GetToasterMessage(), "Toaster message for remove saved search doesn't match");
            myGarage.ClickUndoLink();
            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            Assert.AreEqual(undoDeleteSaveSearchMsg, myGarage.GetToasterMessage(), "Toaster message for undo delete saved search doesn't match");
            myGarage.WaitForToasterMsgNotVisible();

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after undo deleted save search");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after undo deleted save search");
            savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            isSavedSearchExistsInTable = savedSearchCriteria != null ? savedSearchCriteria.Contains(makeCriteria) && savedSearchCriteria.Contains(modelCriteria) : false;
            Assert.True(isSavedSearchExistsInTable, "Azure storage entity not created in savedSearchIndex table after undo deleted save search ");

            Assert.True(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not displayed in Saved Search list of My Garage");
            myGarage.ClickEditSavedSearchLink(searchName);
            Assert.True(myGarage.IsEditSavedSearchDisplayed(), "Edit Saved search is not displayed");
            myGarage.DeleteSelectedSavedSearch();
            Assert.AreEqual(deleteSaveSearchMsg, myGarage.GetToasterMessage(), "Toaster message for remove saved search doesn't match");
            Assert.False(myGarage.IsSavedSearchAvailable(searchName), "Saved search is not deleted from saved search list of My Gargage");

            savedSearchCriteria = savedSearchIndexTable.GetSavedSearchCriteriaByPartitionKey(profileId, startTime);
            Assert.IsNull(savedSearchCriteria, "Azure storage entity is not removed from savedSearchIndex table after delete save search");
        }*/
        #endregion

        #region PriceAlert
        [Test, Property("TestCaseId", "11417")]
        public void VerifyPriceAlertVdpRedirectionRegisteredUserNotLoggedIn()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11417.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11417.pvAdId2") : GetTestData(testDataFile, "11417.pvAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoLogin.LocalAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after performing VDP price alert");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP price alert for profileId: " + profileId);

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after performing VDP price alert for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP price alert for profileId: " + profileId);

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.RedirectToVDPFromPriceAlertSavedVehicles(vdpTitle);
            Assert.True(vdp.IsRedirectedToCorrectVdpFromMyGarage(adId), "Does not redirect to correct VDP from My Garage");

            vdp.UnsubscribeVdpPriceAlert();
            Assert.True(vdp.GetPriceAlertBtnStatus(false), "Price alert button is not turned OFF after un-subscription");

            Assert.False(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not removed from savedAdIndex table after performing VDP price alert un-subscription from VDP for profileId: " + profileId);
            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after performing VDP price alert un-subscription from VDP for profileId: " + profileId);
        }

        [Test, Property("TestCaseId", "11418")]
        public void VerifyPriceAlertFromMyGaragePriceAlertsTabRegisteredUserLoggedIn()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11418.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11418.pvAdId2") : GetTestData(testDataFile, "11418.pvAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoLogin.LocalAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after performing VDP price alert for profileId: " + profileId);
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP price alert for profileId: " + profileId);

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after performing VDP price alert for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP price alert for profileId: " + profileId);

            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Price alert is still displayed in Price Alerts list of My Garage after unsubscription");

            Assert.False(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not removed from savedAdIndex table after performing VDP price alert un-subscription from My Garage for profileId: " + profileId);
        }

        [Test, Property("TestCaseId", "11424")]
        [Ignore("Out of scope for Automated Regression pipeline")]
        public void VerifyPriceAlertCnrAdFromMyGarage()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11424.cnrAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11424.cnrAdId2") : GetTestData(testDataFile, "11424.cnrAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoLogin.LocalAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP price alert for profileId: " + profileId);

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            vdp.GoToVDP(baseURL, adId);
            vdp.UnsubscribeVdpPriceAlert();
            Assert.True(vdp.GetPriceAlertBtnStatus(false), "Price alert button is not turned OFF after un-subscription");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Price alert is still displayed in Price Alerts list of My Garage after unsubscription");
        }

        [Test, Property("TestCaseId", "11546")]
        [Ignore("Social Automation Out of scope for Automated Regression pipeline")]
        public void VerifyPriceAlertOnSocialFBAccount()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11546.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11546.pvAdId2") : GetTestData(testDataFile, "11546.pvAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.SocialAccountFB,
                SocialAccountFBEmail = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBEmail"),
                SocialAccountFBPassword = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBPassword")
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.SocialAccountFBEmail, ssoLogin.SocialAccountFBPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.SocialAccountFBEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoLogin.SocialAccountFBEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP price alert for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Price alert is still displayed in Price Alerts list of My Garage after unsubscription");
        }

        [Test, Property("TestCaseId", "11547")]
        [Ignore("Social Automation Out of scope for Automated Regression pipeline")]
        public void VerifyPriceAlertDataSyncOnMultipleAccountTypes()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11547.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11547.pvAdId2") : GetTestData(testDataFile, "11547.pvAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.localAccountPassword"),
                SocialAccountFBEmail = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBEmail"),
                SocialAccountFBPassword = GetTestData(testDataFile, "commonTestData.featureSSO.socialAccountFBPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.SocialAccountFB, ssoLogin.SocialAccountFBEmail, ssoLogin.SocialAccountFBPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoLogin.SocialAccountFBEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after performing VDP price alert");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP price alert for profileId: " + profileId);

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after performing VDP price alert for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP price alert for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            homePage.LogoutFromSsoAccount();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.LocalAccount, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Price alert is still displayed in Price Alerts list of My Garage after unsubscription");

            Assert.False(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not removed from savedAdIndex table after performing VDP price alert un-subscription from My Garage for profileId: " + profileId);
            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after performing VDP price alert un-subscription from My Garage for profileId: " + profileId);
        }

        [Test, Property("TestCaseId", "11390")]
        public void VerifyPriceAlertForAnonymousUserRegisterAfterMRE()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureSSO.pvAdId");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken5"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName5"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoRegistration.NewSsoAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after performing VDP price alert for profileId: " + profileId);
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP price alert for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "11681")]
        public void VerifyPriceAlertForAnonymousUserRegisterBeforeMRE()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureSSO.pvAdId");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken5"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName5"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoRegistration.NewSsoAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after performing VDP price alert for profileId: " + profileId);
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP price alert for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        //Incapsula is blocking below test from sending lead in Beta.
        [Test, Property("TestCaseId", "11422")]
        [Ignore("Incapsula blocking on sending lead.")]
        public void VerifyPriceAlertSaveSearchOnVdpLeadForm()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11422.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11422.pvAdId2") : GetTestData(testDataFile, "11422.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken5"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName5"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();

            DateTime subscriptionCreatedUtc = DateTime.UtcNow;

            EmailLeadForm emailLeadForm = new EmailLeadForm
            {
                LeadForm = EmailLeadForm.EmailLeads.GeneralInquiry,
                Name = ssoRegistration.NewSsoAccountFirstName + " " + ssoRegistration.NewSsoAccountLastName,
                Email = ssoRegistration.NewSsoAccountEmail,
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                Message = "This is General Inquiry from Marketplace Web Automation"
            };
            vdp.SubmitLeadForm(emailLeadForm, true);
            Assert.True(vdp.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm), "Lead submission message is not displayed");
            if (viewport == Viewport.XS) { Open(); }

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after performing VDP lead form price alert subscription");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after performing VDP lead form price alert subscription for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedSearches);
            Assert.True(myGarage.IsSavedSearchDisplayedForLeadFormSubsciption(), "Saved search is not displayed in Saved searches tab of My Garage");

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after performing VDP lead form price alert subscription for profileId: " + profileId);

            QatdrNotificationStorage savedSearchIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedSearchIndex");
            Assert.True(savedSearchIndexTable.GetSavedSearchIndexTableEntityByPartitionKeyRowKey(profileId, subscriptionCreatedUtc) != null, "Azure storage entity not created in savedSearchIndex table after performing VDP lead form price alert subscription for profileId: " + profileId);
            string savedSearchId = savedSearchIndexTable.GetSavedSearchIndexTableRowKey(profileId, subscriptionCreatedUtc);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after performing VDP lead form price alert subscription for profileId: " + profileId);
            Assert.True(profileTable.GetProfileTableEntityForSaveSearch(profileId, savedSearchId) != null, "Azure storage entity (SS_" + savedSearchId + ") not created in profile table after performing VDP lead form price alert subscription for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }
        #endregion

        #region SavedVehicles
        [Test, Property("TestCaseId", "61439")]
        public void VerifySaveVehicleVDPUnSaveOnSavedVehiclesTabRegisteredUserPostLogIn()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, $"{testcaseId}.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"{testcaseId}.pvAdId2") : GetTestData(testDataFile, $"{testcaseId}.pvAdId3");
            #endregion
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, "commonTestData.featureSSO.savedVehicles.localAccountEmail"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.savedVehicles.localAccountPassword")
            };

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(homePage.IsSsoLoginPageDisplayed(), "User is not redirected to SSO login page after clicking heart button on VDP from logged out state");

            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail), "Profile Id is not same after saving vehicle on VDP.");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after saving vehicle on VDP for profileId: " + profileId);

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after saving vehicle on VDP for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after saving vehicle on VDP for profileId: " + profileId);

            string profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag is not true for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
            });

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of My Garage");

            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Saved vehicle is still displayed in Saved vehicles list of My Garage after un-saving vehicle from My Garage.");

            Assert.False(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not removed from savedAdIndex table after un-saving VDP from My Garage for profileId: " + profileId);
            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after un-saving VDP from My Garage Saved Vehicles tab for profileId: " + profileId);
        }

        [Test, Property("TestCaseId", "61448")]
        public void VerifySaveVehicleVDPRedirectionWithUnSaveOnVDPPreRegistered()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "61439.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "61439.pvAdId2") : GetTestData(testDataFile, "61439.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken4"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName4"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            Assert.AreEqual(profileId, emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail), "Profile Id is not same after saving vehicle on VDP.");
            Assert.AreEqual(catId, profileTable.GetProfileTableCatId(profileId), "Cat Id is not same after saving vehicle on VDP for profileId: " + profileId);

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after saving vehicle on VDP for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after saving vehicle on VDP for profileId: " + profileId);

            string profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag is not true for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
            });

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of My Garage");

            myGarage.RedirectToVDPFromPriceAlertSavedVehicles(vdpTitle);
            Assert.True(vdp.IsRedirectedToCorrectVdpFromMyGarage(adId), "Does not redirect to correct VDP from My Garage");

            vdp.ClickSaveVehicleBtn();
            Assert.True(vdp.GetSaveVehicleBtnStatus(false), "Heart button does not turn OFF after un-saving vehicle on VDP.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Saved vehicle is still displayed in Saved vehicles list of My Garage after un-saving vehicle from VDP.");

            Assert.False(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not removed from savedAdIndex table after un-saving vehicle from VDP for profileId: " + profileId);
            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after un-saving VDP from VDP for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "54803")]
        public void VerifySaveVehicleVDPUnSaveOnHomeTabRegisterOnFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "61439.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "61439.pvAdId2") : GetTestData(testDataFile, "61439.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken4"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName4"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(homePage.IsSsoLoginPageDisplayed(), "User is not redirected to SSO login page after clicking heart button on VDP from logged out state");

            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle and registering SSO on flow on VDP.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.Home);
            Assert.True(myGarage.IsSavedSubscribedVehicleDisplayedInWidget(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of Home tab in My Garage");

            QatdrNotificationStorage savedAdIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "savedAdIndex");
            Assert.True(savedAdIndexTable.GetSavedAdIndexTableEntityByPartitionKeyRowKey(adId, profileId) != null, "Azure storage entity not created in savedAdIndex table after saving vehicle on VDP for profileId: " + profileId);

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after saving vehicle on VDP for profileId: " + profileId);

            string profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag is not true for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
            });

            myGarage.UnsubscribeUnSaveVehicleInWidget(vdpTitle);
            Assert.True(myGarage.IsSavedSubscribedVehicleDisplayedInWidget(vdpTitle, false), "Saved vehicle is still displayed in Saved vehicles widget of Home tab in My Garage after un-saving vehicle from Home tab.");

            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Saved vehicle is still displayed in Saved vehicles list of Saved Vehicles tab after un-saving vehicle from Home tab.");

            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after un-saving VDP from My Garage Home tab for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "64209")]
        public void VerifyVehicleStatusInMyGarageAzureStorageForVDPWithSavedVehiclePriceAlert()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "61439.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "61439.pvAdId2") : GetTestData(testDataFile, "61439.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken4"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName4"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after saving vehicle on VDP for profileId: " + profileId);

            string profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag is not false for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag is not true for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after saving vehicle");
            });

            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoRegistration.NewSsoAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag is not turned True for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after performing price alert");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag is not turned True for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after performing price alert");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag does not stay true for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after performing price alert with active save vehicle on same VDP");
            });

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage");

            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of My Garage");

            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Saved vehicle is still displayed in Saved vehicles list of My Garage after un-saving vehicle from My Garage.");

            profileTableSavedVehicleDatasetPropertyVal = profileTable.RetrieveProfileTablePropertyValue(profileId, adId, "DataSet");
            Assert.Multiple(() => {
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnEnabled.ToString()), "PdnEnabled flag does not stay True for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after unsaving vehicle with active price alert");
                Assert.True(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.PdnLeadEnabled.ToString()), "PdnLeadEnabled flag does not stay True for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after unsaving vehicle with active price alert");
                Assert.False(GetJsonValue<bool>(profileTableSavedVehicleDatasetPropertyVal, AzureProfileTableSavedVehicleFlags.SavedEnabled.ToString()), "SavedEnabled flag does not turn False for Azure storage entity: (SV_5-" + adId + ") and profileId:" + profileId + " after unsaving vehicle");
            });

            myGarage.SwitchToMyGarageTab(MyGarageTabs.PriceAlerts);
            myGarage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle, false), "Price alert is still displayed in Price Alerts list of My Garage after unsubscription");

            Assert.False(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") is not removed from profile table after performing VDP price alert un-subscription from My Garage for profileId: " + profileId);
        }

        [Test, Property("TestCaseId", "64286")]
        public void VerifyVehicleAlreadySaved()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "61439.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "61439.pvAdId2") : GetTestData(testDataFile, "61439.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken7"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName7"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            Assert.True(profileTable.GetProfileTableEntityForPriceAlertSavedVehicle(profileId, adId) != null, "Azure storage entity (SV_5-" + adId + ") not created in profile table after saving vehicle on VDP for profileId: " + profileId);

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of My Garage");

            homePage.LogoutFromSsoAccount();
            vdp.GoToVDP(baseURL, adId);
            vdp.ClickSaveVehicleBtn();
            Assert.True(homePage.IsSsoLoginPageDisplayed(), "User is not redirected to SSO login page after clicking heart button on VDP from logged out state");

            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.LocalAccount, ssoRegistration.NewSsoAccountEmail, ssoRegistration.NewSsoAccountPassword);
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.GetAllVdpCardsCountInMyGarage() == 1, "Saving already saved vehcile creates duplicate saved vehicle cards in My Garage.");

            Assert.True(profileTable.RetrieveProfileTableSVEntityByQueryFilter(profileId, adId) != null, "Saving already saved vehcile creates duplicate Azure Storage entity (SV_5-" + adId + ") for profileId: " + profileId);

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "64402")]
        public void VerifyVDPHeartBtnStatusSync()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "61439.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "61439.pvAdId2") : GetTestData(testDataFile, "61439.pvAdId3");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken7"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName7"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickSaveVehicleBtn();
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button does not turn ON after saving vehicle on VDP.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.SavedVehicles);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Vehicle is not displayed in Saved Vehicles list of My Garage");

            homePage.LogoutFromSsoAccount();
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.GetSaveVehicleBtnStatus(false), "Heart button does not turn OFF on VDP after logging out.");

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.LocalAccount, ssoRegistration.NewSsoAccountEmail, ssoRegistration.NewSsoAccountPassword);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.GetSaveVehicleBtnStatus(), "Heart button status does not sync (turn ON) on VDP after logging in.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }
        #endregion

        #region MyVehicles
        [Test, Property("TestCaseId", "72413")]
        public void VerifyAddVehicleByYmmtFromEmptyState()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken8"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName8"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };
            MyVehicle myVehicle = new MyVehicle
            {
                Year = DateTime.Now.AddYears(-3).Year.ToString(),
                Make = GetTestData(testDataFile, "commonTestData.featureSSO.myVehicles.make"),
                Kilometres = "10000"
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for new registered user");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with Year/Make/Model/Trim failed");

            Assert.Multiple(() => {
                Assert.True(myGarage.GetVehicleTitle().Contains(myVehicle.Year), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain matching vehicle year selected: " + myVehicle.Year);
                Assert.True(myGarage.GetVehicleTitle().Contains(myVehicle.Make.ToLower()), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain matching vehicle make selected: " + myVehicle.Make.ToLower());
                Assert.True(myGarage.GetVehicleMileageDetail().Equals(myVehicle.Kilometres), "Added vehicle detail mileage: '" + myGarage.GetVehicleMileageDetail() + "' does not match with entered mileage: " + myVehicle.Kilometres);
            });

            //Commenting out DB verification for Azure Devops until https://trader.atlassian.net/browse/MCTE-1110 is fixed. Can be run locally with VPN connection.
            /*CatDatabase catDatabase = new CatDatabase(driver, GetTestData(testDataFile, "connectionStrings.sql-tdrcatqa01Cat"));
            Assert.True(catDatabase.GetVehicleId(catId).Count > 0, "Can't find Cat DB record for added vehicle with Cat Id: " + catId);
            Assert.Multiple(() => {
                Assert.True(catDatabase.GetVehicleModel(catId).Any(x => myGarage.GetVehicleTitle().Contains(x.ToLower())), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain vehicle model from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleTrim(catId).Any(x => myGarage.GetVehicleTrimDetail().Equals(x.ToLower())), "Added vehicle detail trim : '" + myGarage.GetVehicleTrimDetail() + "' does not match vehicle trim from Cat Database for Cat Id: " + catId);
            });
            catDatabase.Dispose();*/

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "72414")]
        public void VerifyAddVehicleByVinFromEmptyState()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            #region Variables
            string ApiSubscriptionKey = GetTestData(testDataFile, "restApi.featureMyVehicle.apiSubscriptionKey");
            string ApiSubscriptionValue = GetTestData(testDataFile, "restApi.featureMyVehicle.apiSubscriptionValue");
            string VehicleInformationByVinEndpoint = GetTestData(testDataFile, "restApi.featureMyVehicle.apiEndpoints.vehicleInformationByVinEndpoint");
            string VinParameterKey = GetTestData(testDataFile, "restApi.featureMyVehicle.queryParameters.vinParameterKey");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken8"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName8"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };
            MyVehicle myVehicle = new MyVehicle
            {
                Vin = GetTestData(testDataFile, "commonTestData.featureSSO.myVehicles.vin"),
                Kilometres = "10000"
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for new registered user");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with Vin/Trim failed");

            Assert.Multiple(() => {
                Assert.True(myGarage.GetVehicleVinDetail().Equals(myVehicle.Vin.ToLower()), "Added vehicle detail Vin: '" + myGarage.GetVehicleVinDetail() + "' does not match with entered Vin: " + myVehicle.Vin.ToLower());
                Assert.True(myGarage.GetVehicleMileageDetail().Equals(myVehicle.Kilometres), "Added vehicle detail mileage: '" + myGarage.GetVehicleMileageDetail() + "' does not match with entered mileage: " + myVehicle.Kilometres);
            });

            MyVehicleApi myVehicleApi = new MyVehicleApi(baseURL, ApiSubscriptionKey, ApiSubscriptionValue);
            var getVehicleInformationByVinResponse = myVehicleApi.GetVehicleInformationByVin(VehicleInformationByVinEndpoint, VinParameterKey, myVehicle.Vin);
            var apiResponseContent = myVehicleApi.GetVehicleInformationByVinResponseContent(getVehicleInformationByVinResponse);
            Assert.Multiple(() => {
                Assert.True(myGarage.GetVehicleTitle().Contains(apiResponseContent.Year.ToString()), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain correct decoded Year: " + apiResponseContent.Year.ToString());
                Assert.True(myGarage.GetVehicleTitle().Contains(apiResponseContent.Make.ToLower()), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain correct decoded Make: " + apiResponseContent.Make.ToLower());
                Assert.True(myGarage.GetVehicleTitle().Contains(apiResponseContent.Model.ToLower()), "Added vehicle title: '" + myGarage.GetVehicleTitle() + "' does not contain correct decoded Model: " + apiResponseContent.Model.ToLower());
                Assert.True(apiResponseContent.Trims.Any(x => myGarage.GetVehicleTrimDetail().Equals(x.Value.ToLower())), "Added vehicle detail trim : '" + myGarage.GetVehicleTrimDetail() + "' does not contain correct decoded Trim. List of decoded Trims: " + string.Join(", ", apiResponseContent.Trims.Select(x => x.Value).ToList()));
            });
            myVehicleApi.Dispose();

            //Commenting out DB verification for Azure Devops until https://trader.atlassian.net/browse/MCTE-1110 is fixed. Can be run locally with VPN connection.
            /*CatDatabase catDatabase = new CatDatabase(driver, GetTestData(testDataFile, "connectionStrings.sql-tdrcatqa01Cat"));
            Assert.True(catDatabase.GetVehicleId(catId).Count > 0, "Can't find Cat DB record for added vehicle with Cat Id: " + catId);
            Assert.Multiple(() =>
            {
                Assert.True(catDatabase.GetVehicleVin(catId).Any(x => myGarage.GetVehicleVinDetail().Contains(x.ToLower())), "Added vehicle detail Vin: '" + myGarage.GetVehicleVinDetail() + "' does not contain vehicle Vin from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleTrim(catId).Any(x => myGarage.GetVehicleTrimDetail().Equals(x.ToLower())), "Added vehicle detail trim : '" + myGarage.GetVehicleTrimDetail() + "' does not contain vehicle trim from Cat Database for Cat Id: " + catId);
            });
            catDatabase.Dispose();*/

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "73663")]
        public void VerifyAddVinForExistingMyVehicle()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            #region Variables
            string ApiSubscriptionKey = GetTestData(testDataFile, "restApi.featureMyVehicle.apiSubscriptionKey");
            string ApiSubscriptionValue = GetTestData(testDataFile, "restApi.featureMyVehicle.apiSubscriptionValue");
            string VehicleInformationByVinEndpoint = GetTestData(testDataFile, "restApi.featureMyVehicle.apiEndpoints.vehicleInformationByVinEndpoint");
            string VinParameterKey = GetTestData(testDataFile, "restApi.featureMyVehicle.queryParameters.vinParameterKey");
            #endregion
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken8"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName8"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };
            MyVehicle myVehicle = new MyVehicle
            {
                Year = GetTestData(testDataFile, $"{testcaseId}.year"),
                Make = GetTestData(testDataFile, $"{testcaseId}.make"),
                Model = GetTestData(testDataFile, $"{testcaseId}.model"),
                Kilometres = "10000"
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for new registered user");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with Year/Make/Model/Trim failed");

            string myVehicleTitle = myGarage.GetVehicleTitle();
            myVehicle.Vin = GetTestData(testDataFile, $"{testcaseId}.vin");
            myGarage.AddVinToMyVehicle(myVehicle);

            MyVehicleApi myVehicleApi = new MyVehicleApi(baseURL, ApiSubscriptionKey, ApiSubscriptionValue);
            var getVehicleInformationByVinResponse = myVehicleApi.GetVehicleInformationByVin(VehicleInformationByVinEndpoint, VinParameterKey, myVehicle.Vin);
            var apiResponseContent = myVehicleApi.GetVehicleInformationByVinResponseContent(getVehicleInformationByVinResponse);
            Assert.Multiple(() => {
                Assert.True(myVehicleTitle.Contains(apiResponseContent.Year.ToString()), "VIN decoded Year: " + apiResponseContent.Year.ToString() + ", does not match with existing vehicle title: " + myVehicleTitle);
                Assert.True(myVehicleTitle.Contains(apiResponseContent.Make.ToLower()), "VIN decoded Make: " + apiResponseContent.Make.ToString() + ", does not match with existing vehicle title: " + myVehicleTitle);
                Assert.True(myVehicleTitle.Contains(apiResponseContent.Model.ToLower()), "VIN decoded Model: " + apiResponseContent.Model.ToString() + ", does not match with existing vehicle title: " + myVehicleTitle);
                Assert.True(apiResponseContent.Trims.Any(x => myGarage.GetVehicleTrimDetail().Equals(x.Value.ToLower())), "Updated vehicle detail trim : '" + myGarage.GetVehicleTrimDetail() + "' does not contain correct decoded Trim. List of decoded Trims: " + string.Join(", ", apiResponseContent.Trims.Select(x => x.Value).ToList()));
            });
            myVehicleApi.Dispose();

            Assert.True(myVehicle.Vin.ToLower().Equals(myGarage.GetVehicleVinDetail()), "Added vehicle detail Vin: '" + myGarage.GetVehicleVinDetail() + "' does not match with entered Vin: " + myVehicle.Vin.ToLower());

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "75151")]
        public void VerifyAddVehicleFromAddVehicleTab()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken8"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName8"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };
            MyVehicle myVehicle = new MyVehicle
            {
                Year = DateTime.Now.AddYears(-3).Year.ToString(),
                Make = GetTestData(testDataFile, "commonTestData.featureSSO.myVehicles.make"),
                Kilometres = "10000"
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for new registered user");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with Year/Make/Model/Trim failed");
            Assert.True(myGarage.IsAddVehicleTabOnCarrouselDisplayed(), "'+Add vehicle' tab is not displaying after adding a vehicle from empty state");

            myVehicle.Vin = GetTestData(testDataFile, "commonTestData.featureSSO.myVehicles.vin");
            myGarage.AddMyVehicle(myVehicle, isEmptyState : false);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for added vehicle from '+Add vehicle' tab");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.GetMyVehiclesCount() == 2, "Adding vehicle from '+Add vehicle' tab failed");

            //Commenting out DB verification for Azure Devops until https://trader.atlassian.net/browse/MCTE-1110 is fixed. Can be run locally with VPN connection.
            /*CatDatabase catDatabase = new CatDatabase(driver, GetTestData(testDataFile, "connectionStrings.sql-tdrcatqa01Cat"));
            Assert.True(catDatabase.GetVehicleId(catId, expectedDbRows: 2).Count == 2, "Can't find 2 Cat DB records for added vehicle with Cat Id: " + catId);
            catDatabase.Dispose();*/

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "59201")]
        public void VerifyEditVehicleForYmmtVehicle()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            #region Variables
            string year = GetTestData(testDataFile, $"{testcaseId}.year");
            string make = GetTestData(testDataFile, $"{testcaseId}.make");
            string kilometres = GetTestData(testDataFile, $"{testcaseId}.kilometres");
            string editKilometre = GetTestData(testDataFile, $"{testcaseId}.editKilometre");
            string editTrim = GetTestData(testDataFile, $"{testcaseId}.editTrim");
            string editColourName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.editColour.name");
            string editColourRGB = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.editColour.rgb");
            #endregion

            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = (viewport == Viewport.Small) ? GetTestData(testDataFile, "commonTestData.featureSSO.userEmailSM") : (viewport == Viewport.XS) ? GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.userEmailXS") : GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.userEmailDT"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.password")
            };
            MyVehicle myVehicle = new MyVehicle
            {
                Year = year,
                Make = make,
                Kilometres = kilometres
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            if (myGarage.IsAddMyVehicleSuccess())
            {
                myGarage.DeleteMyVehicleFromEditVehicle();
            }
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for edit vehicle scenario");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with Year/Make/Model/Trim failed");


            myVehicle.Kilometres = editKilometre;
            myVehicle.Trim = editTrim;
            myVehicle.Colour = editColourRGB;
            myGarage.EditMyVehicle(myVehicle);

            string updatedVehicleTitle = myGarage.GetVehicleTitle();
            string updatedMileage = myGarage.GetVehicleMileageDetail();
            string updatedColour = myGarage.GetVehicleColorDetail();
            string updatedTrim = myGarage.GetVehicleTrimDetail();

            /*
            CatDatabase catDatabase = new CatDatabase(driver, GetTestData(testDataFile, "connectionStrings.sql-tdrcatqa01Cat"));
            //Commenting out DB verification for Azure Devops until https://trader.atlassian.net/browse/MCTE-1110 is fixed. Can be run locally with VPN connection.
        
            Assert.True(catDatabase.GetVehicleId(catId).Count > 0, "Can't find Cat DB record for updated vehicle with Cat Id: " + catId);
            Assert.Multiple(() => {
                Assert.True(catDatabase.GetVehicleMileage(catId).Any(x => updatedMileage.Equals(x.ToLower())), "Updated vehicle mileage: '" + updatedVehicleTitle + "' does not contain vehicle mileage from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleTrim(catId).Any(x => updatedTrim.Equals(x.ToLower())), "Updated vehicle detail trim : '" + updatedTrim + "' does not match vehicle trim from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleColor(catId).Any(x => updatedColour.Equals(x.ToLower())), "Updated vehicle detail color : '" + updatedTrim + "' does not match vehicle color from Cat Database for Cat Id: " + catId);
            });
            catDatabase.Dispose();
            */

            Assert.Multiple(() => {
                Assert.True(updatedVehicleTitle.Contains(myVehicle.Year), "Updated vehicle title: '" + updatedVehicleTitle + "' does not contain added vehicle year selected: " + myVehicle.Year);
                Assert.True(updatedVehicleTitle.Contains(myVehicle.Make.ToLower()), "Updated vehicle title: '" + updatedVehicleTitle + "' does not contain added vehicle make selected: " + myVehicle.Make.ToLower());
                if (viewport != Viewport.Small) { Assert.True(updatedMileage.Equals(myVehicle.Kilometres), "Updated vehicle detail mileage: '" + updatedMileage + "' does not match with updated mileage: " + myVehicle.Kilometres); }
                Assert.True(updatedTrim.Equals(myVehicle.Trim.ToLower()), "Updated vehicle trim: '" + updatedTrim + "' does not match with updated trim: " + myVehicle.Trim.ToLower());
                Assert.True(updatedColour.Equals(editColourName, StringComparison.OrdinalIgnoreCase), "Updated color: '" + updatedColour + "' does not match with updated color: " + editColourName);
            });
        }

        [Test, Property("TestCaseId", "59202")]
        public void VerifyEditVehicleForVinVehicle()
        {
            if (viewport == Viewport.Small)
            {
                Assert.Ignore("Add vehicle modal does not let enter kilometer data on Safari");
            }
            #region Variables
            string vin = GetTestData(testDataFile, $"{testcaseId}.vin");
            string kilometres = GetTestData(testDataFile, $"{testcaseId}.kilometres");
            string editKilometre = GetTestData(testDataFile, $"{testcaseId}.editKilometre");
            string editTrim = GetTestData(testDataFile, $"{testcaseId}.editTrim");
            string editColourName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.editColour.name");
            string editColourRGB = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.editColour.rgb");
            #endregion

            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = (viewport == Viewport.Small) ? GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.userEmailSM2") : (viewport == Viewport.XS) ? GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.userEmailXS2") : GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.userEmailDT2"),
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.editVehicle.password")
            };

            MyVehicle myVehicle = new MyVehicle
            {
                Vin = vin,
                Kilometres = kilometres
            };
            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.MyVehicles);
            if (myGarage.IsAddMyVehicleSuccess())
            {
                myGarage.DeleteMyVehicleFromEditVehicle();
            }
            Assert.True(myGarage.IsMyVehicleEmptyStateDisplayed(), "My Vehicle empty state is not displayed for edit vehicle scenario");

            myGarage.AddMyVehicle(myVehicle);
            Assert.True(myGarage.IsAddMyVehicleSuccessModalDisplayed(), "Add vehicle success modal is not displayed for adding vehicle from empty state");
            myGarage.CloseAddVehicleSuccessModal();
            Assert.True(myGarage.IsAddMyVehicleSuccess(), "Adding my vehicle with VIN failed");


            myVehicle.Kilometres = editKilometre;
            myVehicle.Trim = editTrim;
            myVehicle.Colour = editColourRGB;
            myGarage.EditMyVehicle(myVehicle);

            string updatedMileage = myGarage.GetVehicleMileageDetail();
            string updatedColour = myGarage.GetVehicleColorDetail();
            string updatedTrim = myGarage.GetVehicleTrimDetail();

            /*
            CatDatabase catDatabase = new CatDatabase(driver, GetTestData(testDataFile, "connectionStrings.sql-tdrcatqa01Cat"));
            //Commenting out DB verification for Azure Devops until https://trader.atlassian.net/browse/MCTE-1110 is fixed. Can be run locally with VPN connection.
        
            Assert.True(catDatabase.GetVehicleId(catId).Count > 0, "Can't find Cat DB record for updated vehicle with Cat Id: " + catId);
            Assert.Multiple(() => {
                Assert.True(catDatabase.GetVehicleMileage(catId).Any(x => updatedMileage.Equals(x.ToLower())), "Updated vehicle mileage: '" + updatedVehicleTitle + "' does not contain vehicle mileage from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleTrim(catId).Any(x => updatedTrim.Equals(x.ToLower())), "Updated vehicle detail trim : '" + updatedTrim + "' does not match vehicle trim from Cat Database for Cat Id: " + catId);
                Assert.True(catDatabase.GetVehicleColor(catId).Any(x => updatedColour.Equals(x.ToLower())), "Updated vehicle detail color : '" + updatedTrim + "' does not match vehicle color from Cat Database for Cat Id: " + catId);
            });
            catDatabase.Dispose();
            */

            Assert.Multiple(() => {
                if (viewport != Viewport.Small) { Assert.True(updatedMileage.Equals(myVehicle.Kilometres), "Updated vehicle detail mileage: '" + updatedMileage + "' does not match with updated mileage: " + myVehicle.Kilometres); }
                Assert.True(updatedTrim.Equals(myVehicle.Trim.ToLower()), "Updated vehicle trim: '" + updatedTrim + "' does not match with updated trim: " + myVehicle.Trim.ToLower());
                Assert.True(updatedColour.Equals(editColourName, StringComparison.OrdinalIgnoreCase), "Updated color: '" + updatedColour + "' does not match with updated color: " + editColourName);
            });
        }

        #endregion

        #region Registration
        [Test, Property("TestCaseId", "54673")]
        public void VerifyResendCodeRegistrationFlow()
        {
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken6"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName6"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterSsoAccountWithResentCode(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "54747")]
        public void VerifyChangeEmailRegistrationFlow()
        {
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken6"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName6"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower(),
                MailinatorPrivateInboxName_2 = Extensions.GenerateRandomString(7).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountEmail_2 = mailinator.MailinatorPrivateInboxName_2 + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterSsoAccountWithChangedEmail(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail_2);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }

        [Test, Property("TestCaseId", "54986")]
        public void VerifyUniqueSSOProfile()
        {
            Mailinator mailinator1 = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken2"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName2"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower(),                
            };
            Mailinator mailinator2 = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken2"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName2"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(7).ToLower()
            };

            SsoRegistration ssoRegistration1 = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator1.MailinatorPrivateInboxName + "@" + mailinator1.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = GetTestData(testDataFile, "commonTestData.featureSSO.firstName") + Extensions.GenerateRandomString(3),
                NewSsoAccountLastName = GetTestData(testDataFile, "commonTestData.featureSSO.lastName") + Extensions.GenerateRandomString(3)
            };            
            SsoRegistration ssoRegistration2 = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator2.MailinatorPrivateInboxName + "@" + mailinator2.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = GetTestData(testDataFile, "commonTestData.featureSSO.firstName") + "2",
                NewSsoAccountLastName = GetTestData(testDataFile, "commonTestData.featureSSO.lastName") + "2"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration1, mailinator1);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable1 = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            var profileId1 = emailIndexTable1.GetEmailIndexRowKeyByPartitionKey(ssoRegistration1.NewSsoAccountEmail);            
            Assert.IsNotNull(profileId1, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable1 = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            var catId1 = profileTable1.GetProfileTableCatId(profileId1);
            Assert.IsNotNull(catId1, "Cat Id not found on Azure Storage profile table.");

            homePage.LogoutFromSsoAccount();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration2, mailinator2);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable2 = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            var profileId2 = emailIndexTable2.GetEmailIndexRowKeyByPartitionKey(ssoRegistration2.NewSsoAccountEmail);            
            Assert.IsNotNull(profileId2, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable2 = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            var catId2 = profileTable2.GetProfileTableCatId(profileId2);
            Assert.IsNotNull(catId2, "Cat Id not found on Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
            Assert.IsTrue(homePage.IsMyGarageSignedOut(), "It seems the account did not logged out after delete, please investigate");

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.LocalAccount, ssoRegistration1.NewSsoAccountEmail, ssoRegistration1.NewSsoAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();

            Assert.AreNotEqual(profileId1, profileId2, "It seems the profile ids are not unique");
            Assert.AreNotEqual(catId1, catId2, "It seems the cat ids are not unique");
        }

        [Test, Property("TestCaseId", "12200")]
        public void VerifySSONewAccountRegistrationAndAccountDeletion()
        {
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken2"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName2"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = "Marketplace",
                NewSsoAccountLastName = "Uiautomation"
            };

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            catId = profileTable.GetProfileTableCatId(profileId);
            Assert.IsNotNull(catId, "Unable to retrieve Cat Id from Azure Storage profile table.");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();

            TdrCatQaStorage catDeleteRequestsTable = new TdrCatQaStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageTdrcatqastorage"), "CatDeleteRequests");
            catDeleterecord = catDeleteRequestsTable.GetRowKeyByEmail(ssoRegistration.NewSsoAccountEmail);
            Assert.IsNotNull(catDeleterecord, "Unable to retrieve cat delete record from Azure Storage CatDeleteRequests table.");
            Assert.IsTrue(homePage.IsMyGarageSignedOut(), "It seems the account did not log out after delete, please investigate");
        }
        #endregion

        #region AccountSettings
        [Test,Property("TestCaseId", "10973")]
        [Ignore("BUG : CONS2280 The Account Details is not updated successfully, please investigate")]
        public void VerifyUserCanEditPersonalDetailsOnAccountSettingsTabInMyGarage()
        {
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,                
                LocalAccountPassword = GetTestData(testDataFile, $"{testcaseId}.password"),                
                FirstName = GetTestData(testDataFile, $"{testcaseId}.defaultFirstName"),
                LastName = GetTestData(testDataFile, $"{testcaseId}.defaultLastName"),
                PostalCode = GetTestData(testDataFile, $"{testcaseId}.defaultpostcode"),
                PhoneNumber = GetTestData(testDataFile, $"{testcaseId}.defaultphoneNumber"),
                UpdatedFirstName = GetTestData(testDataFile, $"{testcaseId}.updatedFirstName"),
                UpdatedLastName = GetTestData(testDataFile, $"{testcaseId}.updatedLastName"),
                UpdatedPostalCode = GetTestData(testDataFile, $"{testcaseId}.updatePostcode"),
                UpdatedPhoneNumber = GetTestData(testDataFile, $"{testcaseId}.updatePhoneNumber")
            };
            
            //Using multiple accounts just to avoid simultaneous updates to the same account in case of parallel test execution
            switch (viewport)
            {
                case Viewport.Large:
                case Viewport.Small:
                    ssoLogin.LocalAccountEmail = (language==Language.EN)? GetTestData(testDataFile, $"{testcaseId}.userNameWnEN"): GetTestData(testDataFile, $"{testcaseId}.userNameWnFR");
                    break ;                
                case Viewport.XS:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, $"{testcaseId}.userNameAnEN") : GetTestData(testDataFile, $"{testcaseId}.userNameAnFR");                    
                    break;
                default:
                    throw new Exception("Invalid Viewport and Language");
            }
            
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue1 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue2 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> toUpdate;
            defaultValue1.Add(MyGarageLocators.AccountSettings.FirstName, ssoLogin.FirstName);
            defaultValue1.Add(MyGarageLocators.AccountSettings.LastName, ssoLogin.LastName);
            defaultValue1.Add(MyGarageLocators.AccountSettings.PostalCode, ssoLogin.PostalCode);
            defaultValue1.Add(MyGarageLocators.AccountSettings.PhoneNumber, ssoLogin.PhoneNumber);
            defaultValue2.Add(MyGarageLocators.AccountSettings.FirstName, ssoLogin.UpdatedFirstName);
            defaultValue2.Add(MyGarageLocators.AccountSettings.LastName, ssoLogin.UpdatedLastName);
            defaultValue2.Add(MyGarageLocators.AccountSettings.PostalCode, ssoLogin.UpdatedPostalCode);
            defaultValue2.Add(MyGarageLocators.AccountSettings.PhoneNumber, ssoLogin.UpdatedPhoneNumber);
            
            url = new Uri(baseURL);
            Open();
            
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);           
            toUpdate = myGarage.CheckExistingValuess(defaultValue1) ? defaultValue2 : defaultValue1;
            myGarage.ClickEditPersonalDetails();
            myGarage.UpdatePersonalDetails(toUpdate);
            homePage.LogoutFromSsoAccount();
            
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            Assert.IsTrue(myGarage.ValidateUpdatedPersonalDetails(toUpdate), "BUG : CONS2280 The Account Details is not updated successfully, please investigate");  
        }

        [Test, Property("TestCaseId", "10974")]
        public void VerifyUserCanChangePasswordOnAccountSettingsTabInMyGarage()
        {
            if (viewport != Viewport.Small)  //Safari CORS issue on iPad
            {
                ssoLogin = new SsoLogin
                {
                    accountType = SsoLogin.SsoAccountType.LocalAccount,
                    LocalAccountEmail = (viewport.ToString() == "Large") ? GetTestData(testDataFile, $"{testcaseId}.userName1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"{testcaseId}.userName2") : GetTestData(testDataFile, $"{testcaseId}.userName3"),
                    LocalAccountPassword = GetTestData(testDataFile, $"{testcaseId}.passwordA"),
                    UpdatedPassword = GetTestData(testDataFile, $"{testcaseId}.passwordB")
                };

                string updatePassword, oldPassword;
                url = new Uri(baseURL);
                Open();

                homePage.LaunchSsoModal();
                homePage.EnterSsoLoginEmailPassword(ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
                homePage.ClickLogin();

                if (homePage.IsThePasswordCorrect())
                {
                    updatePassword = ssoLogin.UpdatedPassword;
                    oldPassword = ssoLogin.LocalAccountPassword;
                }
                else
                {
                    homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.UpdatedPassword);
                    updatePassword = ssoLogin.LocalAccountPassword;
                    oldPassword = ssoLogin.UpdatedPassword;
                }
                Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

                homePage.NavigateToMyGarage();
                myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
                myGarage.ClickChangePassword();
                myGarage.UpdatePassword(updatePassword, oldPassword);
                homePage.LogoutFromSsoAccount();
                homePage.LaunchSsoModal();
                homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, updatePassword);
                Assert.True(homePage.IsSsoLoggedIn(), "It seems the password update from account details page was not successful, please investigate");
            }
        }

        [Test, Property("TestCaseId", "11443")]
        [Ignore("Social Account Out of scope for Automated Regression pipeline")]
        public void VerifyUserCanupdatePersonalDetailsOnAccountSettingsTabAndCheckFromSocialAccounts()
        {  
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, $"{testcaseId}.userName"),
                LocalAccountPassword = GetTestData(testDataFile, $"{testcaseId}.password"),
                SocialAccountFBPassword = GetTestData(testDataFile, $"{testcaseId}.passwordFB"),
                FirstName = GetTestData(testDataFile, $"{testcaseId}.defaultFirstName"),
                LastName = GetTestData(testDataFile, $"{testcaseId}.defaultLastName"),
                PostalCode = GetTestData(testDataFile, $"{testcaseId}.defaultpostcode"),
                PhoneNumber = GetTestData(testDataFile, $"{testcaseId}.defaultphoneNumber"),
                UpdatedFirstName = GetTestData(testDataFile, $"{testcaseId}.updatedFirstName"),
                UpdatedLastName = GetTestData(testDataFile, $"{testcaseId}.updatedLastName"),
                UpdatedPostalCode = GetTestData(testDataFile, $"{testcaseId}.updatePostcode"),
                UpdatedPhoneNumber = GetTestData(testDataFile, $"{testcaseId}.updatePhoneNumber")
            };
                                
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue1 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue2 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> toUpdate;
            defaultValue1.Add(MyGarageLocators.AccountSettings.FirstName, ssoLogin.FirstName);
            defaultValue1.Add(MyGarageLocators.AccountSettings.LastName, ssoLogin.LastName);
            defaultValue1.Add(MyGarageLocators.AccountSettings.PostalCode, ssoLogin.PostalCode);
            defaultValue1.Add(MyGarageLocators.AccountSettings.PhoneNumber, ssoLogin.PhoneNumber);
            defaultValue2.Add(MyGarageLocators.AccountSettings.FirstName, ssoLogin.UpdatedFirstName);
            defaultValue2.Add(MyGarageLocators.AccountSettings.LastName, ssoLogin.UpdatedLastName);
            defaultValue2.Add(MyGarageLocators.AccountSettings.PostalCode, ssoLogin.UpdatedPostalCode);
            defaultValue2.Add(MyGarageLocators.AccountSettings.PhoneNumber, ssoLogin.UpdatedPhoneNumber);

            url = new Uri(baseURL);
            Open();
            
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            toUpdate = myGarage.CheckExistingValuess(defaultValue1) ? defaultValue2 : defaultValue1;
            myGarage.ClickEditPersonalDetails();
            myGarage.UpdatePersonalDetails(toUpdate);
            homePage.LogoutFromSsoAccount();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.SocialAccountFB, ssoLogin.LocalAccountEmail, ssoLogin.SocialAccountFBPassword);
            Assert.IsTrue(homePage.IsSsoLoggedIn(), "My garage login was not successful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            Assert.IsTrue(myGarage.ValidateUpdatedPersonalDetails(toUpdate), "The Account Details did  not  match when logeed in from Social account, please onvestigate");
        }

        [Test, Property("TestCaseId", "11466")]
        [Ignore("Social Account Out of scope for Automated Regression pipeline")]
        public void VerifyUserCanUpdatePersonalDetailsFromSocialAccountsAndCheckFromLocalAccount()
        {
            #region variables
            string username;
            switch (viewport)
            {
                case Viewport.Large:
                    username = (language == Language.EN) ? GetTestData(testDataFile, $"{testcaseId}.featureSSO.userNameWnEN") : GetTestData(testDataFile, $"{testcaseId}.featureSSO.userNameWnFR");
                    break;
                case Viewport.XS:
                    username = (language == Language.EN) ? GetTestData(testDataFile, $"{testcaseId}.featureSSO.userNameAnEN") : GetTestData(testDataFile, $"{testcaseId}.featureSSO.userNameAnFR");
                    break;
                default:
                    throw new Exception("Invalid Viewport and Language");
            }            
            string password = GetTestData(testDataFile, $"{testcaseId}.featureSSO.password");
            #endregion
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue1 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> defaultValue2 = new Dictionary<MyGarageLocators.AccountSettings, string>();
            IDictionary<MyGarageLocators.AccountSettings, string> toUpdate;
            defaultValue1.Add(MyGarageLocators.AccountSettings.FirstName, GetTestData(testDataFile, $"{testcaseId}.featureSSO.defaultFirstName"));
            defaultValue1.Add(MyGarageLocators.AccountSettings.LastName, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.defaultLastName"));
            defaultValue1.Add(MyGarageLocators.AccountSettings.PostalCode, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.defaultpostcode"));
            defaultValue1.Add(MyGarageLocators.AccountSettings.PhoneNumber, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.defaultphoneNumber"));
            defaultValue2.Add(MyGarageLocators.AccountSettings.FirstName, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.updatedFirstName"));
            defaultValue2.Add(MyGarageLocators.AccountSettings.LastName, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.updatedLastName"));
            defaultValue2.Add(MyGarageLocators.AccountSettings.PostalCode, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.updatePostcode"));
            defaultValue2.Add(MyGarageLocators.AccountSettings.PhoneNumber, GetTestData(testDataFile, $"{ testcaseId}.featureSSO.updatePhoneNumber"));

            url = new Uri(baseURL);
            Open();

            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.SocialAccountFB, username, password);
            Assert.IsTrue(homePage.IsSsoLoggedIn(), "My garage login was not successful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            toUpdate = myGarage.CheckExistingValuess(defaultValue1) ? defaultValue2 : defaultValue1;
            myGarage.ClickEditPersonalDetails();
            myGarage.UpdatePersonalDetails(toUpdate);
            homePage.LogoutFromSsoAccount();
            
            driver.Manage().Cookies.DeleteAllCookies();
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(SsoLogin.SsoAccountType.SocialAccountFB, username, password);
            Assert.IsTrue(homePage.IsSsoLoggedIn(), "My garage login was not successful");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            Assert.IsTrue(myGarage.ValidateUpdatedPersonalDetails(toUpdate), "The Account Details did  not  match when logeed in from Social account, please onvestigate");
        }
        #endregion

        #region Home
        [Test, Property("TestCaseId", "55142")]
        [Ignore("My Account Home Page refactor requires refactoring this test. Will be done on separate story.")]
        public void VerifyPriceAlertOnHomeTabMyGarage()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "11417.pvAdId1") : (viewport.ToString() == "XS") ? GetTestData(testDataFile, "11417.pvAdId2") : GetTestData(testDataFile, "11417.pvAdId3");
            #endregion
            
            mailinator = new Mailinator
            {
                MailinatorApiToken = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorApiToken7"),
                MailinatorPrivateDomainName = GetTestData(testDataFile, "commonTestData.featureSSO.mailinatorPrivateDomainName7"),
                MailinatorPrivateInboxName = Extensions.GenerateRandomString(8).ToLower()
            };
            ssoRegistration = new SsoRegistration
            {
                NewSsoAccountEmail = mailinator.MailinatorPrivateInboxName + "@" + mailinator.MailinatorPrivateDomainName,
                NewSsoAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.newSsoAccountPassword"),
                NewSsoAccountFirstName = GetTestData(testDataFile, "commonTestData.featureSSO.firstName"),
                NewSsoAccountLastName = GetTestData(testDataFile, "commonTestData.featureSSO.lastName")
            };

            url = new Uri(baseURL);
            Open();
            homePage.LaunchSsoModal();
            homePage.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(homePage.IsSsoLoggedIn(), "User is not logged in after account registration");

            vdp.GoToVDP(baseURL, adId);
            vdpTitle = vdp.GetVDPTitle();
            vdp.ClickPriceAlertBtn();
            vdp.EnterEmailPriceAlert(ssoRegistration.NewSsoAccountEmail);
            vdp.ClickPriceAlertSubscribeBtn();
            Assert.True(vdp.IsPriceAlertSuccessDisplayed(), "Price alert modal success message header is not displayed");

            vdp.ClosePriceAlertSuccessModal();
            Assert.True(vdp.GetPriceAlertBtnStatus(), "Price alert button is not turned ON after subscription");

            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.Home);
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage Home Page");
            
            myGarage.ClickShowAllPriceAlert();
            Assert.True(myGarage.IsNavigatedToPriceAlertsTab(), "The see all price alert didnt navigate to the Price Alert Tab");
            Assert.True(myGarage.IsVehicleDisplayedInList(vdpTitle), "Price alert is not displayed in Price Alerts list of My Garage Price Alert Page");

            myGarage.SwitchToMyGarageTab(MyGarageTabs.AccountSettings);
            myGarage.DeleteSsoAccount();
        }
        #endregion

        #region PreferenceCentre
        [Test, Property("TestCaseId", "58299")]
        public void verifyPreferenceCentreAllFlags()
        {
            string updatePreferenceCentreMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.updatePreferenceCentreMsg");
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.password")
            };
            switch (viewport)
            {
                case Viewport.Large:
                case Viewport.Small:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnFR");
                    break;
                case Viewport.XS:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnFR");
                    break;
                default:
                    throw new Exception("Invalid Viewport and Language");
            }

            var preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.NewListings, false } ,
                {MyGarageLocators.PreferenceCentreSettings.PriceDrop, false } ,
                {MyGarageLocators.PreferenceCentreSettings.SimilarListings, false } ,
                {MyGarageLocators.PreferenceCentreSettings.Newsletter, false } ,
                {MyGarageLocators.PreferenceCentreSettings.NewProducts, false } ,
                {MyGarageLocators.PreferenceCentreSettings.OffersAndDiscounts, false } ,
                {MyGarageLocators.PreferenceCentreSettings.Surverys, false }

            };
            url = new Uri(baseURL);
            Open();
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PreferenceCentre);
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.True(myGarage.IsPreferenceCentreModalDisplayed(), "PreferenceCentre modal is not displayed");
            myGarage.ClickPreferenceCenterUnsubscribeLink();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            string headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.Multiple(() =>
            {
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.PdnEnabled.ToString()), $"Price Drop notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SavedSearchEnabled.ToString()), $"New Lisitngs notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SimilarSearchEnabled.ToString()), $"Similar notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewsLettersEnabled.ToString()), $"Newsletter notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SurveysEnabled.ToString()), $"New Products notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewProductsEnabled.ToString()), $"Offers & Discounts notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SpecialOffersEnabled.ToString()), $"Surverys notification is not disabled for profileId {profileId}");
            });

            preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.NewListings, true } ,
                {MyGarageLocators.PreferenceCentreSettings.PriceDrop, true } ,
                {MyGarageLocators.PreferenceCentreSettings.SimilarListings, true } ,
                {MyGarageLocators.PreferenceCentreSettings.Newsletter, true } ,
                {MyGarageLocators.PreferenceCentreSettings.NewProducts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.OffersAndDiscounts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.Surverys, true }

            };
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());
            headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.Multiple(() =>
            {
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.PdnEnabled.ToString()), $"Price Drop notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SavedSearchEnabled.ToString()), $"New Lisitngs notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SimilarSearchEnabled.ToString()), $"Similar Listings notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewsLettersEnabled.ToString()), $"Newsletter notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SurveysEnabled.ToString()), $"New Products notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewProductsEnabled.ToString()), $"Offers & Discounts notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SpecialOffersEnabled.ToString()), $"Surverys notification is not enabled for profileId {profileId}");
            });


        }

        [Test, Property("TestCaseId", "58300")]
        public void verifyPreferenceCentreVehicleListingFlags()
        {
            string updatePreferenceCentreMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.updatePreferenceCentreMsg");
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.password")
            };
            switch (viewport)
            {
                case Viewport.Large:
                case Viewport.Small:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnFR");
                    break;
                case Viewport.XS:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnFR");
                    break;
                default:
                    throw new Exception("Invalid Viewport and Language");
            }

            var preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.NewListings, false } ,
                {MyGarageLocators.PreferenceCentreSettings.PriceDrop, false } ,
                {MyGarageLocators.PreferenceCentreSettings.SimilarListings, false },
                {MyGarageLocators.PreferenceCentreSettings.Newsletter, true } ,
                {MyGarageLocators.PreferenceCentreSettings.NewProducts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.OffersAndDiscounts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.Surverys, true }

            };
            url = new Uri(baseURL);
            Open();
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PreferenceCentre);
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            string headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.PdnEnabled.ToString()), $"Price Drop notification is not disabled for profileId {profileId}");
            Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SavedSearchEnabled.ToString()), $"New Lisitngs notification is not disabled for profileId {profileId}");
            Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SimilarSearchEnabled.ToString()), $"Similar notification is not disabled for profileId {profileId}");

            preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.NewListings, true } ,
                {MyGarageLocators.PreferenceCentreSettings.PriceDrop, true } ,
                {MyGarageLocators.PreferenceCentreSettings.SimilarListings, true }

            };
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());
            headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.Multiple(() =>
            {
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.PdnEnabled.ToString()), $"Price Drop notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SavedSearchEnabled.ToString()), $"New Lisitngs notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SimilarSearchEnabled.ToString()), $"Similar Listings notification is not enabled for profileId {profileId}");

            });
        }

        [Test, Property("TestCaseId", "58301")]
        public void verifyPreferenceCentreNewsAndUpdatesFlags()
        {
            string updatePreferenceCentreMsg = GetTestData(testDataFile, $"commonTestData.featureSSO.{language.ToString()}.updatePreferenceCentreMsg");
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountPassword = GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.password")
            };
            switch (viewport)
            {
                case Viewport.Large:
                case Viewport.Small:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameWnFR");
                    break;
                case Viewport.XS:
                    ssoLogin.LocalAccountEmail = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnEN") : GetTestData(testDataFile, "commonTestData.featureSSO.preferenceCentreTestAccounts.userNameAnFR");
                    break;
                default:
                    throw new Exception("Invalid Viewport and Language");
            }

            var preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.NewListings, true } ,
                {MyGarageLocators.PreferenceCentreSettings.PriceDrop, true } ,
                {MyGarageLocators.PreferenceCentreSettings.SimilarListings, true },
                {MyGarageLocators.PreferenceCentreSettings.Newsletter, false } ,
                {MyGarageLocators.PreferenceCentreSettings.NewProducts, false } ,
                {MyGarageLocators.PreferenceCentreSettings.OffersAndDiscounts, false } ,
                {MyGarageLocators.PreferenceCentreSettings.Surverys, false }

            };
            url = new Uri(baseURL);
            Open();
            homePage.LaunchSsoModal();
            homePage.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(homePage.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            homePage.NavigateToMyGarage();
            myGarage.SwitchToMyGarageTab(MyGarageTabs.PreferenceCentre);
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());

            QatdrNotificationStorage emailIndexTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "emailindex");
            profileId = emailIndexTable.GetEmailIndexRowKeyByPartitionKey(ssoLogin.LocalAccountEmail);
            Assert.IsNotNull(profileId, "Unable to retrieve Profile Id from Azure Storage emailindex table.");

            QatdrNotificationStorage profileTable = new QatdrNotificationStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrNotification"), "profile");
            string headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.Multiple(() =>
            {
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewsLettersEnabled.ToString()), $"Newsletter notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SurveysEnabled.ToString()), $"New Products notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewProductsEnabled.ToString()), $"Offers & Discounts notification is not disabled for profileId {profileId}");
                Assert.False(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SpecialOffersEnabled.ToString()), $"Surverys notification is not disabled for profileId {profileId}");
            });

            preferenceCentreSettings = new Dictionary<MyGarageLocators.PreferenceCentreSettings, bool>
            {
                {MyGarageLocators.PreferenceCentreSettings.Newsletter, true } ,
                {MyGarageLocators.PreferenceCentreSettings.NewProducts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.OffersAndDiscounts, true } ,
                {MyGarageLocators.PreferenceCentreSettings.Surverys, true }

            };
            myGarage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
            myGarage.ClickPreferenceCenterUpdateBtn();
            Assert.AreEqual(updatePreferenceCentreMsg, myGarage.GetToasterMessage(ToasterSelector.PreferenceCenter.ToString()), "Toaster message for update preference centre doesn't match");
            myGarage.WaitForToasterMsgNotVisible(ToasterSelector.PreferenceCenter.ToString());
            headerDataSet = profileTable.GetProfileTableHeaderDataSet(profileId);

            Assert.Multiple(() =>
            {
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewsLettersEnabled.ToString()), $"Newsletter notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SurveysEnabled.ToString()), $"New Products notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.NewProductsEnabled.ToString()), $"Offers & Discounts notification is not enabled for profileId {profileId}");
                Assert.True(GetJsonValue<bool>(headerDataSet, AzureTableProfileHeader.SpecialOffersEnabled.ToString()), $"Surverys notification is not enabled for profileId {profileId}");

            });
        }
        #endregion

    }
}
