using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;


namespace MarketPlaceWeb.Test.GalleryTests
{
    [TestFixture]
    class GalleryTests : Page
    {
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

        [Test, Property("TestCaseId", "7600")]
        public void VerifyYoutubeVideoOnVDP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.YoutubeADsUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.ClickVideoPlayIconInGallery();
            Assert.True(vdp.VerifyYoutubeVideoIsPlaying().Contains("playing-mode"), "The Youtube Video opens in playing mode");
            Assert.True(vdp.VerifyCorrectYoutubeIDLinkIsDisplayedOnVDP().Contains(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.YoutubeID")), "The correct Youtube ID/url displays on the VDP");
        }

        [Test, Property("TestCaseId", "7615")]
        public void VerifyYoutubeVideoDisplaysWithFullUrl()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.YoutubeADsUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.ClickVideoPlayIconInGallery();
            Assert.True(vdp.VerifyYoutubeVideoIsPlaying().Contains("playing-mode"), "The Youtube Video opens in playing mode");
            Assert.True(vdp.VerifyCorrectYoutubeIDLinkIsDisplayedOnVDP().Contains(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.YoutubeID")), "The correct Youtube ID/url displays on the VDP");
        }
    }
}