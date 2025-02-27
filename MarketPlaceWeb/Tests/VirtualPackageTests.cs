using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.SRP;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Tests
{
    class VirtualPackageTests : Page
    {
        SRPMain srp;
        VDPMain vdp;
        HPMain homePage;
        string baseURL;
        SharedMain shared;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        bool isIosDevice;
        bool isHomeDeliveryToggleOn;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            testDataFile = GetTestDataFile(baseURL);
            hpVariant = (azureConfig.isAzureEnabled) ? azureConfig.hpVariant : (viewport.ToString() == "XS") ?
               GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlXS") :
               GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlDT");
            isHomeDeliveryToggleOn = (azureConfig.isAzureEnabled) ? azureConfig.isHomeDeliveryToggleEnabled : Convert.ToBoolean(GetTestData(testDataFile, "FeatureToggles.homeDeliveryToggle"));
            isIosDevice = (azureConfig.isAzureEnabled) ? (azureConfig.config == "ipad-safari-small" || azureConfig.config == "ios-safari-xs") : (localConfig.config == "ipad-safari-small" || localConfig.config == "ios-safari-xs");
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            srp = new SRPMain(driver, viewport, language);
            vdp = new VDPMain(driver, viewport, language);
            shared = new SharedMain(driver, viewport, language);
            homePage = new HPMain(driver, viewport, language);
        }

        [TearDown]
        public void CleanUp()
        {
            ResultState resultState = TestContext.CurrentContext.Result.Outcome;
            if (resultState == ResultState.Error || resultState == ResultState.Failure)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
            }
            driver.Quit();
        }
        [Test, Property("TestCaseId", "11070")]
        public void VerifyProximityOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var virtualAdId = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.virtualAdId");
            var hybridAdId = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.hybridAdId");
            var traditionalAdId = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.traditionalAdId");
            if (viewport == Viewport.XS)
            {
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.virtualProximityTextXS"), srp.GetListingProximity(virtualAdId), "Proximity text doesn't match for Virtual dealer ad " + virtualAdId);
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.hybridProximityTextXS"), srp.GetListingProximity(hybridAdId), "Proximity text doesn't match for Hybrid dealer ad " + hybridAdId);
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.traditionalProximityTextXS"), srp.GetListingProximity(traditionalAdId), "Proximity text doesn't match for traditional dealer ad " + traditionalAdId);
            }
            else
            {
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.virtualProximityText"), srp.GetListingProximity(virtualAdId), "Proximity text doesn't match for Virtual dealer ad " + virtualAdId);
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.hybridProximityText"), srp.GetListingProximity(hybridAdId), "Proximity text doesn't match for Hybrid dealer ad " + hybridAdId);
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.traditionalProximityText"), srp.GetListingProximity(traditionalAdId), "Proximity text doesn't match for traditional dealer ad " + traditionalAdId);
            }
        }
        [Test, Property("TestCaseId", "10404")]
        public void VerifyMapOnVDPForVirtualDealer()
        {
            if (viewport != Viewport.XS)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var virtualAdId = GetTestData(testDataFile, $"{testcaseId}.virtualAdUrl");
                url = new Uri(baseURL + virtualAdId);
                Open();
                Assert.IsFalse(vdp.IsViewMapDisplayed(), $"View map button should not be diplayed for virtual dealer ad {virtualAdId}");
            }
        }
        [Test, Property("TestCaseId", "10405")]
        public void VerifyMapOnVDPForHybridDealer()
        {
            if (viewport != Viewport.XS)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var hybridAdId = GetTestData(testDataFile, $"{testcaseId}.hybridAdUrl");
                url = new Uri(baseURL + hybridAdId);
                Open();
                Assert.IsTrue(vdp.IsViewMapDisplayed(), $"View map button doesn't diplay for hybrid dealer ad {hybridAdId}");
                vdp.ClickOnViewMapButton();
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.hybridAdAddress"), vdp.GetDealerAddressOnViewMap(), $"Address doen't match for hybrid dealer ad {hybridAdId}. It should have parent dealer address");
            }
        }
        [Test, Property("TestCaseId", "10406")]
        public void VerifyMapOnVDPForTraditionalDealer()
        {
            if (viewport != Viewport.XS)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var traditionalAdId = GetTestData(testDataFile, $"{testcaseId}.traditionalAdUrl");
                url = new Uri(baseURL + traditionalAdId);
                Open();
                Assert.IsTrue(vdp.IsViewMapDisplayed(), $"View map button doesn't diplay for traditional dealer ad {traditionalAdId}");
                vdp.ClickOnViewMapButton();
                Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.traditionalAdAddress"), vdp.GetDealerAddressOnViewMap(), $"Address doen't match for traditional dealer ad {traditionalAdId}");
            }

        }

        [Test, Property("TestCaseId", "12033")]
        public void VerifyHomeDeliveryFlowWithLocation()
        {
            if (!isHomeDeliveryToggleOn)
            {
                Assert.Ignore("Skipping test because Home delivery toggle is off");
            }
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            string homeDeliveryURL = GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.homeDeliveryURL");
            string location = GetTestData(testDataFile, $"{testcaseId}.location");
            string minYear = GetTestData(testDataFile, $"{testcaseId}.minYear");
            string homepageVariant = (viewport.ToString() == "XS") ?
                GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlXS") :
                GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlDT");
            var proximity = new Proximity
            {
                virtualProximityText = (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.virtualProximityTextXS") : GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.virtualProximityText"),
                hybridProximityText = (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.hybridProximityTextXS") : GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.hybridProximityText")
            };
            url = new Uri(baseURL + homepageVariant);
            Open();
            homePage.EnterLocation(location,HPAbstract.Searchbox.New);
            homePage.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            shared.GoToBuyOnlinePage(isHomeDeliveryToggleOn);
            srp.WaitForSRPPageLoad();
            Assert.True(IsInCurrentUrl(homeDeliveryURL), "Buy Online link does not redirect to Home Delivery page");
            srp.SelectYearFacet(SRPLocators.Year.MinYear, minYear, "");
            Assert.True(srp.IsHomeDeliveryOn(), "Home Delivery toggle is not enabled on SRP");
            Assert.True(srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.HomeDelivery), "Home Delivery option is not selected from facet Buying Options.");
            srp.ClickOnApplyOnFacet(SRPLocators.Facets.AtHomeServices);
            Assert.True(srp.IsAllListingWithoutTraditionDealer(proximity), "Organic listing should not be displayed traditional dealer when Home Delivery is enabled or Proximity is not displayed for one or more dealer");
        }
        [Test, Property("TestCaseId", "12035")]
        public void VerifyHomeDeliveryFlowWithoutLocation()
        {
            if (!isHomeDeliveryToggleOn)
            {
                Assert.Ignore("Skipping test because Home delivery toggle is off");
            }
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var tooltipMessage = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.homeDeliveryTooptipMsg");
            var infoIconTooltipMessage = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.homeDeliveryInfoIconTooptipMsg");
            string homeDeliveryURL = GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.homeDeliveryURL");
            string location = GetTestData(testDataFile, $"{testcaseId}.location");
            bool isHomeDeliveryEnabled = true;
            string homepageVariant = (viewport.ToString() == "XS") ?
               GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlXS") :
               GetTestData(testDataFile, "optimizelyCookies.homePageSearch.controlDT");
            var proximity = new Proximity
            {
                virtualProximityText = (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.virtualProximityTextXS") : GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.virtualProximityText"),
                hybridProximityText = (viewport.ToString() == "XS") ? GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.hybridProximityTextXS") : GetTestData(testDataFile, $"commonTestData.homeDelivery.{language.ToString()}.hybridProximityText")
            };
            url = new Uri(baseURL + homepageVariant);
            Open();
            shared.GoToBuyOnlinePage(isHomeDeliveryEnabled);
            srp.WaitForSRPPageLoad();
            Assert.True(IsInCurrentUrl(homeDeliveryURL), "Buy Online link does not redirect to Home Delivery page");
            Assert.False(srp.IsHomeDeliveryOn(), "Home Delivery toggle should be disabled on SRP when location is blank");
            if (!isIosDevice)
            {
                if (viewport.ToString() != "XS")
                {
                    string homeDeliveryTooltipMessage = srp.GetHomeDeliveryTooltipMessage();
                    Assert.AreEqual(tooltipMessage, homeDeliveryTooltipMessage, "Home Delivery toggle tooltip message doesn't match when location is blank ");
                }
                string homeDeliveryInfoIconTooltipMessage = srp.GetHomeDeliveryInfoIconTooltipMessage();
                Assert.AreEqual(infoIconTooltipMessage, homeDeliveryInfoIconTooltipMessage, "Home Delivery info icon tooltip message doesn't match when location is blank ");
            }
            srp.SelectLocationFacet(location);
            srp.SelectBuyingOptionsSingle(SRPLocators.BuyingOptions.HomeDelivery, true);
            Assert.True(srp.IsHomeDeliveryOn(), "Home Delivery toggle is not enabled on SRP");
            Assert.True(srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.HomeDelivery), "Home Delivery option is not selected from facet Buying Options.");
            srp.ClickOnApplyOnFacet(SRPLocators.Facets.AtHomeServices);
            Assert.True(srp.IsAllListingWithoutTraditionDealer(proximity), "Organic listing should not display traditional dealer with Home Delivery enabled");

        }

    }
}
