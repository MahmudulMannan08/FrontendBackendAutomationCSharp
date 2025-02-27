using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.VDP;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.BuyOnline;
using MarketPlaceWeb.Locators;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Test.BuyersHubTests
{
    [TestFixture]
    public class BuyersHubTests : Page
    {
        SharedMain shared;
        BuyOnlineMain buyOnline;
        VDPMain vdp;
        SRPMain srp;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;

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
            shared = new SharedMain(driver, viewport, language);
            buyOnline = new BuyOnlineMain(driver, viewport, language);
            vdp = new VDPMain(driver, viewport, language);
            srp = new SRPMain(driver, viewport, language);
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

        [Test, Property("TestCaseId", "7122")]
        public void VerifyLearnMoreVdpFlyout()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "7122.adId");
            string buyersHubTitle1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "7122.buyersHubTitleEn") : GetTestData(testDataFile, "7122.buyersHubTitleFr");
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            #endregion
            vdp.GoToVDP(baseURL, adId);
            vdp.ClickOnLearnMoreBtnOnVdpFlyout();
            vdp.ClickOnLearnMoreBtnOnVdpFlyoutModal();
            SwitchTabOrWindow(() => driver.Url.Contains(buyOnlineUrl));
            Assert.AreEqual(buyersHubTitle1, vdp.GetBuyersHubTitle(), "The title in the Buyers Hub page is different.");
        }

        [Test, Property("TestCaseId", "7120")]
        public void VerifyLearnMoreVdpFlyoutViaMoneyBackGuaranteeText()
        {
            #region Variables
            string adId = GetTestData(testDataFile, "7120.adId");
            string flyoutTitle1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "7120.flyoutTitleEn") : GetTestData(testDataFile, "7120.flyoutTitleFr");
            #endregion
            vdp.GoToVDP(baseURL, adId);
            vdp.ClickTenDayMoneyBackGuaranteeText();
            Assert.AreEqual(flyoutTitle1, vdp.GetFlyoutTitle(), "The title in the Buy Online flyout modal is different.");
        }

        [Test, Property("TestCaseId", "6660")]
        public void VerifyHeaderFooterOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            Assert.True(shared.IsLogoDisplayed(), "Logo is not displayed on Buy Online page");
            Assert.True(shared.AreAllCategoriesDisplayed(), "All categories in header are not available on Buy Online page");
            Assert.True(shared.AreAllMenusDisplayed(), "All menus in header are not available on Buy Online page");

            Assert.True(shared.IsFooterDisplayed(), "All links/buttons in footer are not available on Buy Online page");
        }

        [Test, Property("TestCaseId", "6661")]
        public void VerifyBuyOnlinePage()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string buyYourNextCarTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.buyYourNextCarTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.buyYourNextCarTitle");
            string getNotifiedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.getNotifiedTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.getNotifiedTitle");
            string justATapAwayTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.justATapAwayTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.justATapAwayTitle");
            string subTitle1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.subTitle1") : GetTestData(testDataFile, "6661.buyOnlineFr.subTitle1");
            string subTitle2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.subTitle2") : GetTestData(testDataFile, "6661.buyOnlineFr.subTitle2");
            string subTitle3 = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.subTitle3") : GetTestData(testDataFile, "6661.buyOnlineFr.subTitle3");
            string subTitle4 = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.subTitle4") : GetTestData(testDataFile, "6661.buyOnlineFr.subTitle4");
            string subTitle5 = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.subTitle5") : GetTestData(testDataFile, "6661.buyOnlineFr.subTitle5");
            string showMeAllVehiclesTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.showMeAllVehiclesTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.showMeAllVehiclesTitle");
            string onlineSellerPlusTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.onlineSellerPlusTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.onlineSellerPlusTitle");
            string bestCarDealsTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.bestCarDealsTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.bestCarDealsTitle");
            string priceGuidanceSubtitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.priceGuidanceSubtitle") : GetTestData(testDataFile, "6661.buyOnlineFr.priceGuidanceSubtitle");
            string liveChatCarouselTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.liveChatCarouselTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.liveChatCarouselTitle");
            string homeTestDriveCarouselTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.homeTestDriveCarouselTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.homeTestDriveCarouselTitle");
            string virtualAppraisalCarouselTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.virtualAppraisalCarouselTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.virtualAppraisalCarouselTitle");
            string onlineReservationCarouselTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.onlineReservationCarouselTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.onlineReservationCarouselTitle");
            string localDeliveryCarouselTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.localDeliveryCarouselTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.localDeliveryCarouselTitle");
            string icoTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6661.buyOnlineEn.icoTitle") : GetTestData(testDataFile, "6661.buyOnlineFr.icoTitle");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            Assert.AreEqual(buyOnline.GetBuyYourNextCarTitle(), buyYourNextCarTitle, "Buy Your Next Car Online section title is not as expected");
            Assert.True(buyOnline.IsSearchAllVehiclesButtonAvailable(), "Buy Your Next Car Online section button Search All Vehicles is missing");

            Assert.AreEqual(buyOnline.GetGetNotifiedTitle(), getNotifiedTitle, "Get Notified section title is not as expected");
            Assert.True(buyOnline.IsGetOnTheListLinkAvailable(), "Get Notified section link Get On The List is missing");
            Assert.True(buyOnline.IsGetNotifiedButtonAvailable(), "Get Notified section button Get Notified is missing");

            Assert.AreEqual(buyOnline.GetJustATapAwayTitle(), justATapAwayTitle, "Just A Tap Away section title is not as expected");
            Assert.AreEqual(buyOnline.GetBuyingProcessSubtitle(), subTitle1, "Buying Process sub title is not as expected");
            Assert.AreEqual(buyOnline.GetCustomizePurchaseSubtitle(), subTitle2, "Customize Purchase sub title is not as expected");
            Assert.AreEqual(buyOnline.GetApplyCreditSubtitle(), subTitle3, "Apply Credit sub title is not as expected");
            Assert.AreEqual(buyOnline.GetReserveVehicleSubtitle(), subTitle4, "Reserve Vehicle sub title is not as expected");
            Assert.AreEqual(buyOnline.GetTradeInVehicleSubtitle(), subTitle5, "Trade-In Vehicle sub title is not as expected");
            Assert.True(buyOnline.IsMoreAboutTradeInLinkAvailable(), "More About Trade-In link is missing");

            Assert.AreEqual(buyOnline.GetShowMeAllVehiclesTitle(), showMeAllVehiclesTitle, "Show Me All Vehicles section title is not as expected");
            Assert.True(buyOnline.IsMakeDropdownAvailable(), "Make dropdown is not displayed");
            Assert.True(buyOnline.IsLocationTextboxAvailable(), "Location textbox is not displayed");
            Assert.True(buyOnline.IsShowMeVehiclesButtonAvailable(), "Show me vehicles button is not displayed");
            Assert.True(buyOnline.IsTrendingDealsAvailable(), "Trending Deals section is not available");

            Assert.AreEqual(buyOnline.GetOnlineSellerPlusTitle(), onlineSellerPlusTitle, "OnlineSellerPlus section title is not as expected");
            Assert.True(buyOnline.IsFindOutMoreLinkAvailable(), "Find Out More link is not displayed");

            Assert.AreEqual(buyOnline.GetBestCarDealsTitle(), bestCarDealsTitle, "Best Car Deals section title is not as expected");
            Assert.AreEqual(buyOnline.GetPriceGuidanceSubtitle(), priceGuidanceSubtitle, "Price Guidance subtitle is not as expected");
            Assert.True(buyOnline.IsGreatPriceBadgeAvailable(), "Great Price badge section is not displayed");
            Assert.True(buyOnline.IsGoodPriceBadgeAvailable(), "Good Price badge section is not displayed");
            Assert.True(buyOnline.IsFairPriceBadgeAvailable(), "Fair Price badge section is not displayed");
            Assert.True(buyOnline.IsAbovePriceBadgeAvailable(), "Above Average Price badge section is not displayed");

            Assert.AreEqual(buyOnline.GetLiveChatCarouselTitle(), liveChatCarouselTitle.ToUpper(), "Live Chat carousel title is not as expected");  //On iOS devices Element Text returns Clavardage (chat) whereas Desktop and Android returns Clavardage (Chat)
            Assert.AreEqual(buyOnline.GetHomeTestDriveCarouselTitle(), homeTestDriveCarouselTitle.ToUpper(), "Home Test Drive carousel title is not as expected");
            Assert.AreEqual(buyOnline.GetVirtualAppraisalCarouselTitle(), virtualAppraisalCarouselTitle.ToUpper(), "Virtual Appraisal carousel title is not as expected");
            Assert.AreEqual(buyOnline.GetOnlineReservationCarouselTitle(), onlineReservationCarouselTitle.ToUpper(), "Online Reservation carousel title is not as expected");
            Assert.AreEqual(buyOnline.GetLocalDeliveryCarouselTitle(), localDeliveryCarouselTitle.ToUpper(), "Local Delivery carousel title is not as expected");

            Assert.AreEqual(buyOnline.GetICOTitle(), icoTitle, "ICO section title is not as expected");
            Assert.True(buyOnline.IsMoreDetailsButtonAvailable(), "ICO section button More Details is missing");

            Assert.True(buyOnline.IsFaqSectionAvailable(), "Faq section is not available");
        }

        [Test, Property("TestCaseId", "6667")]
        public void VerifySearchByMakeOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string make = GetTestData(testDataFile, "6667.make");
            string model = GetTestData(testDataFile, "6667.model");
            string postalCode = GetTestData(testDataFile, "commonTestData.featureBuyOnline.postalCode");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            buyOnline.ClickSearchAllVehiclesBtn();
            Assert.True(buyOnline.IsShowMeVehiclesButtonAvailable(), "Show me vehicles button is not displayed");

            buyOnline.SelectMakeOnBuyOnline(make);
            buyOnline.SelectModelOnBuyOnline(model);
            buyOnline.EnterPostalCodeOnBuyOnline(postalCode);
            int showMeVehiclesCount = buyOnline.GetShowMeVehiclesCount();
            buyOnline.ClickShowMeVehiclesBtn();

            Assert.AreEqual(srp.GetTotalNumberOfFound(), showMeVehiclesCount, "Show me vehicles count and total number of found on SRP does not match");
            string srpTitle = srp.GetTitleText();
            Assert.True(srpTitle.Contains(make), "Make is not available on SRP title");
            Assert.True(srpTitle.Contains(model), "Model is not available on SRP title");
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild), make);
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild), model);
            Assert.IsTrue(srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.BuyOnline), "Buy Online option is not checked in Buying Options facet");
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.CityPostalCodeChild), postalCode);
        }

        [Test, Property("TestCaseId", "6668")]
        public void VerifyDefaultSearchOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string postalCode = GetTestData(testDataFile, "commonTestData.featureBuyOnline.postalCode");
            string defaultSRPTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "6668.buyOnlineEn.defaultSRPTitle") : GetTestData(testDataFile, "6668.buyOnlineFr.defaultSRPTitle");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            buyOnline.ClickSearchAllVehiclesBtn();
            buyOnline.ClickShowMeVehiclesBtn(true);
            Assert.True(buyOnline.IsPostalCodeWarningMsgDisplayed(), "Postal Code warning message is not displayed");

            buyOnline.EnterPostalCodeOnBuyOnline(postalCode);
            int showMeVehiclesCount = buyOnline.GetShowMeVehiclesCount();
            buyOnline.ClickShowMeVehiclesBtn();

            Assert.AreEqual(srp.GetTotalNumberOfFound(), showMeVehiclesCount, "Show me vehicles count and total number of found on SRP does not match");
            Assert.True(srp.GetTitleText().Contains(defaultSRPTitle), "SRP title does not match");
            Assert.IsTrue(srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.BuyOnline), "Buy Online option is not checked in Buying Options facet");
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.CityPostalCodeChild), postalCode);
        }

        [Test, Property("TestCaseId", "6669")]
        public void VerifyTrendingDealsSearchOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string postalCode = GetTestData(testDataFile, "commonTestData.featureBuyOnline.postalCode");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            buyOnline.ClickSearchAllVehiclesBtn();
            Assert.True(buyOnline.IsTrendingDealsAvailable(), "Trending Deals section is not available");

            buyOnline.EnterPostalCodeOnBuyOnline(postalCode);
            string make = buyOnline.GetMakeModelOnTrendingDeals()[0];
            string model = buyOnline.GetMakeModelOnTrendingDeals()[1];
            buyOnline.ClickTrendingDealsBadge1();

            string srpTitle = srp.GetTitleText();
            Assert.True(srpTitle.Contains(make), "Make is not available on SRP title");
            Assert.True(srpTitle.Contains(model), "Model is not available on SRP title");
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild), make);
            Assert.AreEqual(srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild), model);
            Assert.IsTrue(srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.BuyOnline), "Buy Online option is not checked in Buying Options facet");
        }

        [Test, Property("TestCaseId", "6670")]
        public void VerifyIcoOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string icoUrl = GetTestData(testDataFile, "6670.icoUrl");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            Assert.True(buyOnline.IsMoreDetailsButtonAvailable(), "ICO section button More Details is missing");

            buyOnline.ClickMoreDetailsBtnOnBuyOnline();
            Assert.True(IsInCurrentUrl(icoUrl), "More Details button does not redirect to ICO page");
        }

        [Test, Property("TestCaseId", "7764")]
        public void VerifyPrivacyTermsOnGetNotified()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string privacyUrl = GetTestData(testDataFile, "commonTestData.featureBuyOnline.privacyUrl");
            string termsUrl = GetTestData(testDataFile, "commonTestData.featureBuyOnline.termsUrl");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");
            Assert.True(buyOnline.IsGetOnTheListLinkAvailable(), "Get Notified section link Get On The List is missing");
            Assert.True(buyOnline.IsGetNotifiedButtonAvailable(), "Get Notified section button Get Notified is missing");

            buyOnline.ClickGetNotifiedBtn();
            Assert.True(buyOnline.IsPrivacyLinkOnGetNotifiedDisplayed(), "Privacy policy link is not displayed on Get Notified modal");
            Assert.True(buyOnline.IsTermsLinkOnGetNotifiedDisplayed(), "Terms of Use link is not displayed on Get Notified modal");

            buyOnline.ClickPrivacyLinkOnGetNotified();
            Assert.True(buyOnline.IsRedirectToPrivacyFromGetNotified(privacyUrl), "Privacy policy link on Get Notified modal does not redirect to Privacy page or URL does not match");  //Click event is not registering/Switching tab does not work from modal overlay for iOS devices
            Assert.True(buyOnline.IsPrivacySectionDisplayed(), "User is not redirected to Privacy section of the Privacy Statement page");

            SwitchToBaseWindow();
            buyOnline.ClickTermsLinkOnGetNotified();
            Assert.True(buyOnline.IsRedirectToTermsFromGetNotified(termsUrl), "Privacy policy link on Get Notified modal does not redirect to Privacy page or URL does not match");
            Assert.True(buyOnline.IsTermsSectionDisplayed(), "User is not redirected to Terms section of the Privacy Statement page");
        }

        [Test, Property("TestCaseId", "7764")]
        public void VerifyGetNotifiedOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string email = "BuyOnline_" + Extensions.GenerateRandomString(6) + "@mpwebtest.com";
            string postalCode = GetTestData(testDataFile, "commonTestData.featureBuyOnline.postalCode");
            string successMsg = (language.ToString() == "EN") ? GetTestData(testDataFile, "7764.buyOnlineEn.successMsg") : GetTestData(testDataFile, "7764.buyOnlineFr.successMsg");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");
            Assert.True(buyOnline.IsGetOnTheListLinkAvailable(), "Get Notified section link Get On The List is missing");
            Assert.True(buyOnline.IsGetNotifiedButtonAvailable(), "Get Notified section button Get Notified is missing");

            buyOnline.ClickGetNotifiedBtn();
            buyOnline.EnterEmailOnGetNotified(email);
            buyOnline.EnterPostalCodeOnGetNotified(postalCode);
            buyOnline.CheckTermsOnGetNotified();
            Assert.True(buyOnline.IsGetNotifiedBtnEnabled(), "Get Notified button on get notified modal does not enable after entering data on modal");

            buyOnline.ClickGetNotifiedBtnOnModal();
            Assert.AreEqual(buyOnline.GetSuccessMessageDisplayedOnModal(), successMsg, "Success message is not displayed on Get Notified modal or does not match");
            Assert.True(buyOnline.IsCloseBtnDisplayedOnModal(), "Close button is not displayed on Get Notified modal");
            buyOnline.ClickCloseBtnOnModal();

            buyOnline.ClickGetOnTheListLink();
            buyOnline.EnterEmailOnGetNotified(email);
            buyOnline.EnterPostalCodeOnGetNotified(postalCode);
            buyOnline.CheckTermsOnGetNotified();
            Assert.True(buyOnline.IsGetNotifiedBtnEnabled(), "Get Notified button on get notified modal does not enable after entering data on modal");

            buyOnline.ClickGetNotifiedBtnOnModal();
            Assert.AreEqual(buyOnline.GetSuccessMessageDisplayedOnModal(), successMsg, "Success message is not displayed on Get Notified modal or does not match");
            Assert.True(buyOnline.IsCloseBtnDisplayedOnModal(), "Close button is not displayed on Get Notified modal");
            buyOnline.ClickCloseBtnOnModal();
        }

        [Test, Property("TestCaseId", "7652")]
        public void VerifyFAQSectionOnBuyOnline()
        {
            #region Variables
            string buyOnlineUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineEn.buyOnlineUrl") : GetTestData(testDataFile, "commonTestData.featureBuyOnline.buyOnlineFr.buyOnlineUrl");
            string faqTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "7652.buyOnlineEn.faqTitle") : GetTestData(testDataFile, "7652.buyOnlineFr.faqTitle");
            #endregion
            url = new Uri(baseURL);
            Open();

            shared.GoToBuyOnlinePage();
            Assert.True(buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl), "Navigation to Buy Online page failed");

            Assert.True(buyOnline.IsFaqSectionAvailable(), "Faq section is not available");
            Assert.AreEqual(buyOnline.GetFaqTitle(), faqTitle, "FAQ section title not available or does not match");
            Assert.True(buyOnline.IsFaqAccordionDisplayed(), "FAQ accordion is not displayed");

            buyOnline.ClickFaqQuestion1();
            Assert.True(buyOnline.IsQuestion1AnswerDisplayed(), "FAQ first questions answer is not expanded");

            buyOnline.ClickFaqQuestion1(false);
            Assert.False(buyOnline.IsQuestion1AnswerDisplayed(), "FAQ first questions answer is not collapsed");
        }
    }
}