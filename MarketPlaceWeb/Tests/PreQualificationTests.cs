using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using UIAutomationLibrary.Pages.PreQual;

namespace MarketPlaceWeb.Test
{
    [TestFixture]    
    public class PreQualificationTests : Page
    {
        SRPMain srp;
        VDPMain vdp;
        HPMain hp;
        PreQualMain preQual;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        SsoLogin ssoLogin;
        Mailinator mailinator;
        SsoRegistration ssoRegistration;
        string testcaseId;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            testDataFile = GetTestDataFile(baseURL);
            srpVariant = (azureConfig.isAzureEnabled) ? azureConfig.srpVariant : (viewport.ToString() == "XS") ?
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantXS") :
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantDT");
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            vdp = new VDPMain(driver, viewport, language);
            srp = new SRPMain(driver, viewport, language);
            hp = new HPMain(driver, viewport, language);
            preQual = new PreQualMain(driver, viewport, language);
            testcaseId = (string)TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
        }

        [TearDown]
        public void CleanUp()
        {
            ResultState resultState = TestContext.CurrentContext.Result.Outcome;
            if (resultState == ResultState.Error || resultState == ResultState.Failure)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
                if (!string.IsNullOrEmpty(localConfig.config) && !localConfig.config.ToLower().Contains("local")) BrowserStackExtensions.MarkBSFailedStatus(driver);
            }
            driver.Quit();
        }

        #region Old preQual 
        [Test, Property("TestCaseId", "8092")]        
        public void VerifyEasyFinanceApprovalBadge()
        {
            #region Variables
            string directLinkURL = (language.ToString() == "EN") ? GetTestData(testDataFile, "8092.EN.SRPLinkUrl") : GetTestData(testDataFile, "8092.FR.SRPLinkUrl");
            #endregion

            url = new Uri(baseURL + directLinkURL);
            Open();
            srp.IsEasyFinanceBadgeDisplayed();
            Assert.IsTrue(srp.IsEasyFinanceBadgeDisplayed(), $"The Easy Finance Approval badge is not displayed");
            Assert.IsTrue(srp.IsEasyFinanceBadgeDisplayedForAllListings(), $"The Easy Finance Approval badge is not displayed for all the listings");

            srp.OpenEasyFinanceModal();
            //srp.WaitForElementVisible(By.CssSelector(SRPLocators.Listing.EasyFinanceModal.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description));  //Obsolete test, add redirection to VDP verification
            //Assert.IsTrue(srp.IsElementVisible(By.CssSelector(SRPLocators.Listing.EasyFinanceModal.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description)), $"The Easy Finance Modal is not displayed");
        }

        [Test, Property("TestCaseId", "8056")]        
        public void VerifyPreQualJourney()
        {

            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = (language.ToString() == "EN") ? GetTestData(testDataFile, "8056.EN.preQualUrl") : GetTestData(testDataFile, "8056.FR.preQualUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.ClickPreQualGetStartedBtn();
            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.firstName"),
                LastName = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.lastName"),
                Email = "PreQual_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.phoneNumber"),


            };
            vdp.FillPersonalInfo(personalInfo);

            vdp.ClickPreQualNextBtn();

            Assert.True(vdp.IsPersonalInfoDisplayed(), "Personal info is not displayed on side bar");

            var addressInfo = new AddressInformation
            {

                AddressLine1 = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.addressLine1"),
                AddressLine2 = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.addressLine2"),
                City = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.city"),
                Province = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.province"),
                PostalCode = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.postalCode"),

            };
            vdp.FillAddressInfo(addressInfo);
            vdp.ClickPreQualNextBtn();
            vdp.SelectMonth();
            vdp.ClickPreQualNextBtn();
            vdp.SelectEmploymentStatus();
            vdp.ClickPreQualNextBtn();
            vdp.FillIncomeInfo();
            vdp.ClickPreQualNextBtn();

            var expenses = new Expenses
            {

                VehiclePayments = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.vehiclePayments"),
                HousingPayments = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.preQualConfig.housingPayments"),

            };

            vdp.FillExpenses(expenses);
            vdp.ClickPreQualNextBtn();
            vdp.FillCreditScoreInfo();
            vdp.ClickPreQualNextBtn();
            vdp.FillAppSummary();
            vdp.ClickPreQualNextBtn();
            Assert.True(vdp.IsConfirmatoryTitleDisplayed(), "Title is not displayed");
        }

        #endregion

        #region New PreQual

        [Test, Property("TestCaseId", "8092")]        
        public void VerifyPreQualLandingPageAndModalService()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickGlobalNavPreQual();
            Assert.IsTrue(preQual.VerifyPrequalLandingPage(),"Landing page has issues, please investigate");
            preQual.ClickFindOutYourAffordability();
            Assert.IsTrue(preQual.IsPreQualModalLandingVisible(),"Prequal Modal is not displayed when clicked from 'Find your affordability' button");
            preQual.ClickPreQualModalLandingCloseBtn();

        }

        [Test, Property("TestCaseId", "73113")]        
        public void VerifyExpiredPreQual()
        {
            PreQualLocators.PreQual resultType = PreQualLocators.PreQual.Expired;
            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.Email"),
                LocalAccountPassword = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.Password")
            };

           
            url = new Uri(baseURL);
            Open();
            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
            preQual.ClickContinueOnPreQualModalLanding();
            preQual.Wait(2);// using static wait for this situation as relogin needs extra buffer and wait for page load and other didint work
            hp.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(hp.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the sceanrio {resultType.ToString()} is not verified ");
            preQual.ClickGetStartedButton();
            Assert.True(preQual.IsStep1ModalVisible(), "Step 1 modal is not visible");


        }

        [Test, Property("TestCaseId", "73337")]        
        public void VerifyIDVMaxLimitReached()
        {
            PreQualLocators.PreQual resultType = PreQualLocators.PreQual.IDVMaxLimitReached;
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

            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.firstName"),
                LastName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.lastName"),
                Dob = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.dob"),
                PhoneNumber = Extensions.GeneratePhoneNumber(),
            };

            var addressInfo = new AddressInformation
            {
                AddressLine1 = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.addressLine1"),
                City = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.city"),
                Province = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.province"),
                PostalCode = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.postalCode"),
            };

            url = new Uri(baseURL);
            Open();
            for (int attempt = 0; attempt < 2; attempt++)
            {
                hp.ClickGlobalNavPreQual();
                preQual.ClickWhatCanIaffordFromLandingPage();
                preQual.WaitforPreQualModal();
                Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
                preQual.ClickContinueOnPreQualModalLanding();
                if (attempt == 0)
                {
                    hp.RegisterNewSsoAccount(ssoRegistration, mailinator);
                    Assert.True(hp.IsSsoLoggedIn(), "User is not logged in after account registration");
                }
                bool dataPersist = (attempt > 0)?true:false;
                preQual.CompletePreQualStep1(personalInfo, dataPersist);
                preQual.CompletePreQualStep2(addressInfo, dataPersist);
                preQual.CompletePreQualStep3InvalidKBA();
                preQual.ClickBrowseVehiclesButton();
            }


            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal(true);
            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the scenario {resultType.ToString()} is not verified");
            preQual.ClickBrowseVehiclesButton();
            Assert.IsTrue(srp.VerifyPriceFilterForPreQual(resultType), $"The SRP filter is not applied accurately for {resultType.ToString()}, please investigate");
        }


        [Test, Property("TestCaseId", "73120")]        
        [TestCase(PreQualLocators.PreQual.ErrorInValidIDV)]
        [TestCase(PreQualLocators.PreQual.ErrorCreditFileNotFound)]
        public void VerifyInvalidIDV(PreQualLocators.PreQual resultType)
        {
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

            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.firstName"),
                LastName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.lastName"),
                Dob = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.dob"),
                PhoneNumber = Extensions.GeneratePhoneNumber(),
            };

            var addressInfo = new AddressInformation
            {
                AddressLine1 = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.addressLine1"),
                City = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.city"),
                Province = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.province"),
                PostalCode = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.postalCode"),
            };

            url = new Uri(baseURL);
            Open();
            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
            preQual.ClickContinueOnPreQualModalLanding();

            hp.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(hp.IsSsoLoggedIn(), "User is not logged in after account registration");
            preQual.CompletePreQualStep1(personalInfo);
            preQual.CompletePreQualStep2(addressInfo);
            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the sceanrio {resultType.ToString()} is not verified ");
            if (resultType.Equals(PreQualLocators.PreQual.ErrorCreditFileNotFound))
            {
                preQual.ClickTryAgainButton();
                Assert.IsTrue(preQual.IsStep1ModalVisible());
            }else
            {
                preQual.ClickBrowseVehiclesButton();
                Assert.IsTrue(srp.VerifyPriceFilterForPreQual(resultType), $"The SRP filter is not applied accurately for {resultType.ToString()}, please investigate");

            }

        }


        [Test, Property("TestCaseId", "73121")]       
        public void VerifyInvalidKBA()
        {
            PreQualLocators.PreQual resultType = PreQualLocators.PreQual.ErrorInvalidKBA;
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
            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
            preQual.ClickContinueOnPreQualModalLanding();
            hp.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(hp.IsSsoLoggedIn(), "User is not logged in after account registration");


            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.firstName"),
                LastName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.lastName"),
                Dob = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.dob"),
                PhoneNumber = Extensions.GeneratePhoneNumber(),
            };

            var addressInfo = new AddressInformation
            {
                AddressLine1 = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.addressLine1"),
                City = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.city"),
                Province = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.province"),
                PostalCode = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.postalCode"),
            };

            preQual.CompletePreQualStep1(personalInfo);
            preQual.CompletePreQualStep2(addressInfo);
            preQual.CompletePreQualStep3InvalidKBA();
            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the sceanrio {resultType.ToString()} is not verified ");
            preQual.ClickBrowseVehiclesButton();
            Assert.IsTrue(srp.VerifyPriceFilterForPreQual(resultType), $"The SRP filter is not applied accurately for {resultType.ToString()}, please investigate");

        }

        [Test, Property("TestCaseId", "73343")]        
        public void VerifyPreQualDecline()
        {

            PreQualLocators.PreQual resultType = PreQualLocators.PreQual.PreQualDecline;
            #region Test Data
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
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

            ssoLogin = new SsoLogin
            {
                accountType = SsoLogin.SsoAccountType.LocalAccount,
                LocalAccountEmail = ssoRegistration.NewSsoAccountEmail,
                LocalAccountPassword = ssoRegistration.NewSsoAccountPassword,
            };

            #endregion

            url = new Uri(baseURL);
            Open();
            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
            preQual.ClickContinueOnPreQualModalLanding();
            hp.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(hp.IsSsoLoggedIn(), "User is not logged in after account registration");

            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.firstName"),
                LastName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.lastName"),
                Dob = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.dob"),
                PhoneNumber = Extensions.GeneratePhoneNumber(),
            };


            var addressInfo = new AddressInformation
            {
                AddressLine1 = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.addressLine1"),
                City = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.city"),
                Province = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.province"),
                PostalCode = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.postalCode"),
            };
            preQual.CompletePreQualStep1(personalInfo);
            preQual.CompletePreQualStep2(addressInfo);
            preQual.CompletePreQualIDVStep3();

            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the sceanrio {resultType.ToString()} is not verified ");
            preQual.ClickPreQualDeclineShopAllButton(resultType);
            Assert.IsTrue(srp.VerifyPriceFilterForPreQual(resultType), $"The SRP filter is not applied accurately for {resultType.ToString()}, please investigate");
            hp.LogoutFromSsoAccount();

            hp.ClickGlobalNavPreQual();
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(), "The PreQual Landing page did not open, please investigate");
            preQual.ClickContinueOnPreQualModalLanding();
            preQual.Wait(2);// using static wait for this situation as relogin needs extra buffer and wait for page load and other didint work
            hp.LoginToSsoAccount(ssoLogin.accountType, ssoLogin.LocalAccountEmail, ssoLogin.LocalAccountPassword);
            Assert.True(hp.IsSsoLoggedIn(), "Login to local account was unsuccessful");
            Assert.IsTrue(preQual.VerifyPreQualResults(resultType), $"The results page for the sceanrio {resultType.ToString()} is not verified ");


        }


        [Test, Property("TestCaseId", "72515")]
        [TestCase(PreQualLocators.PreQual.SuperPrime)]
        [TestCase(PreQualLocators.PreQual.Prime)]
        [TestCase(PreQualLocators.PreQual.SubPrime)]
        [TestCase(PreQualLocators.PreQual.DeepSubPrime)]
        [TestCase(PreQualLocators.PreQual.NearPrime)]
        public void VerifyPreQualJourneyFromGlobalNav(PreQualLocators.PreQual resultType)
        {
            #region Test Data
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
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

                                  
            #endregion

            url = new Uri(baseURL);
            Open();            
            hp.ClickGlobalNavPreQual();            
            preQual.ClickWhatCanIaffordFromLandingPage();
            preQual.WaitforPreQualModal();
            Assert.IsTrue(preQual.VerifyPrequalModalLanding(),"The PreQual Landing page did not open, please investigate");            
            preQual.ClickContinueOnPreQualModalLanding();

            hp.RegisterNewSsoAccount(ssoRegistration, mailinator);
            Assert.True(hp.IsSsoLoggedIn(), "User is not logged in after account registration");

            var personalInfo = new PersonalInformation
            {

                FirstName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.firstName"),
                LastName = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.lastName"),
                Email = ssoRegistration.NewSsoAccountEmail,
                Dob = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.dob"),
                PhoneNumber = Extensions.GeneratePhoneNumber(),
            };
            preQual.CompletePreQualStep1(personalInfo);

            var addressInfo = new AddressInformation
            {
                AddressLine1 = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.addressLine1"),                
                City = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.city"),
                Province = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.province"),
                PostalCode = GetTestData(testDataFile, $"preQualConfig.{resultType.ToString()}.postalCode"),
            };
            preQual.CompletePreQualStep2(addressInfo);            
            preQual.CompletePreQualIDVStep3();

            Assert.IsTrue(preQual.VerifyPreQualResults(resultType),$"The results page for the sceanrio {resultType.ToString()} is not verified ");
            Assert.IsTrue(preQual.IsExpiryDateValid(resultType),$"The Expiry date is not correct for the scenario {resultType.ToString()}, Please investigate ");
            string preQualMaxAmount = preQual.GetPreQualMaxapprovedAmount();
            preQual.ClickShopVehicleLink(resultType);
            Assert.IsTrue(srp.VerifyPriceFilterForPreQual(resultType, preQualMaxAmount), $"The SRP filter is not applied accurately for {resultType.ToString()}, please investigate");




        }

        #endregion 
    }
}
