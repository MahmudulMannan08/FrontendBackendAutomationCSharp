using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using static MarketPlaceWeb.Pages.SRP.SRPAbstract;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class SpinTests : Page
    {
        SRPMain srp;
        VDPMain vdp;
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
            srp = new SRPMain(driver, viewport, language);
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


        [Test, Property("TestCaseId", "10625")]
        public void VerifySpinIconOnSRP()
        {
            if (viewport == Viewport.Large)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
                url = new Uri(baseURL + testURL);
                Open();
                var topSpotListItem = srp.GetFirstMatchedListing(ListingsType.Org);
                Assert.IsNotNull(topSpotListItem, "TopSpot doen't display on SRP ");
                Assert.True(srp.IsSpinIconDisplayedOnSrp(topSpotListItem), "Spin icon doesn't diplay on Topspot list item");
                var xplListItem = srp.GetFirstMatchedListing(ListingsType.XPL);
                Assert.IsNotNull(xplListItem, "XPL doen't display on SRP ");
                Assert.True(srp.IsSpinIconDisplayedOnSrp(xplListItem), "Spin icon doesn't diplay on XPL list item");
                var orgListItem = srp.GetFirstMatchedListing(ListingsType.Org);
                Assert.IsNotNull(orgListItem, "Org doen't display on SRP ");
                Assert.True(srp.IsSpinIconDisplayedOnSrp(orgListItem), "Spin icon doesn't diplay on Org list item");
            }
        }

        [Test, Property("TestCaseId", "10626")]
        public void VerifySpinIconOnVDP()
        {
            if (viewport == Viewport.Large)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
                url = new Uri(baseURL + testURL);
                Open();
                srp.ClickOnFirstOrganicListing();
                Assert.True(vdp.IsSpinIconDisplayedOnVdp(), "Spin icon doen't display over hero image");
                vdp.ClickSpinIconOnVdp();
                Assert.True(vdp.IsSeeInsideButtonDisplayed(), "See inside button doesn't diplay on VDP");
                string seeInsideButtonText = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.seeInsideButton");
                Assert.AreEqual(seeInsideButtonText, vdp.GetSeeInsideButtonText().Replace("\"", ""), "See Inside button text doesn't match");
                vdp.CloseSpinModal();
                Assert.True(vdp.IsSpinModalClosed(), "Unable to close spoin modal");
            }
        }
        [Test, Property("TestCaseId", "10632")]
        public void VerifySpinWidgetOnVDP()
        {
            if (viewport == Viewport.Large)
            {
                var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
                var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
                url = new Uri(baseURL + testURL);
                Open();
                srp.ClickOnFirstOrganicListing();
                Assert.True(vdp.IsSpinWidgetDisplayedOnVdp(), "Spin widget doen't display on VDP");
                string spinWidgetText = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.spinWidgetText");
                Assert.AreEqual(spinWidgetText, vdp.GetSpinWidgetTextDisplayedOnVdp(), "Spin widget text doen't match on VDP");
                vdp.ClickWidgetSpinIcon();
                vdp.CloseSpinModal();
                Assert.True(vdp.IsSpinModalClosed(), "Unable to close spoin modal");
            }

        }
    }
}
