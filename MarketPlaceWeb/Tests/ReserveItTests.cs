using MarketPlaceWeb.Base;
using MarketPlaceWeb.Base.SqlDatabase;
using UIAutomationLibrary.Base.AzureStorage;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIAutomationLibrary.Base.RestApi;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    class ReserveItTests : Page
    {
        VDPMain vdp;
        UIAutomationLibrary.Base.ClientApi3rdParty.Stripe stripe;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        ReserveVehicleForm reserveVehicleForm;
        JoinWaitListForm joinWaitListForm;
        StatisticDatabase statisticDatabase;
        ReserveVehicleApi restApi;
        DateTime EmailCreatedUtc;
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
            isIosDevice = (azureConfig.isAzureEnabled) ? (azureConfig.config == "ipad-safari-small" || azureConfig.config == "ios-safari-xs"): (localConfig.config == "ipad-safari-small" || localConfig.config == "ios-safari-xs");
            stripe = new UIAutomationLibrary.Base.ClientApi3rdParty.Stripe(GetTestData(testDataFile, "stripe.secretKey"), GetTestData(testDataFile, "stripe.accountId"));
            restApi = new ReserveVehicleApi(baseURL);
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            vdp = new VDPMain(driver, viewport, language);
            reserveVehicleForm = new ReserveVehicleForm
            {
                Name = "MP ReserveVehicleTest",
                Phone = GetTestData(testDataFile, "commonTestData.featureReserveVehicle.phone"),
                Email = "ReserveVehicle_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PostalCode = GetTestData(testDataFile, "commonTestData.featureReserveVehicle.postalCode"),
                CCNumber = GetTestData(testDataFile, "creditCard.ccNumber"),
                CCName = "Marketplace Cc",
                CCExpiry = GetTestData(testDataFile, "creditCard.ccExpiry"),
                CVV = GetTestData(testDataFile, "creditCard.CVV")
            };

            joinWaitListForm = new JoinWaitListForm
            {
                Name = "MP JoinWaitListTest",
                Email = "JoinWaitList_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                Phone = "6044444444"
            };

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
            restApi.Dispose();
            statisticDatabase.Dispose();
            driver.Quit();
        }

        [Test, Property("TestCaseId", "6768")]
        public void VerifyVehicleReservation()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6768.adId1") : GetTestData(testDataFile, "6768.adId2");
                string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
                string emailSubjectVehicleReservation = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectVehicleReservation") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectVehicleReservation");
                string emailSubjectReservationCancellation = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectReservationCancellation") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectReservationCancellation");
                string emailSubjectReminder = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectReminder") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectReminder");
                string vehicleReservationConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationConsumerTopicId");
                string vehicleReservationDealerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationDealerTopicId");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                vdp.ClickDepositReserveVehcileButton();
                Assert.True(vdp.VerifyDepositSuccess(), "Vehicle reservation was not successful");
                vdp.ClickDepositFinishButton();

                var stripeCharge = stripe.RetrieveChargeByAdId(adId);
                Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
                Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

                stripe.CaptureChargeByAdId(adId);  //Capture charge on Stripe
                stripe.CancelOrRefundChargeByAdId(adId);  //Refund charge on Stripe

                Assert.True(statisticDatabase.GetEmailLog(reserveVehicleForm.Email, emailSubjectVehicleReservation, EmailCreatedUtc).Count > 0, "'Vehicle reserved' email is not sent or email subject does not match");  //Consumer/Dealer Reservation Confirmation Email Verification

                var expectedReservationTopicIdList = new List<string> { vehicleReservationConsumerTopicId, vehicleReservationDealerTopicId };
                var actualReservationTopicIdList = statisticDatabase.GetEmailTopicId(reserveVehicleForm.Email, emailSubjectVehicleReservation, EmailCreatedUtc);
                Assert.True(expectedReservationTopicIdList.All(x => actualReservationTopicIdList.Contains(x)), "Vehicle Reservation topic Id not found: " + string.Join(", ", expectedReservationTopicIdList.Except(actualReservationTopicIdList).ToList()));  //Consumer and Dealer Reservation Confirmation Email Verification by Topic ID

                Assert.True(statisticDatabase.GetEmailLog(reserveVehicleForm.Email, emailSubjectReminder, EmailCreatedUtc).Count > 0, "'Your reserved vehicle is waiting for you' reminder email is not sent or email subject does not match");  //Consumer Reminder Email Verification

                Assert.True(statisticDatabase.GetEmailLog(reserveVehicleForm.Email, emailSubjectReservationCancellation, EmailCreatedUtc).Count > 0, "'Your reservation has been canceled' email is not sent or email subject does not match");  //Payment Cancellation Email Verification

                stripe.CancelOrRefundChargeByAdId(adId);
            }
        }

        [Test, Property("TestCaseId", "6766")]
        public void VerifyReserveVehicleTermsConditions()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6766.adId1") : GetTestData(testDataFile, "6766.adId2");
            string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
            string reserveVehicleTCHeader = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleTCHeader") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleTCHeader");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
            vdp.EnterBuyerInfo(reserveVehicleForm);
            vdp.ClickContinueToPaymentButton();
            vdp.ClickTermsConditionsLink();
            Assert.True(vdp.IsReserveItTCDisplayed(reserveVehicleTCHeader), "Reserve Vehicle Terms & Conditions page is not displayed");

            vdp.ClickBackButtonOnReserveItTCPage();
            Assert.True(vdp.IsDepositPageDisplayed(), "Back button on reserve vehile Terms & Conditions page not working");
        }

        [Test, Property("TestCaseId", "6767")]
        public void VerifyJoiningWaitList()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6767.adId1") : GetTestData(testDataFile, "6767.adId2");
                string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
                string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
                string joinWaitListSuccessMessage = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListSuccessMessage") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListSuccessMessage");
                string emailSubjectJoinWaitList = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectJoinWaitList") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectJoinWaitList");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                vdp.ClickDepositReserveVehcileButton();
                Assert.True(vdp.VerifyDepositSuccess(), "Vehicle reservation was not successful");
                vdp.ClickDepositFinishButton();

                var stripeCharge = stripe.RetrieveChargeByAdId(adId);
                Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
                Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

                Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel), "Join Waiting List button not displayed after vehicle reservation");
                vdp.ClickJoinWaitListButton();
                vdp.EnterWaitListInfo(joinWaitListForm);
                vdp.ClickWaitListSubmitButton();
                Assert.True(vdp.IsJoinWaitListSucceed(joinWaitListSuccessMessage), "Joining wait list was not successful");
                vdp.ClickJoinWaitListFinishButton();

                Assert.True(statisticDatabase.GetEmailLog(joinWaitListForm.Email, emailSubjectJoinWaitList, EmailCreatedUtc).Count > 0, "Join Waiting List email is not sent or email subject does not match");  //Join Waiting List Email Verification

                stripe.CancelOrRefundChargeByAdId(adId);
            }
        }

        [Test, Property("TestCaseId", "6773")]
        public void VerifyJoinWaitListPrivacyPolicy()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6773.adId1") : GetTestData(testDataFile, "6773.adId2");
                string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
                string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
                string privacyUrl = GetTestData(testDataFile, "6773.privacyUrl");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                vdp.ClickDepositReserveVehcileButton();
                Assert.True(vdp.VerifyDepositSuccess(), "Vehicle reservation was not successful");
                vdp.ClickDepositFinishButton();

                var stripeCharge = stripe.RetrieveChargeByAdId(adId);
                Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
                Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

                Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel), "Join Waiting List button not displayed after vehicle reservation");
                vdp.ClickJoinWaitListButton();
                vdp.ClickPrivacyPolicyLink();
                Assert.True(vdp.IsJoinWaitListPrivacyDisplayed(privacyUrl), "Join Waiting List Privacy Policy page is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId);
            }
            
        }

        [Test, Property("TestCaseId", "6772")]
        public void CancelReserveVehicleFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6772.adId1") : GetTestData(testDataFile, "6772.adId2");
            string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
            vdp.EnterBuyerInfo(reserveVehicleForm);
            vdp.ClickContinueToPaymentButton();
            vdp.ClickDepositBackButton();
            Assert.True(vdp.IsBuyerInfoPageDisplayed(), "Back button does not work on Reserve It deposit page");

            vdp.ClickBuyerInfoCancelButton();
            Assert.True(vdp.IsReserveVehicleModalAvailable(false), "Cancel button does not work on Reserve It Buyer Info page");
        }

        [Test, Property("TestCaseId", "6764")]
        public void VerifyOnlineReservationCSBadge()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6764.adId1") : GetTestData(testDataFile, "6764.adId2");
                string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickOnlineReservationBadge();
                Assert.True(vdp.IsReserveVehicleModalAvailable(), "Online Reservation CS badge does not open Reserve Vehicle modal");
                Assert.True(vdp.IsBuyerInfoPageDisplayed(), "Online Reservation CS badge does not open Reserve Vehicle Buyer Info page");

                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                vdp.ClickDepositReserveVehcileButton();
                Assert.True(vdp.VerifyDepositSuccess(), "Vehicle reservation was not successful");
                vdp.ClickDepositFinishButton();
                Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel), "Join Waiting List button not displayed after vehicle reservation");

                vdp.ClickOnlineReservationBadge();
                Assert.True(vdp.IsReserveVehicleModalAvailable(), "Online Reservation CS badge does not open Join Waiting List modal");
                Assert.True(vdp.IsJoinWaitListBuyerInfoPageVisible(), "Online Reservation CS badge does not open Join Waiting List Buyer Info page");

                stripe.CancelOrRefundChargeByAdId(adId);
            }
        }

        [Test, Property("TestCaseId", "6765")]
        public void ReserveVehicleErrorValidations()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "6765.adId1") : GetTestData(testDataFile, "6765.adId2");
                string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);

                vdp.ClickBuyerInfoNameField();
                vdp.ClickBuyerInfoPhoneField();
                Assert.True(vdp.IsBuyerInfoNameFieldErrorDisplayed(), "Buyer Info name field error validation message not displayed");
                vdp.ClickBuyerInfoNameField();
                Assert.True(vdp.IsBuyerInfoPhoneFieldErrorDisplayed(), "Buyer Info phone field error validation message not displayed");

                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                Assert.True(vdp.IsDepositPageDisplayed(), "Unable to proceed to deposit page after filling up buyer info page post error validation");

                vdp.ClickDepositEmail();
                vdp.ClickDepositPostalCode();
                vdp.ClickDepositCCNumber();
                vdp.ClickDepositCCName();
                vdp.ClickDepositCCExpiry();
                vdp.ClickDepositCVV();
                vdp.CheckDepositTerms();
                vdp.CheckDepositTerms(false);
                Assert.True(vdp.IsDepositEmailFieldErrorDisplayed(), "Deposit email field error validation message not displayed");
                Assert.True(vdp.IsDepositPostalCodeFieldErrorDisplayed(), "Deposit postal code field error validation message not displayed");
                Assert.True(vdp.IsDepositCCNumberFieldErrorDisplayed(), "Deposit credit card number field error validation message not displayed");
                Assert.True(vdp.IsDepositCCNameFieldErrorDisplayed(), "Deposit credit card name field error validation message not displayed");
                Assert.True(vdp.IsDepositCCExpiryFieldErrorDisplayed(), "Deposit credit card expiry field error validation message not displayed");
                Assert.True(vdp.IsDepositCVVFieldErrorDisplayed(), "Deposit credit card CVV field error validation message not displayed");
                Assert.True(vdp.IsDepositTermsCheckboxErrorDisplayed(), "Deposit terms checkbox error validation message not displayed");

                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                Assert.True(vdp.IsReserveVehicleButtonEnabled(), "Reserve this vehicle button is not enabled after filling up deposit page post error validation");
            }
            
        }

        [Test, Property("TestCaseId", "8297")]
        public void VerifyReservationExpiredEmailForReserveIt()
        {
            if (!isIosDevice)
            {
                #region Variables
                string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "8297.adId1") : GetTestData(testDataFile, "8297.adId2");
                string reserveVehicleBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.reserveVehicleBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.reserveVehicleBtnLabel");
                string expiredReservationEndPoint = GetTestData(testDataFile, "restApi.featureReserveVehicle.apiEndpoints.expiredReservationEndPoint");
                string reservationExpiredTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.reservationExpiredTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.reservationExpiredTopicId");
                string emailSubjectReservationExpired = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectReservationExpired") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectReservationExpired");
                #endregion

                stripe.CancelOrRefundChargeByAdId(adId);
                vdp.GoToVDP(baseURL, adId);
                vdp.ClickReserveVehicleButton(reserveVehicleBtnLabel);
                vdp.EnterBuyerInfo(reserveVehicleForm);
                vdp.ClickContinueToPaymentButton();
                vdp.EnterDepositInfo(reserveVehicleForm);
                vdp.CheckDepositTerms();
                vdp.ClickDepositReserveVehcileButton();
                Assert.True(vdp.VerifyDepositSuccess(), "Vehicle reservation was not successful");
                vdp.ClickDepositFinishButton();

                var stripeCharge = stripe.RetrieveChargeByAdId(adId);
                Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
                Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

                QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "ReservationStatus");
                DateTime reservationDateTime = qatdratStorage.GetReservationDateByChargeId(stripeCharge.Id);
                Assert.False(reservationDateTime.Equals(default), "ReservationStatus Azure storage table query execution timed out");
                qatdratStorage.UpdateExpirationDateByChargeId(stripeCharge.Id, reservationDateTime.AddDays(-1));

                var apiResponse = restApi.ExpiredReservationsApi(expiredReservationEndPoint);
                Assert.True(apiResponse != null, "Expired Reservation API response status not OK");

                Assert.True(statisticDatabase.GetEmailTopicId(reserveVehicleForm.Email, emailSubjectReservationExpired, EmailCreatedUtc).Contains(reservationExpiredTopicId));
            }
        }
    }
}