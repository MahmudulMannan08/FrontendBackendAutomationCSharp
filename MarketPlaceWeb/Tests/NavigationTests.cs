using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.SRP;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Test.NavigationTests
{
    [TestFixture]
    class NavigationTests : Page
    {
        HPMain hp;
        SRPMain srp;
        VDPMain vdp;
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
            hp = new HPMain(driver, viewport, language);
            srp = new SRPMain(driver, viewport, language);
            vdp = new VDPMain(driver, viewport, language);
            shared = new SharedMain(driver, viewport, language);
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

        [Test, Property("TestCaseId", "6538")]
        public void VerifyPVNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6538.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6538.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6538.categoryFr"); 
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.CarsTrucksSuv);
            hp.EnterLocation(location, HPAbstract.Searchbox.New);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.CityPostalCodeChild), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.CarsTrucksSuv);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.CarsTrucksSuv);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains(listingTitle), "HeroCard Title of the Vehicle din't match with SRP Listing Title:" + listingTitle);
        }

        [Test, Property("TestCaseId", "6539")]
        public void VerifyCommercialNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6539.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6539.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6539.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Commercial);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Commercial);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Commercial);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6540")]
        public void VerifyTailersNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6540.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6540.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6540.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Trailers);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Trailers);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Trailers);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6541")]
        public void VerifyRVNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6541.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6541.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6541.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.RVs);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.RVs);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.RVs);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6542")]
        public void VerifyBoatsNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6542.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6542.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6542.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Boats);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Boats);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Boats);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6543")]
        public void VerifyWatercraftNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6543.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6543.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6543.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Watercraft);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Watercraft);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Watercraft);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6606")]
        public void VerifyBikesNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6606.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6606.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6606.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Bikes);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, HeaderFooterLocators.Categories.Bikes, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Bikes);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Bikes);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6607")]
        public void VerifySnowmobilesNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6607.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6607.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6607.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Snowmobiles);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Snowmobiles);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Snowmobiles);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6608")]
        public void VerifyHeavyEquipmentNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6608.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6608.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6608.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.HeavyEquipment);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.HeavyEquipment);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.HeavyEquipment);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }

        [Test, Property("TestCaseId", "6609")]
        public void VerifyFarmNavigationFromHpToVdp()
        {
            #region Variables
            string location = GetTestData(testDataFile, "6609.location");
            string selectedCategoryEn = GetTestData(testDataFile, "6609.categoryEn");
            string selectedCategoryFr = GetTestData(testDataFile, "6609.categoryFr");
            #endregion
             url = new Uri(baseURL);
            Open();
            shared.ClickCategory(HeaderFooterLocators.Categories.Farm);
            hp.EnterLocation(location);
            hp.ClickOnSearch();
            srp.WaitForSRPPageLoad();
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.LocationParent, srpDesign: SrpDesign.Old), "The location doesn't match HomePage Location.");
            string verifyCategory = (language.ToString() == "EN") ? selectedCategoryEn : selectedCategoryFr;
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            string listingTitle = srp.GetOrganicListingTitle(0, HeaderFooterLocators.Categories.Farm);
            srp.ClickOnFirstOrganicListing(HeaderFooterLocators.Categories.Farm);
            Assert.AreEqual(verifyCategory, shared.ActiveCategory(), "Active Category doesn't match with what was selected");
            Assert.IsTrue(vdp.GetVDPTitle().Contains((listingTitle)), "HeroCard Title of the Vehicle din't match with SRP Listing Title");
        }
    }
}
