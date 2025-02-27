using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.SRP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    class BoostTests : Page
    {
        SRPMain srp;
        SharedMain shared;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
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
            boostVariant = (azureConfig.isAzureEnabled) ? azureConfig.boostVariant : (viewport.ToString() == "XS") ?
                GetTestData(testDataFile, "optimizelyCookies.boost.variantXS") :
                GetTestData(testDataFile, "optimizelyCookies.boost.variantDT");
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            srp = new SRPMain(driver, viewport, language);
            shared = new SharedMain(driver, viewport, language);
            testcaseId = (string)TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrlBoost");
            url = new Uri(baseURL + testURL + boostVariant);
            Open();
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

        [Test, Property("TestCaseId", "7532")]
        public void VerifyBoostListingOrder()
        {
            var isPriorityList = true;
            var priotiryListingAdSortPoints = srp.GetAdSortPoints(isPriorityList);
            var allListingAdSortPoints = srp.GetAdSortPoints();
            if (priotiryListingAdSortPoints.Count > 0)
            {
                Assert.AreEqual(true, shared.IsArraySortedReverseOrder(priotiryListingAdSortPoints[SRPAbstract.ListingsType.PL].ToArray()), "PL and XPL combined sort points are not sorted in descending order");
            }
            if (allListingAdSortPoints.Count > 0)
            {
                Assert.AreEqual(true, shared.IsArraySortedReverseOrder(allListingAdSortPoints[SRPAbstract.ListingsType.Org].ToArray()), "All listing sort points are not sorted in descending order");
            }
        }

        [Test, Property("TestCaseId", "7533")]
        public void VerifyBoostDefaultSortType()
        {
            var sortType = srp.GetSortType();
            string sortTypeTestData = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.boostDefaultSortType");
            Assert.AreEqual(sortTypeTestData, sortType, "Sort type is not default for Boost");
        }
    }
}
