using MarketPlaceWeb.Base;
using MarketPlaceWeb.Base.SqlDatabase;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIAutomationLibrary.Base.AzureStorage;
using UIAutomationLibrary.Pages.InstantCashOffer;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class InstantCashOfferTests : Page
    {
        string baseURL;
        dynamic testDataFile;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        VDPMain vdp;
        InstantCashOfferMain ico;
        IcoLeadForm icoLeadForm;
        StatisticDatabase statisticDatabase;
        DateTime EmailCreatedUtc;
        VinGenerator vinGenerator;
        bool isVinRequiredToggle;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            testDataFile = GetTestDataFile(baseURL);
            isVinRequiredToggle = (azureConfig.isAzureEnabled) ? azureConfig.isVinRequiredToggle : Convert.ToBoolean(GetTestData(testDataFile, "FeatureToggles.vinRequired"));
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            vdp = new VDPMain(driver, viewport, language);
            ico = new InstantCashOfferMain(driver, viewport, language);
            vinGenerator = new VinGenerator();

            icoLeadForm = new IcoLeadForm
            {
                Year = DateTime.Now.AddYears(-3).Year.ToString(),
                Make = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.make"),
                Kilometers = "10000",
                PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.postalCode"),
                FirstName = "MP",
                LastName = "IcoLeadTest",
                PhoneNumber = "6044" + Extensions.GenerateRandomNumber(6),
                Email = "IcoLeadTest_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                Color = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.color") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.color"),
                FrontTireCondition = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.frontTireCondition"),
                RearTireCondition = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.rearTireCondition"),
                CodeVerificationMethodName = (language.ToString() == "EN") ? "Text me" : "Me texter",
                PhoneCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.phoneCode"),

                IcoQuestionnaireAnswers = new Dictionary<IcoLeadForm.IcoQuestionnaire, bool> {
                    [IcoLeadForm.IcoQuestionnaire.IsOriginalOwner] = true,
                    [IcoLeadForm.IcoQuestionnaire.IsMakingPayment] = false,
                    [IcoLeadForm.IcoQuestionnaire.IsVehicleReplacement] = false,
                    [IcoLeadForm.IcoQuestionnaire.WasInAccident] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasCleanHistory] = true,
                    [IcoLeadForm.IcoQuestionnaire.HasDamage] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasMechanicalIssue] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasWarningLight] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasModification] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasOdor] = false,
                    [IcoLeadForm.IcoQuestionnaire.HasOtherIssue] = false,
                    [IcoLeadForm.IcoQuestionnaire.TireCondition] = true
                }
            };

            switch (icoLeadForm.icoLeadFormType)
            {
                case IcoLeadForm.IcoLeadFormType.BuyingCenter:
                case IcoLeadForm.IcoLeadFormType.TradeIn:
                    icoLeadForm.PhoneNumber = "6044" + Extensions.GenerateRandomNumber(6);
                    icoLeadForm.PhoneCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.phoneCode");

                    break;
                case IcoLeadForm.IcoLeadFormType.DWW:
                    icoLeadForm.PhoneNumber = "8675555309";
                    icoLeadForm.PhoneCode = "1234";

                    break;
            }
            statisticDatabase = new StatisticDatabase(driver, GetTestData(testDataFile, "connectionStrings.azrmdbqa1Statistic"));
            EmailCreatedUtc = DateTime.UtcNow;
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
            statisticDatabase.Dispose();
            driver.Quit();
        }

        [Test, Property("TestCaseId", "6692")]
        public void VerifyTradeinIcoByYMMT()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string tradeinIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoConsumerTopicId");
            string tradeinIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoDealerTopicId");
            string tradeinIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIco1STopicId");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.TradeIn;
            #endregion

            vdp.GoToVDP(baseURL, adId);
            Assert.True(ico.IsIcoWidgetAvailable(), "Instant Cash Offer widget is not available");

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterYmmtDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            ico.CloseIcoSuccessModal();
            Assert.True(ico.IsIcoModalClosed(), "Unable to close ICO success page");
            Assert.True(ico.IsIcoOfferDisplayedOnVdp(), "Instant cash offer is not displayed on VDP");
            Assert.True(ico.IsIcoWidgetButtonDisplayed(false, icoBtnLabel), "Instant cash offer button is displayed on VDP after submitting ICO");

            string icoOfferCode = statisticDatabase.GetIcoOfferCode(icoLeadForm.Email, EmailCreatedUtc);
            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            Assert.True(qatdratStorage.GetIcoOfferTableEntityByPartitionKey(icoOfferCode) != null, "IcoOfferTable entity is not found for submitted Instant Cash Offer");

            var expectedTradeinIcoTopicIdList = new List<string> { tradeinIcoConsumerTopicId, tradeinIcoDealerTopicId, tradeinIco1STopicId };
            var actualTradeinIcoTopicIdList = statisticDatabase.GetEmailTopicId(icoLeadForm.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expectedTradeinIcoTopicIdList.OrderBy(x => x), actualTradeinIcoTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualTradeinIcoTopicIdList.ToList()));  //Trade-in ICO Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "6693")]
        public void VerifyTradeinIcoByVIN()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string tradeinIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoConsumerTopicId");
            string tradeinIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoDealerTopicId");
            string tradeinIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIco1STopicId");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.TradeIn;
            #endregion

            vdp.GoToVDP(baseURL, adId);
            Assert.True(ico.IsIcoWidgetAvailable(), "Instant Cash Offer widget is not available");

            ico.ClickIcoButton(icoLeadForm);
            icoLeadForm.Vin = vinGenerator.GetRandomVin().VinNumber;
            ico.EnterVinDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            ico.CloseIcoSuccessModal();
            Assert.True(ico.IsIcoModalClosed(), "Unable to close ICO success page");
            Assert.True(ico.IsIcoOfferDisplayedOnVdp(), "Instant cash offer is not displayed on VDP");
            Assert.True(ico.IsIcoWidgetButtonDisplayed(false, icoBtnLabel), "Instant cash offer button is displayed on VDP after submitting ICO");

            string icoOfferCode = statisticDatabase.GetIcoOfferCode(icoLeadForm.Email, EmailCreatedUtc);
            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            Assert.True(qatdratStorage.GetIcoOfferTableEntityByPartitionKey(icoOfferCode) != null, "IcoOfferTable entity is not found for submitted Instant Cash Offer");

            var expectedTradeinIcoTopicIdList = new List<string> { tradeinIcoConsumerTopicId, tradeinIcoDealerTopicId, tradeinIco1STopicId };
            var actualTradeinIcoTopicIdList = statisticDatabase.GetEmailTopicId(icoLeadForm.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expectedTradeinIcoTopicIdList.OrderBy(x => x), actualTradeinIcoTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualTradeinIcoTopicIdList.ToList()));  //Trade-in ICO Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "6706")]
        public void VerifyBuyingCenterIcoByYMMT()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string buyingCenterIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoConsumerTopicId");
            string buyingCenterIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoDealerTopicId");
            string buyingCenterIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIco1STopicId");
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            #endregion

            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterYmmtDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            int participatingDealerCount = ico.GetParticipatingDealerCount();
            ico.CloseIcoSuccessModal();
            Assert.True(ico.IsIcoModalClosed(), "Unable to close ICO success page");

            string icoOfferCode = statisticDatabase.GetIcoOfferCode(icoLeadForm.Email, EmailCreatedUtc);
            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            Assert.True(qatdratStorage.GetIcoOfferTableEntityByPartitionKey(icoOfferCode) != null, "IcoOfferTable entity is not found for submitted Instant Cash Offer");

            var expectedTradeinIcoTopicIdList = new List<string> { buyingCenterIcoConsumerTopicId };
            expectedTradeinIcoTopicIdList.AddRange(Enumerable.Repeat(buyingCenterIcoDealerTopicId, participatingDealerCount));
            expectedTradeinIcoTopicIdList.AddRange(Enumerable.Repeat(buyingCenterIco1STopicId, participatingDealerCount));
            var actualTradeinIcoTopicIdList = statisticDatabase.GetEmailTopicId(icoLeadForm.Email, EmailCreatedUtc, expectedTradeinIcoTopicIdList.Count);
            Assert.True(Enumerable.SequenceEqual(expectedTradeinIcoTopicIdList.OrderBy(x => x), actualTradeinIcoTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualTradeinIcoTopicIdList.ToList()));  //Buying Center Lead Email(3 types of emails - Consumer, Dealer, ADF - number of emails determined by participating dealer count) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "8754")]
        public void VerifyBuyingCenterIcoByVIN()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string buyingCenterIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoConsumerTopicId");
            string buyingCenterIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoDealerTopicId");
            string buyingCenterIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIco1STopicId");
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            #endregion

            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            icoLeadForm.Vin = vinGenerator.GetRandomVin().VinNumber;
            ico.EnterVinDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            int participatingDealerCount = ico.GetParticipatingDealerCount();
            ico.CloseIcoSuccessModal();
            Assert.True(ico.IsIcoModalClosed(), "Unable to close ICO success page");

            string icoOfferCode = statisticDatabase.GetIcoOfferCode(icoLeadForm.Email, EmailCreatedUtc);
            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            Assert.True(qatdratStorage.GetIcoOfferTableEntityByPartitionKey(icoOfferCode) != null, "IcoOfferTable entity is not found for submitted Instant Cash Offer");

            var expectedTradeinIcoTopicIdList = new List<string> { buyingCenterIcoConsumerTopicId };
            expectedTradeinIcoTopicIdList.AddRange(Enumerable.Repeat(buyingCenterIcoDealerTopicId, participatingDealerCount));
            expectedTradeinIcoTopicIdList.AddRange(Enumerable.Repeat(buyingCenterIco1STopicId, participatingDealerCount));
            var actualTradeinIcoTopicIdList = statisticDatabase.GetEmailTopicId(icoLeadForm.Email, EmailCreatedUtc, expectedTradeinIcoTopicIdList.Count);
            Assert.True(Enumerable.SequenceEqual(expectedTradeinIcoTopicIdList.OrderBy(x => x), actualTradeinIcoTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualTradeinIcoTopicIdList.ToList()));  //Buying Center ICO Lead Email(3 types of emails - Consumer, Dealer, ADF - number of emails determined by participating dealer count) Verification by Topic ID
        }

        
        [Test, Property("TestCaseId", "8678")]
        public void VerifyDwwIcoByVIN()
        {
            #region Variables
            string dwwLink = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.dwwLink");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string tradeinIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoConsumerTopicId");
            string tradeinIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIcoDealerTopicId");
            string tradeinIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.tradeinIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.tradeinIco1STopicId");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.DWW;
            #endregion
            url = new Uri(baseURL+ dwwLink);
            Open();

            icoLeadForm.Vin = vinGenerator.GetRandomVin().VinNumber;
            ico.EnterVinDetailsDWW(icoLeadForm);
            ico.EnterVehicleDetailsDWW(icoLeadForm);
            ico.EnterVehicleConditionsDWW(icoLeadForm);
            ico.EnterCustomerDetailsDWW(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsDWWIcoOfferAvailable(), "Failed to submit ICO. Instant cash offer not displayed.");


            string icoOfferCode = statisticDatabase.GetIcoOfferCode(icoLeadForm.Email, EmailCreatedUtc);
            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            Assert.True(qatdratStorage.GetIcoOfferTableEntityByPartitionKey(icoOfferCode) != null, "IcoOfferTable entity is not found for submitted Instant Cash Offer");

            var expectedTradeinIcoTopicIdList = new List<string> { tradeinIcoConsumerTopicId, tradeinIcoDealerTopicId, tradeinIco1STopicId };
            var actualTradeinIcoTopicIdList = statisticDatabase.GetEmailTopicId(icoLeadForm.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expectedTradeinIcoTopicIdList.OrderBy(x => x), actualTradeinIcoTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualTradeinIcoTopicIdList.ToList()));  //Trade-in ICO Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "11348")]
        public void VerifyUnableToGetOffer1()
        {
            #region Variables
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            icoLeadForm.Vin = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.vin");
            icoLeadForm.Kilometers = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.kilometers");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.postalCodeICO1");
            #endregion

            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterVinDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);

            Assert.True(ico.IsHighMileageOldVehiclePageDisplayed(), "Vehicle Condition Page is not displayed");
        }

        [Test, Property("TestCaseId", "11423")]
        public void VerifyUnableToGetOffer2()
        {
            #region Variables
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            icoLeadForm.Vin = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.vin");
            icoLeadForm.Kilometers = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.kilometers");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.postalCodeICO2");
            #endregion

            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterVinDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);

            Assert.True(ico.IsHighMileageOldVehiclePageDisplayed(), "Vehicle Condition Page is not displayed");
        }

        [Test, Property("TestCaseId", "11551")]
        public void VerifyElementsForHighMileageOldVehicle()
        {
            #region Variables
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            icoLeadForm.Vin = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.vin");
            icoLeadForm.Kilometers = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.kilometers");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.highMileageOldVehicle.postalCodeICO2");
            #endregion

            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterVinDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);

            Assert.True(ico.IsHighMileageOldVehiclePageDisplayed(), "Vehicle Condition Page is not displayed");
            Assert.True(ico.VerifyElementsOnHighMileageOldVehiclePage(), "All links are not available on High Mileage Old Vehicle Page");
        }


        [Test, Property("TestCaseId", "10711")]
        public void VerifyEmailPhoneMaskingSingleDealer()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string buyingCenterIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoConsumerTopicId");
            string buyingCenterIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoDealerTopicId");
            string buyingCenterIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIco1STopicId");
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoEmailPhoneMaskingPostalCode.singleDealerPostalCode");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
          
            //ppg settings for Single Dealer Original is 'Only OHM' so overriding the Year 
            // to make it qualify for OHM
            const int OHMyear = 13;
            icoLeadForm.Year = (DateTime.Now.Year - OHMyear).ToString();
            
            string expectedMaskedEmailSubString = "@beta.trader.ca";
            #endregion


            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);

            #region When VIN required Feature is ON
                //Enter Incorrect Vin 3 Times to get YMMT window
                if (isVinRequiredToggle)
                    ico.EnterIncorrectVinDetails();
                #endregion

            #region When VIN required Feature is OFF
                else
                    ico.ClickEnterYourVehicleMakeAndModelBtn();
                #endregion
              
            ico.EnterYmmtDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

         
            // getting the CouponId from Qatdrat using Consumer Email and Consumer Phone
            QatdratStorage azureStorageQatdrat = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "IcoOfferTable");
            var partitionKey = azureStorageQatdrat.GetPartitionKeyByConsumerEmailandPhone(icoLeadForm.Email, icoLeadForm.PhoneNumber).FirstOrDefault();


            QatdrTbybStorage azureStorageQatdrtbyb = new QatdrTbybStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrtbyb"), "IcoMaskingMapping");
           
            //waiting for CouponId to appear in ICO masking Mapping
            var couponId = azureStorageQatdrtbyb.GetCouponId(partitionKey);

            var proxyEmail = azureStorageQatdrtbyb.GetProxyEmails(icoLeadForm.Email);
            var proxyPhones = azureStorageQatdrtbyb.GetProxyPhoneNumber(icoLeadForm.PhoneNumber);
            var userOriginalPhoneNo = azureStorageQatdrtbyb.GetOriginalPhoneNumber(icoLeadForm.PhoneNumber);

            CollectionAssert.AreNotEqual(proxyEmail, icoLeadForm.Email, "The proxyEmail and OriginalEmail No should not be equal.");
            CollectionAssert.AreNotEqual(proxyPhones, userOriginalPhoneNo, "The proxyphones and useroriginalphone No should not be equal.");
            Assert.IsTrue(proxyEmail[0].Contains(expectedMaskedEmailSubString), $"ProxyEmail should contain {expectedMaskedEmailSubString}, but it did not.");
        }

        [Test, Property("TestCaseId", "10712")]
        public void VerifyEmailPhoneMaskingMultiDealer()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string buyingCenterIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoConsumerTopicId");
            string buyingCenterIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoDealerTopicId");
            string buyingCenterIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIco1STopicId");
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoEmailPhoneMaskingPostalCode.multiDealerPostalCode");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            string expectedMaskedEmailSubString = "@beta.trader.ca";
            #endregion


            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterYmmtDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            QatdrTbybStorage azureStorageQatdrtbyb = new QatdrTbybStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrtbyb"), "IcoMaskingMapping");
            var proxyPhones = azureStorageQatdrtbyb.GetProxyPhoneNumber(icoLeadForm.PhoneNumber);
            var userOriginalPhoneNo = azureStorageQatdrtbyb.GetOriginalPhoneNumber(icoLeadForm.PhoneNumber);
            var proxyEmails = azureStorageQatdrtbyb.GetProxyEmails(icoLeadForm.Email);
            var userOriginalEmail = azureStorageQatdrtbyb.GetUserOriginalEmails(icoLeadForm.Email);


            CollectionAssert.AreNotEqual(proxyPhones, userOriginalPhoneNo, "The proxyphones and userOriginalPhoneNo should not be equal.");
            CollectionAssert.AreNotEqual(proxyEmails, userOriginalEmail, "The proxyEmails and userOriginalEmail should not be equal.");

            foreach (string proxyEmail in proxyEmails)
            {
                Assert.IsTrue(proxyEmail.Contains(expectedMaskedEmailSubString), $"ProxyEmail should contain {expectedMaskedEmailSubString}, but it did not.");
            }


        }

        [Test, Property("TestCaseId", "10713")]
        public void VerifyNonProvisionedDealerNotMasked()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.adId");
            string icoBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferEN.icoBtnLabel") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.instantCashOfferFR.icoBtnLabel");
            string buyingCenterIcoConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoConsumerTopicId");
            string buyingCenterIcoDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIcoDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIcoDealerTopicId");
            string buyingCenterIco1STopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdLargeSmall.buyingCenterIco1STopicId") : GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.emailTopicId.topicIdXS.buyingCenterIco1STopicId");
            string icoPageUrl = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoPageUrl");
            icoLeadForm.PostalCode = GetTestData(testDataFile, "commonTestData.featureInstantCashOffer.icoEmailPhoneMaskingPostalCode.twoDealersPostalCode");
            icoLeadForm.icoLeadFormType = IcoLeadForm.IcoLeadFormType.BuyingCenter;
            #endregion


            url = new Uri(baseURL + icoPageUrl);
            Open();

            ico.ClickIcoButton(icoLeadForm);
            ico.EnterYmmtDetails(icoLeadForm);
            ico.EnterVehicleDetails(icoLeadForm);
            ico.EnterVehicleConditions(icoLeadForm);
            ico.EnterCustomerDetails(icoLeadForm);
            ico.SubmitPhoneCodeVerificationByText(icoLeadForm);
            Assert.True(ico.IsIcoSuccessPageDisplayed(), "Failed to submit ICO. Instant cash offer not displayed.");

            QatdrTbybStorage azureStorageQatdrtbyb = new QatdrTbybStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrtbyb"), "IcoMaskingMapping");
            var proxyPhones = azureStorageQatdrtbyb.GetProxyPhoneNumber(icoLeadForm.PhoneNumber);
            var userOriginalPhoneNo = azureStorageQatdrtbyb.GetOriginalPhoneNumber(icoLeadForm.PhoneNumber);
            var proxyEmails = azureStorageQatdrtbyb.GetProxyEmails(icoLeadForm.Email);


            CollectionAssert.AreNotEqual(proxyPhones, userOriginalPhoneNo, "The proxyphones and userOriginalPhoneNo should not be equal.");
            Assert.IsTrue(proxyPhones.Count == 2, "The number of proxyPhones should be 2.");
            Assert.IsTrue(userOriginalPhoneNo.Count == 2, "The number of userOriginalPhoneNo should be 2.");
            Assert.IsTrue(proxyEmails.Count == 2, "The number of proxyEmails should be 2.");

        }


    }

}

