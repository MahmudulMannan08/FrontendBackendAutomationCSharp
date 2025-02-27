using MarketPlaceWeb.Base;
using MarketPlaceWeb.Base.SqlDatabase;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages.TryBeforeYouBuy;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UIAutomationLibrary.Base.AzureStorage;
using UIAutomationLibrary.Base.RestApi;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class TryBeforeYouBuyTests : Page
    {
        string baseURL;
        dynamic testDataFile;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        VDPMain vdp;
        TryBeforeYouBuyMain tbyb;
        TbybPersonalDetails tbybPersonalDetails;
        TbybDeliveryDetails tbybDeliveryDetails;
        TbybDeposit tbybDeposit;
        UIAutomationLibrary.Base.ClientApi3rdParty.Stripe stripe;
        //StatisticDatabase statisticDatabase;
        ReserveVehicleApi restApi;
        DateTime EmailCreatedUtc;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            testDataFile = GetTestDataFile(baseURL);

            stripe = new UIAutomationLibrary.Base.ClientApi3rdParty.Stripe(GetTestData(testDataFile, "stripe.secretKey"), GetTestData(testDataFile, "stripe.accountId"));
            restApi = new ReserveVehicleApi(baseURL);
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            vdp = new VDPMain(driver, viewport, language);
            tbyb = new TryBeforeYouBuyMain(driver, viewport, language);

            tbybPersonalDetails = new TbybPersonalDetails
            {
                FirstName = "MP",
                LastName = "TryBeforeYouBuyTest",
                EmailAddress = "TryBeforeYouBuy_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureTbyb.phoneNumber"),
            };
            tbybDeliveryDetails = new TbybDeliveryDetails
            {
                StreetAddress = "1370 Dundas Street East",
                UnitNumber = "6",
                City = GetTestData(testDataFile, "commonTestData.featureTbyb.city"),
                PostalCode = GetTestData(testDataFile, "commonTestData.featureTbyb.postalCode")
            };
            tbybDeposit = new TbybDeposit
            {
                CCNumber = GetTestData(testDataFile, "creditCard.ccNumber"),
                CCName = "Marketplace Cc",
                CCExpiry = GetTestData(testDataFile, "creditCard.ccExpiry"),
                CVV = GetTestData(testDataFile, "creditCard.CVV"),
                PostalCode = GetTestData(testDataFile, "commonTestData.featureTbyb.postalCode")
            };

            //statisticDatabase = new StatisticDatabase(driver, GetTestData(testDataFile, "connectionStrings.azrmdbqa1Statistic"));
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
            //statisticDatabase.Dispose();
            driver.Quit();
        }

        [Test, Property("TestCaseId", "7538")]
        public void VerifyTbybDeliveryFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "7538.adId1") : GetTestData(testDataFile, "7538.adId2");
            string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
            string emailSubjectVehicleReservation = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectVehicleReservation") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectVehicleReservation");
            string vehicleReservationConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationConsumerTopicId");
            string vehicleReservationReminderTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationReminderTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationReminderTopicId");
            string vehicleReservationCancellationTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationCancellationTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationCancellationTopicId");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.IsTbybButtonEnabled(), "Try Before You Buy button is disabled. Stripe charge cancel/refund failed.");

            vdp.ClickTbybButton();
            Assert.True(tbyb.IsRedirectedToTbybFunnel(), "Try Before You Buy button is not redirecting to TBYB funnel");

            tbyb.EnterPersonalDetails(tbybPersonalDetails);
            tbyb.CheckTermsConditions();
            tbyb.CheckCommunications();
            Assert.True(tbyb.IsNextDeliveryDetailsBtnEnabled(), "Next: Delivery Details button is not enabled after filling up personal details");

            tbyb.ClickNextDeliveryDetailsBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Next: Delivery Details button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.PersonalDetailsProgress), "Personal Details status indicator does not show complete after moving to Delivery Details"); }

            tbyb.SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType.Delivery);
            tbyb.EnterDeliveryAddress(tbybDeliveryDetails);
            DeliveryPickupDayDate deliveryPickupDayDate = tbyb.SelectFirstAvailableDate();
            string deliveryPickupTime = tbyb.SelectFirstAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button is not enabled after filling up delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.DeliveryDetailsProgress), "Delivery Details and Personal Details status indicator does not show complete after moving to Deposit"); }

            tbyb.EnterDepositDetails(tbybDeposit);
            Assert.True(tbyb.IsPlaceDepositBtnEnabled(), "Place Deposit button is not enabled after filling up deposit details");

            tbyb.ClickPlaceDepositBtn();
            Assert.True(tbyb.IsRedirectedToCongratulations(), "Place deposit failed");

            Assert.AreEqual(tbyb.GetAddressOnCongratulations(), tbybDeliveryDetails.StreetAddress + ", " + tbybDeliveryDetails.City + ", " + tbybDeliveryDetails.PostalCode, "Address on Congratulations page does not match with consumer provided delivery address");

            DeliveryPickupMonthYear deliveryPickupMonthYear = tbyb.GetDeliveryPickupMonthYear(deliveryPickupDayDate.Day, deliveryPickupDayDate.Date);
            string deliveryPickupDay = (language.ToString() == "EN") ? DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US")) : DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US"));  //Bug: French Congratulations page shows English Day https://trader.atlassian.net/browse/DR-704  //DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("fr-FR"))
            Assert.AreEqual(tbyb.GetDateTimeOnCongratulations(), (language.ToString() == "EN") ? deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime : deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime, "Date & Time on Congratulations page does not match with consumer selected Date & Time for delivery");

            tbyb.ClickBackToListingBtn();
            Assert.True(IsInCurrentUrl(adId), "Back To Listing button click failed OR does not redirect to correct VDP");
            Assert.True(vdp.IsTbybButtonEnabled(false), "Try Before You Buy button is not disabled after reserving vehicle through Tbyb");
            Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel, true), "Join Waiting List widget/button not displayed after reserving vehicle through Tbyb");

            var stripeCharge = stripe.RetrieveChargeByAdId(adId);
            Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
            Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

            stripe.CaptureChargeByAdId(adId);  //Capture charge on Stripe
            stripe.CancelOrRefundChargeByAdId(adId);  //Refund charge on Stripe

            //Assert.True(statisticDatabase.GetEmailLog(tbybPersonalDetails.EmailAddress, emailSubjectVehicleReservation, EmailCreatedUtc).Count > 0, "'Vehicle reserved' email is not sent or email subject does not match");  //Consumer/Dealer Reservation Confirmation Email Verification

            //var expectedReservationTopicIdList = new List<string> { vehicleReservationConsumerTopicId, vehicleReservationReminderTopicId, vehicleReservationCancellationTopicId };
            //var actualReservationTopicIdList = statisticDatabase.GetEmailTopicId(tbybPersonalDetails.EmailAddress, EmailCreatedUtc, 3);  //Bug: Reminder email is sent twice for TBYB sometimes
            //Assert.True(expectedReservationTopicIdList.All(x => actualReservationTopicIdList.Contains(x)), "Vehicle Reservation topic Id not found: " + string.Join(", ", expectedReservationTopicIdList.Except(actualReservationTopicIdList).ToList()));  //Consumer Reservation Confirmation, Consumer Reminder, Payment Cancellation Emails Verification by Topic ID
        }

        [Test, Property("TestCaseId", "7539")]
        public void VerifyTbybPickupFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "7539.adId1") : GetTestData(testDataFile, "7539.adId2");
            string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
            string emailSubjectVehicleReservation = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectVehicleReservation") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectVehicleReservation");
            string vehicleReservationConsumerTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationConsumerTopicId");
            string vehicleReservationReminderTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationReminderTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationReminderTopicId");
            string vehicleReservationCancellationTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.vehicleReservationCancellationTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.vehicleReservationCancellationTopicId");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.IsTbybButtonEnabled(), "Try Before You Buy button is disabled. Stripe charge cancel/refund failed.");

            vdp.ClickTbybButton();
            Assert.True(tbyb.IsRedirectedToTbybFunnel(), "Try Before You Buy button is not redirecting to TBYB funnel");

            tbyb.EnterPersonalDetails(tbybPersonalDetails);
            tbyb.CheckTermsConditions();
            tbyb.CheckCommunications();
            Assert.True(tbyb.IsNextDeliveryDetailsBtnEnabled(), "Next: Delivery Details button is not enabled after filling up personal details");

            tbyb.ClickNextDeliveryDetailsBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Next: Delivery Details button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.PersonalDetailsProgress), "Personal Details status indicator does not show complete after moving to Delivery Details"); }

            tbyb.SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType.Pickup);
            Assert.True(tbyb.IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType.Pickup), "Pickup tab click failed");

            string dealerPickupAddress = tbyb.GetAddressOnPickup();
            DeliveryPickupDayDate deliveryPickupDayDate = tbyb.SelectFirstAvailableDate();
            string deliveryPickupTime = tbyb.SelectFirstAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button is not enabled after filling up delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.DeliveryDetailsProgress), "Delivery Details and Personal Details status indicator does not show complete after moving to Deposit"); }

            tbyb.EnterDepositDetails(tbybDeposit);
            Assert.True(tbyb.IsPlaceDepositBtnEnabled(), "Place Deposit button is not enabled after filling up deposit details");

            tbyb.ClickPlaceDepositBtn();
            Assert.True(tbyb.IsRedirectedToCongratulations(), "Place deposit failed");

            Assert.True(tbyb.GetAddressOnCongratulations().Contains(dealerPickupAddress), "Address on Congratulations page does not match with dealer pickup address");

            DeliveryPickupMonthYear deliveryPickupMonthYear = tbyb.GetDeliveryPickupMonthYear(deliveryPickupDayDate.Day, deliveryPickupDayDate.Date);
            string deliveryPickupDay = (language.ToString() == "EN") ? DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US")) : DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US"));  //Bug: French Congratulations page shows English Day https://trader.atlassian.net/browse/DR-704  //DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("fr-FR"))
            Assert.AreEqual(tbyb.GetDateTimeOnCongratulations(), (language.ToString() == "EN") ? deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime : deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime, "Date & Time on Congratulations page does not match with consumer selected Date & Time for pickup");

            tbyb.ClickBackToListingBtn();
            Assert.True(IsInCurrentUrl(adId), "Back To Listing button click failed OR does not redirect to correct VDP");
            Assert.True(vdp.IsTbybButtonEnabled(false), "Try Before You Buy button is not disabled after reserving vehicle through Tbyb");
            Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel, true), "Join Waiting List widget/button not displayed after reserving vehicle through Tbyb");

            var stripeCharge = stripe.RetrieveChargeByAdId(adId);
            Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
            Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

            stripe.CaptureChargeByAdId(adId);  //Capture charge on Stripe
            stripe.CancelOrRefundChargeByAdId(adId);  //Refund charge on Stripe

            //Assert.True(statisticDatabase.GetEmailLog(tbybPersonalDetails.EmailAddress, emailSubjectVehicleReservation, EmailCreatedUtc).Count > 0, "'Vehicle reserved' email is not sent or email subject does not match");  //Consumer/Dealer Reservation Confirmation Email Verification

            //var expectedReservationTopicIdList = new List<string> { vehicleReservationConsumerTopicId, vehicleReservationReminderTopicId, vehicleReservationCancellationTopicId };
            //var actualReservationTopicIdList = statisticDatabase.GetEmailTopicId(tbybPersonalDetails.EmailAddress, EmailCreatedUtc, 3);  //Bug: Reminder email is sent twice for TBYB sometimes
            //Assert.True(expectedReservationTopicIdList.All(x => actualReservationTopicIdList.Contains(x)), "Vehicle Reservation topic Id not found: " + string.Join(", ", expectedReservationTopicIdList.Except(actualReservationTopicIdList).ToList()));  //Consumer Reservation Confirmation, Consumer Reminder, Payment Cancellation Emails Verification by Topic ID
        }

        [Test, Property("TestCaseId", "7540")]
        public void VerifyTbybUpdatedDeliveryFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "7540.adId1") : GetTestData(testDataFile, "7540.adId2");
            string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.IsTbybButtonEnabled(), "Try Before You Buy button is disabled. Stripe charge cancel/refund failed.");

            vdp.ClickTbybButton();
            Assert.True(tbyb.IsRedirectedToTbybFunnel(), "Try Before You Buy button is not redirecting to TBYB funnel");

            tbyb.EnterPersonalDetails(tbybPersonalDetails);
            tbyb.CheckTermsConditions();
            tbyb.CheckCommunications();
            Assert.True(tbyb.IsNextDeliveryDetailsBtnEnabled(), "Next: Delivery Details button is not enabled after filling up personal details");

            tbyb.ClickNextDeliveryDetailsBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Next: Delivery Details button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.PersonalDetailsProgress), "Personal Details status indicator does not show complete after moving to Delivery Details"); }

            tbyb.SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType.Delivery);
            tbyb.EnterDeliveryAddress(tbybDeliveryDetails);
            tbyb.SelectFirstAvailableDate();
            tbyb.SelectFirstAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button is not enabled after filling up delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");

            tbyb.ClickDepositBackBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Deposit page Back button failed");
            Assert.True(tbyb.IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType.Delivery), "Flow type selection is not persistent");

            tbybDeliveryDetails = new TbybDeliveryDetails
            {
                StreetAddress = "81 Bay Street",
                UnitNumber = "1",
                City = GetTestData(testDataFile, "commonTestData.featureTbyb.updatedCity"),
                PostalCode = GetTestData(testDataFile, "commonTestData.featureTbyb.updatedPostalCode")
            };
            tbyb.EnterDeliveryAddress(tbybDeliveryDetails);
            DeliveryPickupDayDate updatedDeliveryPickupDayDate = tbyb.SelectLastAvailableDate();
            string updatedDeliveryPickupTime = tbyb.SelectLastAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button does not remain enabled after updating delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.DeliveryDetailsProgress), "Delivery Details and Personal Details status indicator does not show complete after moving to Deposit"); }

            tbyb.EnterDepositDetails(tbybDeposit);
            Assert.True(tbyb.IsPlaceDepositBtnEnabled(), "Place Deposit button is not enabled after filling up deposit details");

            tbyb.ClickPlaceDepositBtn();
            Assert.True(tbyb.IsRedirectedToCongratulations(), "Place deposit failed");

            Assert.AreEqual(tbyb.GetAddressOnCongratulations(), tbybDeliveryDetails.StreetAddress + ", " + tbybDeliveryDetails.City + ", " + tbybDeliveryDetails.PostalCode, "Address on Congratulations page does not match with updated delivery address");

            DeliveryPickupMonthYear deliveryPickupMonthYear = tbyb.GetDeliveryPickupMonthYear(updatedDeliveryPickupDayDate.Day, updatedDeliveryPickupDayDate.Date);
            string deliveryPickupDay = (language.ToString() == "EN") ? DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US")) : DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US"));  //Bug: French Congratulations page shows English Day https://trader.atlassian.net/browse/DR-704  //DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("fr-FR"))
            Assert.AreEqual(tbyb.GetDateTimeOnCongratulations(), (language.ToString() == "EN") ? deliveryPickupDay + " " + updatedDeliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + updatedDeliveryPickupTime : deliveryPickupDay + " " + updatedDeliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + updatedDeliveryPickupTime, "Date & Time on Congratulations page does not match with updated Date & Time for delivery");

            tbyb.ClickBackToListingBtn();
            Assert.True(IsInCurrentUrl(adId), "Back To Listing button click failed OR does not redirect to correct VDP");
            Assert.True(vdp.IsTbybButtonEnabled(false), "Try Before You Buy button is not disabled after reserving vehicle through Tbyb");
            Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel, true), "Join Waiting List widget/button not displayed after reserving vehicle through Tbyb");

            var stripeCharge = stripe.RetrieveChargeByAdId(adId);
            Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
            Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

            stripe.CancelOrRefundChargeByAdId(adId);  //Refund charge on Stripe
        }

        [Test, Property("TestCaseId", "7541")]
        public void VerifyTbybUpdatedPickupFlow()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "7541.adId1") : GetTestData(testDataFile, "7541.adId2");
            string joinWaitListBtnLabel = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.joinWaitListBtnLabel") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.joinWaitListBtnLabel");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.IsTbybButtonEnabled(), "Try Before You Buy button is disabled. Stripe charge cancel/refund failed.");

            vdp.ClickTbybButton();
            Assert.True(tbyb.IsRedirectedToTbybFunnel(), "Try Before You Buy button is not redirecting to TBYB funnel");

            tbyb.EnterPersonalDetails(tbybPersonalDetails);
            tbyb.CheckTermsConditions();
            tbyb.CheckCommunications();
            Assert.True(tbyb.IsNextDeliveryDetailsBtnEnabled(), "Next: Delivery Details button is not enabled after filling up personal details");

            tbyb.ClickNextDeliveryDetailsBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Next: Delivery Details button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.PersonalDetailsProgress), "Personal Details status indicator does not show complete after moving to Delivery Details"); }

            tbyb.SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType.Pickup);
            Assert.True(tbyb.IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType.Pickup), "Pickup tab click failed");

            string dealerPickupAddress = tbyb.GetAddressOnPickup();
            tbyb.SelectFirstAvailableDate();
            tbyb.SelectFirstAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button is not enabled after filling up delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");

            tbyb.ClickDepositBackBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Deposit page Back button failed");
            Assert.True(tbyb.IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType.Pickup), "Flow type selection is not persistent");

            DeliveryPickupDayDate updatedDeliveryPickupDayDate = tbyb.SelectLastAvailableDate();
            string updatedDeliveryPickupTime = tbyb.SelectLastAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button does not remain enabled after updating delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.DeliveryDetailsProgress), "Delivery Details and Personal Details status indicator does not show complete after moving to Deposit"); }

            tbyb.EnterDepositDetails(tbybDeposit);
            Assert.True(tbyb.IsPlaceDepositBtnEnabled(), "Place Deposit button is not enabled after filling up deposit details");

            tbyb.ClickPlaceDepositBtn();
            Assert.True(tbyb.IsRedirectedToCongratulations(), "Place deposit failed");

            Assert.True(tbyb.GetAddressOnCongratulations().Contains(dealerPickupAddress), "Address on Congratulations page does not match with dealer pickup address");

            DeliveryPickupMonthYear deliveryPickupMonthYear = tbyb.GetDeliveryPickupMonthYear(updatedDeliveryPickupDayDate.Day, updatedDeliveryPickupDayDate.Date);
            string deliveryPickupDay = (language.ToString() == "EN") ? DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US")) : DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US"));  //Bug: French Congratulations page shows English Day https://trader.atlassian.net/browse/DR-704  //DateTime.Parse(deliveryPickupMonthYear.Month + "/" + updatedDeliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("fr-FR"))
            Assert.AreEqual(tbyb.GetDateTimeOnCongratulations(), (language.ToString() == "EN") ? deliveryPickupDay + " " + updatedDeliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + updatedDeliveryPickupTime : deliveryPickupDay + " " + updatedDeliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + updatedDeliveryPickupTime, "Date & Time on Congratulations page does not match with updated Date & Time for pickup");

            tbyb.ClickBackToListingBtn();
            Assert.True(IsInCurrentUrl(adId), "Back To Listing button click failed OR does not redirect to correct VDP");
            Assert.True(vdp.IsTbybButtonEnabled(false), "Try Before You Buy button is not disabled after reserving vehicle through Tbyb");
            Assert.True(vdp.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel, true), "Join Waiting List widget/button not displayed after reserving vehicle through Tbyb");

            var stripeCharge = stripe.RetrieveChargeByAdId(adId);
            Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
            Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

            stripe.CancelOrRefundChargeByAdId(adId);  //Refund charge on Stripe
        }

        [Test, Property("TestCaseId", "8157")]
        public void VerifyTbybStickyCta()
        {
            #region Variables
            string adId1 = GetTestData(testDataFile, "8157.adId1");  //TBYB = Enabled, Email = Enabled, Request Text = Disabled, Text = Disabled, Call = Disabled
            string adId2 = GetTestData(testDataFile, "8157.adId2");  //TBYB = Enabled, Email = Disabled, Request Text = Enabled, Text = Disabled, Call = Disabled
            string adId3 = GetTestData(testDataFile, "8157.adId3");  //TBYB = Disabled, Email = Disabled, Request Text = Disabled, Text = Enabled, Call = Disabled
            string adId4 = GetTestData(testDataFile, "8157.adId4");  //TBYB = Enabled, Email = Disabled, Request Text = Disabled, Text = Disabled, Call = Enabled
            string adId5 = GetTestData(testDataFile, "8157.adId5");  //TBYB = Enabled, Email = Enabled, Request Text = Enabled, Text = Disabled, Call = Disabled
            string adId6 = GetTestData(testDataFile, "8157.adId6");  //TBYB = Enabled, Email = Enabled, Request Text = Disabled, Text = Enabled, Call = Disabled
            string adId7 = GetTestData(testDataFile, "8157.adId7");  //TBYB = Enabled, Email = Enabled, Request Text = Disabled, Text = Disabled, Call = Enabled
            string adId8 = GetTestData(testDataFile, "8157.adId8");  //TBYB = Enabled, Email = Disabled, Request Text = Enabled, Text = Disabled, Call = Enabled
            string adId9 = GetTestData(testDataFile, "8157.adId9");  //TBYB = Enabled, Email = Disabled, Request Text = Disabled, Text = Enabled, Call = Enabled
            string adId10 = GetTestData(testDataFile, "8157.adId10");  //TBYB = Enabled, Email = Enabled, Request Text = Enabled, Text = Disabled, Call = Enabled
            string adId11 = GetTestData(testDataFile, "8157.adId11");  //TBYB = Enabled, Email = Enabled, Request Text = Disabled, Text = Enabled, Call = Enabled
            string adId12 = GetTestData(testDataFile, "8157.adId12");  //TBYB = Enabled, Email = Disabled, Request Text = Disabled, Text = Disabled, Call = Disabled
            string adId13 = GetTestData(testDataFile, "8157.adId13");  //TBYB = Disabled, Email = Disabled, Request Text = Disabled, Text = Disabled, Call = Disabled
            string adId14 = GetTestData(testDataFile, "8157.adId14");  //TBYB = Disabled, Email = Enabled, Request Text = Enabled, Text = Disabled, Call = Enabled
            #endregion

            Assert.Multiple(() => {
                stripe.CancelOrRefundChargeByAdId(adId1);
                vdp.GoToVDP(baseURL, adId1);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad1: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.EmailSeller), "ad1: Email Seller sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.TryBeforeYouBuy), "ad1: Try Before You Buy sticky CTA button is not enabled");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.EmailSeller), "ad1: Email Seller sticky CTA button is not enabled");

                vdp.ClickStickyButton(StickyButtons.EmailSeller);
                Assert.True(vdp.IsVdpLeadModalOpen(), "ad1: Failed to open Email tab");

                vdp.ClickEmailDialogueCloseBtn();
                Assert.True(vdp.IsVdpLeadModalOpen(false), "ad1: Failed to close Email tab");

                vdp.ClickStickyButton(StickyButtons.TryBeforeYouBuy);
                Assert.True(tbyb.IsRedirectedToTbybFunnel(), "ad1: Try Before You Buy sticky button is not redirecting to TBYB funnel");

                tbyb.ClickBackIconTbybFunnel();
                Assert.True(IsInCurrentUrl(adId1), "ad1: Failed to close TBYB funnel");

                stripe.CancelOrRefundChargeByAdId(adId2);
                vdp.GoToVDP(baseURL, adId2);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad2: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Text), "ad2: Text sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.TryBeforeYouBuy), "ad2: Try Before You Buy sticky CTA button is not enabled");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Text), "ad2: Text sticky CTA button is not enabled");

                vdp.ClickStickyButton(StickyButtons.Text);
                Assert.True(vdp.IsVdpTextModalOpen(), "ad2: Failed to open Request Text tab");

                vdp.ClickRequestTextModalCloseBtn();
                Assert.True(vdp.IsVdpTextModalOpen(false), "ad2: Failed to close Request Text tab");

                stripe.CancelOrRefundChargeByAdId(adId3);
                vdp.GoToVDP(baseURL, adId3);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy, false), "ad3: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Text), "ad3: Text sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Text), "ad3: Text sticky CTA button is not enabled");

                vdp.ClickStickyButton(StickyButtons.Text);
                Assert.True(vdp.IsVdpTextModalOpen(), "ad3: Failed to open Text tab");

                vdp.ClickRequestTextModalCloseBtn();
                Assert.True(vdp.IsVdpTextModalOpen(false), "ad3: Failed to close Text tab");

                stripe.CancelOrRefundChargeByAdId(adId4);
                vdp.GoToVDP(baseURL, adId4);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad4: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Call), "ad4: Call sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.TryBeforeYouBuy), "ad4: Try Before You Buy sticky CTA button is not enabled");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Call), "ad4: Call sticky CTA button is not enabled");

                stripe.CancelOrRefundChargeByAdId(adId5);
                vdp.GoToVDP(baseURL, adId5);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad5: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad5: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Email), "ad5: Contact Seller Email option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad5: Contact Seller Email option is not displayed");

                vdp.ClickContactSellerOption(ContactSellerButtons.Email);
                Assert.True(vdp.IsVdpLeadModalOpen(), "ad5: Failed to open Email tab");

                vdp.ClickEmailDialogueCloseBtn();
                Assert.True(vdp.IsVdpLeadModalOpen(false), "ad5: Failed to close Email tab");

                vdp.ClickContactSellerOption(ContactSellerButtons.Text);
                Assert.True(vdp.IsVdpTextModalOpen(), "ad5: Failed to open Request Text tab");

                vdp.ClickRequestTextModalCloseBtn();
                Assert.True(vdp.IsVdpTextModalOpen(false), "ad5: Failed to close Request Text tab");

                stripe.CancelOrRefundChargeByAdId(adId6);
                vdp.GoToVDP(baseURL, adId6);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad6: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad6: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Email), "ad6: Contact Seller Email option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad6: Contact Seller Email option is not displayed");

                vdp.ClickContactSellerOption(ContactSellerButtons.Text);
                Assert.True(vdp.IsVdpTextModalOpen(), "ad6: Failed to open Text tab");

                vdp.ClickRequestTextModalCloseBtn();
                Assert.True(vdp.IsVdpTextModalOpen(false), "ad6: Failed to close Text tab");

                stripe.CancelOrRefundChargeByAdId(adId7);
                vdp.GoToVDP(baseURL, adId7);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad7: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad7: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Email), "ad7: Contact Seller Email option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Call), "ad7: Contact Seller Call option is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId8);
                vdp.GoToVDP(baseURL, adId8);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad8: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad8: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad8: Contact Seller Text option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Call), "ad8: Contact Seller Call option is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId9);
                vdp.GoToVDP(baseURL, adId9);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad9: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad9: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad9: Contact Seller Text option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Call), "ad9: Contact Seller Call option is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId10);
                vdp.GoToVDP(baseURL, adId10);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad10: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad10: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Email), "ad10: Contact Seller Email option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad10: Contact Seller Text option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Call), "ad10: Contact Seller Call option is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId11);
                vdp.GoToVDP(baseURL, adId11);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad11: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller), "ad11: Contact Seller sticky CTA button is not displayed");

                vdp.ClickStickyButton(StickyButtons.ContactSeller);
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Email), "ad11: Contact Seller Email option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Text), "ad11: Contact Seller Text option is not displayed");
                Assert.True(vdp.IsContactSellerOptionsDisplayed(ContactSellerButtons.Call), "ad11: Contact Seller Call option is not displayed");

                stripe.CancelOrRefundChargeByAdId(adId12);
                vdp.GoToVDP(baseURL, adId12);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy), "ad12: Try Before You Buy sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller, false), "ad12: Contact Seller sticky CTA button is not supposed to display");

                stripe.CancelOrRefundChargeByAdId(adId13);
                vdp.GoToVDP(baseURL, adId13);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.TryBeforeYouBuy, false), "ad13: Try Before You Buy sticky CTA button is not supposed to display");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.ContactSeller, false), "ad13: Contact Seller sticky CTA button is not supposed to display");

                stripe.CancelOrRefundChargeByAdId(adId14);
                vdp.GoToVDP(baseURL, adId14);
                ScrollToBottom();
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Call), "ad14: Call sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Email), "ad14: Email sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonDisplayed(StickyButtons.Text), "ad14: Text sticky CTA button is not displayed");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Call), "ad14: Call sticky CTA button is not enabled");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Email), "ad14: Email sticky CTA button is not enabled");
                Assert.True(vdp.IsStickyButtonEnabled(StickyButtons.Text), "ad14: Text sticky CTA button is not enabled");

                vdp.ClickStickyButton(StickyButtons.Email);
                Assert.True(vdp.IsVdpLeadModalOpen(), "ad14: Failed to open Email tab");

                vdp.ClickEmailDialogueCloseBtn();
                Assert.True(vdp.IsVdpLeadModalOpen(false), "ad14: Failed to close Email tab");

                vdp.ClickStickyButton(StickyButtons.Text);
                Assert.True(vdp.IsVdpTextModalOpen(), "ad14: Failed to open Request Text tab");

                vdp.ClickRequestTextModalCloseBtn();
                Assert.True(vdp.IsVdpTextModalOpen(false), "ad14: Failed to close Request Text tab");
            });
        }

        [Test, Property("TestCaseId", "8299")]
        public void VerifyReservationExpiredEmailForTBYB()
        {
            #region Variables
            string adId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "8299.adId1") : GetTestData(testDataFile, "8299.adId2");
            string expiredReservationEndPoint = GetTestData(testDataFile, "restApi.featureReserveVehicle.apiEndpoints.expiredReservationEndPoint");
            string reservationExpiredTopicId = (viewport.ToString() == "Large") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdLarge.reservationExpiredTopicId") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.emailTopicId.topicIdSmallXS.reservationExpiredTopicId");
            string emailSubjectReservationExpired = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleEn.emailSubjectReservationExpired") : GetTestData(testDataFile, "commonTestData.featureReserveVehicle.reserveVehicleFr.emailSubjectReservationExpired");
            #endregion

            stripe.CancelOrRefundChargeByAdId(adId);
            vdp.GoToVDP(baseURL, adId);
            Assert.True(vdp.IsTbybButtonEnabled(), "Try Before You Buy button is disabled. Stripe charge cancel/refund failed.");

            vdp.ClickTbybButton();
            Assert.True(tbyb.IsRedirectedToTbybFunnel(), "Try Before You Buy button is not redirecting to TBYB funnel");

            tbyb.EnterPersonalDetails(tbybPersonalDetails);
            tbyb.CheckTermsConditions();
            tbyb.CheckCommunications();
            Assert.True(tbyb.IsNextDeliveryDetailsBtnEnabled(), "Next: Delivery Details button is not enabled after filling up personal details");

            tbyb.ClickNextDeliveryDetailsBtn();
            Assert.True(tbyb.IsRedirectedToDeliveryDetails(), "Clicking on Next: Delivery Details button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.PersonalDetailsProgress), "Personal Details status indicator does not show complete after moving to Delivery Details"); }

            tbyb.SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType.Pickup);
            Assert.True(tbyb.IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType.Pickup), "Pickup tab click failed");

            string dealerPickupAddress = tbyb.GetAddressOnPickup();
            DeliveryPickupDayDate deliveryPickupDayDate = tbyb.SelectFirstAvailableDate();
            string deliveryPickupTime = tbyb.SelectFirstAvailableTime();
            Assert.True(tbyb.IsNextDepositBtnEnabled(), "Next: Deposit button is not enabled after filling up delivery details");

            tbyb.ClickNextDepositBtn();
            Assert.True(tbyb.IsRedirectedToDeposit(), "Clicking on Next: Deposit button failed");
            if (viewport.ToString() == "Large") { Assert.True(tbyb.IsProgressComplete(ProgressBarTbyb.DeliveryDetailsProgress), "Delivery Details and Personal Details status indicator does not show complete after moving to Deposit"); }

            tbyb.EnterDepositDetails(tbybDeposit);
            Assert.True(tbyb.IsPlaceDepositBtnEnabled(), "Place Deposit button is not enabled after filling up deposit details");

            tbyb.ClickPlaceDepositBtn();
            Assert.True(tbyb.IsRedirectedToCongratulations(), "Place deposit failed");

            Assert.True(tbyb.GetAddressOnCongratulations().Contains(dealerPickupAddress), "Address on Congratulations page does not match with dealer pickup address");

            DeliveryPickupMonthYear deliveryPickupMonthYear = tbyb.GetDeliveryPickupMonthYear(deliveryPickupDayDate.Day, deliveryPickupDayDate.Date);
            string deliveryPickupDay = (language.ToString() == "EN") ? DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US")) : DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("en-US"));  //Bug: French Congratulations page shows English Day https://trader.atlassian.net/browse/DR-704  //DateTime.Parse(deliveryPickupMonthYear.Month + "/" + deliveryPickupDayDate.Date + "/" + deliveryPickupMonthYear.Year, CultureInfo.InvariantCulture).ToString("dddd", new CultureInfo("fr-FR"))
            Assert.AreEqual(tbyb.GetDateTimeOnCongratulations(), (language.ToString() == "EN") ? deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime : deliveryPickupDay + " " + deliveryPickupDayDate.Date + ", " + deliveryPickupMonthYear.Year + " at " + deliveryPickupTime, "Date & Time on Congratulations page does not match with consumer selected Date & Time for pickup");

            var stripeCharge = stripe.RetrieveChargeByAdId(adId);
            Assert.IsNotNull(stripeCharge, $"Stripe charge could not be retrieved for ad id {adId}");  //Verify payment recieved
            Assert.AreEqual(stripe.stripe_AccountId, stripeCharge.Metadata["Dealer Id"]);  //Verify payment for correct stripe account

            QatdratStorage qatdratStorage = new QatdratStorage(driver, GetTestData(testDataFile, "connectionStrings.azureStorageQatdrat"), "ReservationStatus");
            DateTime reservationDateTime = qatdratStorage.GetReservationDateByChargeId(stripeCharge.Id);
            Assert.False(reservationDateTime.Equals(default), "ReservationStatus Azure storage table query execution timed out");
            qatdratStorage.UpdateExpirationDateByChargeId(stripeCharge.Id, reservationDateTime.AddDays(-1));

            var apiResponse = restApi.ExpiredReservationsApi(expiredReservationEndPoint);
            Assert.True(apiResponse != null, "Expired Reservation API response status not OK");

            //Assert.True(statisticDatabase.GetEmailTopicId(tbybPersonalDetails.EmailAddress, emailSubjectReservationExpired, EmailCreatedUtc).Contains(reservationExpiredTopicId));
        }
    }
}
