using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.Shared;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using static MarketPlaceWeb.Pages.SRP.SRPAbstract;

namespace MarketPlaceWeb.Test
{
    class SRPListingTests : Page
    {
        SRPMain srp;
        SharedMain shared;
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
            shared = new SharedMain(driver, viewport, language);
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

        #region PriorityListings
        [Test, Property("TestCaseId", "6930")]
        public void VerifyTSImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing");
            url = new Uri(baseURL + testURL);
            Open();
            var listItem = srp.GetFirstMatchedListing(ListingsType.TS);
            Assert.IsNotNull(listItem, "TopSpot doen't display on SRP ");
            var mainImage = srp.GetListingMainImage(listItem);
            if (viewport == Viewport.XS)
            {
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidthXS")), mainImage.Width, "TopSpot Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeightXS")), mainImage.Height, "TopSpot Main Image height doesn't match for ad: " + mainImage.URL);
            }
            else
            {
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "TopSpot Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "TopSpot Main Image height doesn't match for ad: " + mainImage.URL);
                var stripeImage = srp.GetListingStripeImage(listItem);
                Assert.IsNotNull(stripeImage, "Strip Image gallery is missing for TopSpot listing");
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageWidth")), stripeImage.Width, "TopSpot Stripe Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageHeight")), stripeImage.Height, "TopSpot Stripe Image height doesn't match for ad: " + mainImage.URL);
            }

        }
        [Test, Property("TestCaseId", "8554")]
        public void VerifyXPLImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing");
            url = new Uri(baseURL + testURL);
            Open();
            var listItem = srp.GetFirstMatchedListing(ListingsType.XPL);
            Assert.IsNotNull(listItem, "XPL doen't display on SRP ");
            var mainImage = srp.GetListingMainImage(listItem);
            if (viewport == Viewport.XS)
            {
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidthXS")), mainImage.Width, "XPL Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeightXS")), mainImage.Height, "XPL Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }
            else
            {
                var stripeImage = srp.GetListingStripeImage(listItem);
                Assert.IsNull(stripeImage, "Stripe Image gallery should not be displayed for XPL listing");
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "XPL Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "XPL Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }

        }
        [Test, Property("TestCaseId", "8556")]
        public void VerifyPLImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing");
            url = new Uri(baseURL + testURL);
            Open();
            var listItem = srp.GetFirstMatchedListing(ListingsType.PL);
            Assert.IsNotNull(listItem, "PL doen't display on SRP ");
            var mainImage = srp.GetListingMainImage(listItem);
            if (viewport == Viewport.XS)
            {
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidthXS")), mainImage.Width, "Priority Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeightXS")), mainImage.Height, "Priority Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }
            else
            {
                var stripeImage = srp.GetListingStripeImage(listItem);
                Assert.IsNull(stripeImage, "Stripe Image gallery should not be displayed for Priority listing");
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "Priority Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "Priority Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }

        }
        #endregion

        #region AllListings
        [Test, Property("TestCaseId", "8555")]
        public void VerifyPPLImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing"); 
            url = new Uri(baseURL + testURL);
            Open();
            if (viewport == Viewport.XS)
            {
                var listItem = srp.GetFirstMatchedListing(ListingsType.TA);
                Assert.IsNotNull(listItem, "Top Ad doen't display on SRP ");
                var mainImage = srp.GetListingMainImage(listItem);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidthXS")), mainImage.Width, "Top Ad Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeightXS")), mainImage.Height, "Top Ad Main Image height doesn't match for ad: " + mainImage.URL);
            }
            else
            {
                var listItem = srp.GetFirstMatchedListing(ListingsType.PPL);
                Assert.IsNotNull(listItem, "PPL doen't display on SRP ");
                var mainImage = srp.GetListingMainImage(listItem);
                var stripeImage = srp.GetListingStripeImage(listItem);
                Assert.IsNotNull(stripeImage, "Strip Image gallery is missing for PPL listing");
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "Provincial Priority Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "Provincial Priority Listing Main Image height doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageWidth")), stripeImage.Width, "Provincial Priority Listing Stripe Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageHeight")), stripeImage.Height, "Provincial Priority Listing Stripe Image height doesn't match for ad: " + mainImage.URL);
            }

        }
        [Test, Property("TestCaseId", "8558")]
        public void VerifyHLImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing"); 
            url = new Uri(baseURL + testURL);
            Open();
            var listItem = srp.GetFirstMatchedListing(ListingsType.HL);
            Assert.IsNotNull(listItem, "HL doen't display on SRP ");
            var mainImage = srp.GetListingMainImage(listItem);
            var stripeImage = srp.GetListingStripeImage(listItem);
            Assert.IsNotNull(stripeImage, "Strip Image gallery is missing for Highlight listing");
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "Highlight Listing Main Image width doesn't match for ad: " + mainImage.URL);
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "Highlight Listing Main Image height doesn't match for ad: " + mainImage.URL);
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageWidth")), stripeImage.Width, "Highlight Listing Stripe Image width doesn't match for ad: " + mainImage.URL);
            Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.stripeImageHeight")), stripeImage.Height, "Highlight Listing Stripe Image height doesn't match for ad: " + mainImage.URL);
        }
        [Test, Property("TestCaseId", "8559")]
        public void VerifyOLImageOnSRP()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.srpUrlEnhancedListing"); 
            url = new Uri(baseURL + testURL);
            Open();
            var listItem = srp.GetFirstMatchedListing(ListingsType.Org);
            Assert.IsNotNull(listItem, "OL doen't display on SRP ");
            var mainImage = srp.GetListingMainImage(listItem);
            if (viewport == Viewport.XS)
            {
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidthXS")), mainImage.Width, "Ordinary Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeightXS")), mainImage.Height, "Ordinary Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }
            else
            {
                var stripeImage = srp.GetListingStripeImage(listItem);
                Assert.IsNull(stripeImage, "Stripe Image gallery should not be displayed for ordinary listing");
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageWidth")), mainImage.Width, "Ordinary Listing Main Image width doesn't match for ad: " + mainImage.URL);
                Assert.AreEqual(Convert.ToInt32(GetTestData(testDataFile, $"{testcaseId}.mainImageHeight")), mainImage.Height, "Ordinary Listing Main Image height doesn't match for ad: " + mainImage.URL);
            }
        }
        #endregion

    }
}
