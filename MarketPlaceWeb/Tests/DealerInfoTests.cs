using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class DealerInfoTests : Page
    {
        VDPMain vdp;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        bool isIosDevice;
        bool isDealerInfoBrandingVideoToggle;

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
            isDealerInfoBrandingVideoToggle = (azureConfig.isAzureEnabled) ? azureConfig.isDealerInfoBrandingVideoToggle : Convert.ToBoolean(GetTestData(testDataFile, "FeatureToggles.dealerInfoBrandingVideoToggle"));
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            vdp = new VDPMain(driver, viewport, language);
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

      
        [Test, Property("TestCaseId", "11376")]
        public void VerifyDealerInfoForDealershipVideoAd()
        {
            if (!isDealerInfoBrandingVideoToggle)
            {
                Assert.Ignore("Skipping test because branding video dealer info toggle is off");
            }
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var brandingVideoAd = GetTestData(testDataFile, $"{testcaseId}.brandingVideoUrl");
            url = new Uri(baseURL + brandingVideoAd);
            Open();
            Assert.IsTrue(vdp.IsDealerInfoDisplayed(), $"DealerInfo widget doesn't display on VDP for Ad {brandingVideoAd}");
            string actualDealerInfoTitle = vdp.GetDealerInfoWidgetHeader();
            Assert.AreEqual(GetTestData(testDataFile, $"commonTestData.dealerInfo.{language.ToString()}.widgetHeader"), actualDealerInfoTitle, $"DealerInfo widget title doesn't match for Ad {brandingVideoAd}");
            string actualDealerName = vdp.GetDealerInfoName();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerName"), actualDealerName, $"DealerInfo dealer name doesn't match for Ad {brandingVideoAd}");
            Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkClickable(), $"DealerInfo visit website link doen't diplay on VDP for Ad {brandingVideoAd}");
            Assert.IsTrue(vdp.IsDealerInfoViewAllInventoryButtonDisplayed(), $"DealerInfo View all inventory button doen't diplay on VDP for Ad {brandingVideoAd}");
            if (!isIosDevice)
            {
                vdp.ClickOnDealerInfoVisitWebsiteLink();
                Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkUrlMatch(GetTestData(testDataFile, $"{testcaseId}.visitWebsiteLink")), "DealerInfo visit website link doesn't match");
                
                vdp.ClickOnViewAllInventoryButton();
                Assert.IsTrue(vdp.IsDealershipPageMatch(GetTestData(testDataFile, $"{testcaseId}..{language.ToString()}.dealershipURLRoute")), $"Dealership page doen't match clicking on View all inventory button for Ad {brandingVideoAd}");
                Back();
            }
            Assert.IsTrue(vdp.IsDealerInfoShowmapButtonDisplayed(), $"DealerInfo Show map button doen't diplay on VDP for Ad {brandingVideoAd}");
            vdp.ClickOnShowmapButton();
            string actualDealerAddressOnViewMap = vdp.GetDealerAddressOnViewMap();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerAddress"), actualDealerAddressOnViewMap, $"Show map Address doen't match for ad with URL for Ad {brandingVideoAd}");
            vdp.CloseViewMap();
           
            int actualDealerUSPCount = vdp.GetDealerInfoUSPItemsCount();
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.upsItemCounts")), actualDealerUSPCount, $"DealerInfo USP item count doesn't match for Ad {brandingVideoAd}");
            Assert.IsTrue(vdp.IsDealerInfoAwardWinnerLogoDisplayed(), $"DealerInfo award winner logo doesn't display on VDP for Ad {brandingVideoAd}");
            Assert.IsTrue(vdp.IsBrandingVideoImageThumbDisplayed(), $"DealerInfo branding video image thumbnail doen't diplay on VDP for Ad {brandingVideoAd}");
            vdp.ClickOnBrandingVideo();
            Assert.IsTrue(vdp.IsBandingVideoModalDisplayed(), $"DealerInfo branding video doesn't open after clicking on thumbnail for Ad {brandingVideoAd}");
            vdp.CloseBrandingVideo();
            Assert.IsFalse(vdp.IsBandingVideoModalDisplayed(), $"DealerInfo branding video doesn't close after clicking on close button for Ad {brandingVideoAd}");
        }

        [Test, Property("TestCaseId", "11377")]
        public void VerifyDealerInfoForOTLEventAd()
        {
            if (!isDealerInfoBrandingVideoToggle)
            {
                Assert.Ignore("Skipping test because branding video dealer info toggle is off");
            }
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var otlEventAd = GetTestData(testDataFile, $"{testcaseId}.otlEventUrl");
            url = new Uri(baseURL + otlEventAd);
            Open();
            Assert.IsTrue(vdp.IsDealerInfoDisplayed(), $"DealerInfo widget doesn't display on VDP for Ad {otlEventAd}");
            string actualDealerInfoTitle = vdp.GetDealerInfoWidgetHeader();
            Assert.AreEqual(GetTestData(testDataFile, $"commonTestData.dealerInfo.{language.ToString()}.widgetHeader"), actualDealerInfoTitle, $"DealerInfo widget title doesn't match for Ad {otlEventAd}");
            string actualDealerName = vdp.GetDealerInfoName();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerName"), actualDealerName, $"DealerInfo dealer name doesn't match for Ad {otlEventAd}");
            Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkClickable(), $"DealerInfo visit website link doen't diplay on VDP for Ad {otlEventAd}");
            Assert.IsTrue(vdp.IsDealerInfoViewAllInventoryButtonDisplayed(), $"DealerInfo View all inventory button doen't diplay on VDP for Ad {otlEventAd}");

            if (!isIosDevice)
            {
                vdp.ClickOnDealerInfoVisitWebsiteLink();
                Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkUrlMatch(GetTestData(testDataFile, $"{testcaseId}.visitWebsiteLink")), "DealerInfo visit website link doesn't match");
                
                vdp.ClickOnViewAllInventoryButton();
                Assert.IsTrue(vdp.IsDealershipPageMatch(GetTestData(testDataFile, $"{testcaseId}..{language.ToString()}.dealershipURLRoute")), $"Dealership page doen't match clicking on View all inventory button for Ad {otlEventAd}");
                Back();

            }
            Assert.IsTrue(vdp.IsDealerInfoShowmapButtonDisplayed(), $"DealerInfo Show map button doen't diplay on VDP for Ad {otlEventAd}");
            vdp.ClickOnShowmapButton();
            string actualDealerAddressOnViewMap = vdp.GetDealerAddressOnViewMap();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerAddress"), actualDealerAddressOnViewMap, $"Show map Address doen't match for ad with URL for Ad {otlEventAd}");
            vdp.CloseViewMap();
           
            int actualDealerUSPCount = vdp.GetDealerInfoUSPItemsCount();
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.upsItemCounts")), actualDealerUSPCount, $"DealerInfo USP item count doesn't match for Ad {otlEventAd}");
            Assert.IsTrue(vdp.IsDealerInfoAwardWinnerLogoDisplayed(), $"DealerInfo award winner logo doesn't display on VDP for Ad {otlEventAd}");
            Assert.IsTrue(vdp.IsOTLEventImageThumbDisplayed(), $"DealerInfo OTL event image thumbnail doen't diplay on VDP for Ad {otlEventAd}");
        }

        [Test, Property("TestCaseId", "11378")]
        public void VerifyDealerInfoForDefaultAd()
        {
            if (!isDealerInfoBrandingVideoToggle)
            {
                Assert.Ignore("Skipping test because branding video dealer info toggle is off");
            }
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var defaultAd = GetTestData(testDataFile, $"{testcaseId}.defaultAd");
            url = new Uri(baseURL + defaultAd);
            Open();
            Assert.IsTrue(vdp.IsDealerInfoDisplayed(), $"DealerInfo widget doesn't display on VDP for Ad {defaultAd}");
            string actualDealerInfoTitle = vdp.GetDealerInfoWidgetHeader();
            Assert.AreEqual(GetTestData(testDataFile, $"commonTestData.dealerInfo.{language.ToString()}.widgetHeader"), actualDealerInfoTitle, $"DealerInfo widget title doesn't match for Ad {defaultAd}");
            Assert.IsTrue(vdp.IsDealerInfoLogoDisplayed(), $"DealerInfo logo doen't diplay on VDP for Ad {defaultAd}");
            string actualDealerName = vdp.GetDealerInfoName();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerName"), actualDealerName, $"DealerInfo dealer name doesn't match for Ad {defaultAd}");
            Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkClickable(), $"DealerInfo visit website link doen't diplay on VDP for Ad {defaultAd}");
            if (!isIosDevice)
            {
                vdp.ClickOnDealerInfoVisitWebsiteLink();
                Assert.IsTrue(vdp.IsDealerInfoVisitWebsiteLinkUrlMatch(GetTestData(testDataFile, $"{testcaseId}.visitWebsiteLink")), "DealerInfo visit website link doesn't match");
            }

            Assert.IsTrue(vdp.IsDealerInfoShowmapButtonDisplayed(), $"DealerInfo Show map button doen't diplay on VDP for Ad {defaultAd}");
            vdp.ClickOnShowmapButton();
            string actualDealerAddressOnViewMap = vdp.GetDealerAddressOnViewMap();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.dealerAddress"), actualDealerAddressOnViewMap, $"Show map Address doen't match for ad with URL for Ad {defaultAd}");
            vdp.CloseViewMap();
            Assert.IsFalse(vdp.IsDealerInfoViewAllInventoryButtonDisplayed(), $"DealerInfo View all inventory button should not diplay on VDP for Ad {defaultAd}");

            Assert.IsTrue(vdp.IsDealerInfoSeeReviewsLinkClickable(), $"DealerInfo see review link is not accessible on VDP for Ad {defaultAd}");
            Assert.IsTrue(vdp.IsDealerInfoGoogleReviewDisplayed(), $"DealerInfo google review link doen't diplay on VDP for Ad {defaultAd}");
            int actualDealerUSPCount = vdp.GetDealerInfoUSPItemsCount();
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.upsItemCounts")), actualDealerUSPCount, $"DealerInfo USP item count doesn't match for Ad {defaultAd}");
            Assert.IsTrue(vdp.IsDealerInfoAwardWinnerLogoDisplayed(), $"DealerInfo award winner logo doesn't display on VDP for Ad {defaultAd}");
            if (viewport == Viewport.Large)
            {
                Assert.IsTrue(vdp.IsDefaultImageThumbDisplayed(), "DealerInfo default image doen't diplay on VDP");
            }
            else
            {
                Assert.IsFalse(vdp.IsDefaultImageThumbDisplayed(), "DealerInfo default image should not diplay on VDP");
            }

        }
     
    }
}
