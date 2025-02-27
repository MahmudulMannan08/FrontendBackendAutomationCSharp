using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using UIAutomationLibrary.Pages.HubPages;
using static MarketPlaceWeb.Locators.SRPLocators;

namespace SEO.Tests
{
    class EVSRPSEOWidgetTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string srpUrl;
        string baseURL;
        SRPMain srp;
        HubPagesMain hubpages;
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
            srp = new SRPMain(driver, viewport, language);
            hubpages = new HubPagesMain(driver, viewport, language);
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
        #region EVSRPSEO

        [Test]
        public void VerifyIntroDescForGreenVehicle()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.EVSRP") : GetTestData(testDataFile, "IntroDescFr.EVSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc, "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularCars()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Chevrolet Bolt EUV", "Honda Accord Hybrid", "Honda CR-V Hybrid", "Hyundai Sonata Hybrid", "Toyota Prius Prime" };
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }
        }

        [Test]
        public void VerifyByClickingOnInventoryLinkEVSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkEVSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkEVSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllEVSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyCompareLinksForElectricVehicles()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.researchComparisonPage") : baseURL + GetTestData(testDataFile, "urlsFr.researchComparisonPage");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(1, SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
            Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
        }

        [Test]
        public void VerifyCompareLinksForHybridVehicles()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.researchComparisonPage") : baseURL + GetTestData(testDataFile, "urlsFr.researchComparisonPage");
            #endregion
            url = new Uri(srpUrl + "/" + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(3, SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
            Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
        }

        [Test]
        public void VerifyIntroDescForElectricVehicle()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc, "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularElectricCars()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string[] modelNames = { "Chevrolet Bolt EUV", "Hyundai IONIQ 5", "Kia EV6", "Nissan Leaf", "Tesla Model 3" };
            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }
        }

        [Test]
        public void VerifyByClickingOnInventoryLinkElectricSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));
        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkElectricSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string electricSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(srpUrl + "/" + electricSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridVehicle()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridSRP") : GetTestData(testDataFile, "IntroDescFr.HybridSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc, "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularHybridCars()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Honda Accord Hybrid", "Hyundai Santa Fe Hybrid", "Hyundai Sonata Hybrid", "Kia Sportage Hybrid", "Toyota Prius Prime" };
            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }
        }

        [Test]
        public void VerifyByClickingOnInventoryLinkHybridSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));
        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkHybridSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridSRPPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string hybridSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(srpUrl + "/" + hybridSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }
        #endregion


        #region EV+bodyType SRP SEO Widget
        [Test]
        public void VerifyIntroDescForGreenSUVVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularGreenSUV()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Chevrolet Bolt EUV", "Honda CR-V Hybrid", "Hyundai Santa Fe Hybrid", "Kia Sportage Hybrid" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkGreenSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkGreenSUVSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricSUVVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularElectricSUV()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string[] modelNames = { "Chevrolet Bolt EUV", "Kia EV6", "Kia NIRO EV", "Nissan Ariya", "Tesla Model X" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkElectricSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkElectricSUVSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridSUVVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularHybridSUV()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Honda CR-V Hybrid", "Hyundai Santa Fe Hybrid", "Kia Sorento Hybrid", "Kia Sportage Hybrid", "Mitsubishi Outlander PHEV" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkHybridSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkHybridSUVSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridSUVSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2"); String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyCompareLinksForGreenSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(1, SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
        }

        [Test]
        public void VerifyCompareLinksForElectricSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
        }

        [Test]
        public void VerifyCompareLinksForHybridSUVSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
        }


        [Test]
        public void VerifyIntroDescForGreenSedanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularGreenSedan()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Chevrolet Bolt EV", "Honda Accord Hybrid", "Hyundai Elantra Hybrid", "Hyundai Sonata Hybrid", "Toyota Prius Prime" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkGreenSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkGreenSedanSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyCompareLinksForGreenSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(1, SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);


        }

        [Test]
        public void VerifyIntroDescForElectricSedanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularElectricSedan()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string[] modelNames = { "Chevrolet Bolt EV", "Hyundai IONIQ 5", "Hyundai Kona Electric", "Nissan Leaf", "Tesla Model 3" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkElectricSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkElectricSedanSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridSedanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyMMUnderDiscoverPopularHybridSedan()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            string[] modelNames = { "Chevrolet Volt", "Honda Accord Hybrid", "Hyundai Elantra Hybrid", "Hyundai Sonata Hybrid", "Toyota Prius Prime" };

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] modelName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < modelNames.Length; i++)
            {
                Assert.AreEqual(modelNames[i], modelName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyByClickingOnInventoryLinkHybridSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }

        [Test]
        public void VerifyByClickingOnLearnMoreLinkHybridSedanSRPWidget()
        {
            #region Variables
            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);
            Console.WriteLine(mmName);
            Console.WriteLine(hubpages.GetH1TagText());
            Assert.True(hubpages.GetH1TagText().Equals(mmName));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridSedanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyCompareLinksForElectricSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
        }

        [Test]
        public void VerifyCompareLinksForHybridSedanSRPWidget()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ComparePopularCars);
            WaitForPageLoad(30);
        }

        //Trucks

        [Test]
        public void VerifyIntroDescForGreenTruckVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricTruckVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridTruckVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridTruckSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        //Minivans

        [Test]
        public void VerifyIntroDescForGreenMinivanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricMinivanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridMinivanVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridMinivanSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        //Coupe

        [Test]
        public void VerifyIntroDescForGreenCoupeVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricCoupeVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridCoupeVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridCoupeSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        //Wagons

        [Test]
        public void VerifyIntroDescForGreenWagonVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricWagonVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridWagonVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridWagonSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.wagonSRP") : GetTestData(testDataFile, "urlsFr.wagonSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        //Hatchback

        [Test]
        public void VerifyIntroDescForGreenHatchbackVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricHatchbackVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectricHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridHatchbackVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridHatchbackSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hayon";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        //Convertibles

        [Test]
        public void VerifyIntroDescForGreenConvertibleVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GreenBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.GreenBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGreenConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGreenConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.greenSRP") : GetTestData(testDataFile, "urlsFr.greenSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForElectricConvertibleVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.ElectricBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.ElectricBodyTypeSRP");
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkElectricConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllElectriConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.electricSRP") : GetTestData(testDataFile, "urlsFr.electricSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        [Test]
        public void VerifyIntroDescForHybridConvertibleVehicle()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.HybridBodyTypeSRP") : GetTestData(testDataFile, "IntroDescFr.HybridBodyTypeSRP");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            Assert.AreEqual(srp.GetIntroDescSEOWidget().ToLower(), String.Format(IntroDesc.ToLower(), bodyType.ToLower()), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkHybridConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }

        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHybridConvertibleSRPPage()
        {
            #region Variables

            string evSRP = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hybridSRP") : GetTestData(testDataFile, "urlsFr.hybridSRP");
            string bodyTypeUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable";
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + bodyTypeUrl + evSRP);
            Open();
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType2), "The facet value is not correct.");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The Body Type facet value is not correct.");
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            WaitForPageLoad(30);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });
        }

        #endregion
    }
}