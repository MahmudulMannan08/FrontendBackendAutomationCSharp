using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;

namespace MarketPlaceWeb.Test.ClearSearchTests
{
    [TestFixture]
    class DisplayTests : Page
    {
        SRPMain srp;        
        string facetValue;
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
            facetValue = (language.ToString() == "EN") ? "Any" : "Tout";
            facetValue = (viewport != Viewport.XS) ? facetValue : string.Empty;
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

        [Test, Property("TestCaseId", "8467")]
        public void VerifySRPDisplayCountForPV()  // Failing due to known issue - If we set display 100 , SRP display lesser than 100 orgniac listings (display 98 ,99 ) while filter even have more than 1500 listing count this is happening changing to 25 , 50 as well 
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var displayCount = srp.GetDisplay();
            Assert.AreEqual("15", displayCount, "The default display count is not 15");

            srp.SelectDisplay(SRPLocators.Display._25);            
            Assert.AreEqual("25", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 25");

            srp.SelectDisplay(SRPLocators.Display._50);            
            Assert.AreEqual("50", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 50");

            srp.SelectDisplay(SRPLocators.Display._100);           
            Assert.AreEqual("100", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 100");

            srp.SelectDisplay(SRPLocators.Display._15);            
            Assert.AreEqual("15", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 15");
        }

        [Test, Property("TestCaseId", "8468")]
        public void VerifySRPDisplayCountForCnR()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var displayCount = srp.GetDisplay();
            Assert.AreEqual("15", displayCount, "The default display count is not 15");

            srp.SelectDisplay(SRPLocators.Display._25);            
            Assert.AreEqual("25", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 25");

            srp.SelectDisplay(SRPLocators.Display._50);            
            Assert.AreEqual("50", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 50");

            srp.SelectDisplay(SRPLocators.Display._100);            
            Assert.AreEqual("100", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 100");

            srp.SelectDisplay(SRPLocators.Display._15);             
            Assert.AreEqual("15", srp.GetOrganicListingsDisplayCount().ToString(), "The organic listings display count mismatch for 15");
        }

    }
}