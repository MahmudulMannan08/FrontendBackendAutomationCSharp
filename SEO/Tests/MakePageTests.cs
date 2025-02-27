using MarketPlaceWeb.Base;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using UIAutomationLibrary.Locators;
using UIAutomationLibrary.Pages.Editorials;
using UIAutomationLibrary.Pages.HubPages;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace SEO.Tests
{
    public class MakePageTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string researchURL;
        string baseURL;
        HubPagesMain hubpages;
        EditorialMain editorials;
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
            hubpages = new HubPagesMain(driver, viewport, language);
            editorials= new EditorialMain(driver, viewport, language);
            researchURL = hubpages.researchPageURL(language, testDataFile);

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



        [Test]
        public void VerifyLatestVehiclesStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + "/" + make);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.LatestVehiclesStickyTab), "it didn't navigate to Reseach by make section");
        }

        [Test]

        public void VerifySUVsStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.SUVsStickyTabMakePage), "it didn't navigate to SUVs section");
        }
        [Test]

        public void VerifySedansStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.TrucksStickyTabMakePage), "it didn't navigate toTrucks section");
        }
        [Test]
        public void VerifyCoupesStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.SedansStickyTabMakePage), "it didn't navigate to Sedans section");
        }
        [Test]
        public void VerifyTrucksStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.CoupesStickyTabMakePage), "it didn't navigate to Coupes section");
        }
        [Test]
        public void VerifyHatchBacksStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.HatchbacksStickyTabMakePage), "it didn't navigate to Hatchbacks section");
        }
        [Test]
        public void VerifyMinivansStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.MinivansStickyTabMakePage), "it didn't navigate to Wagons section");
        }
        [Test]
        public void VerifyConvertiblesStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.ConvertiblesTabMakePage), "it didn't navigate to Minivans section");
        }
        [Test]
        public void VerifyArticlesStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + "/" + make);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.ArticlesStickyTabMakePage), "it didn't navigate to Minivans section");
        }
        [Test]
        public void VerifyInventoryStickyNav()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.InventoryStickyTabMakePage), "it didn't navigate to Minivans section");
        }
       
        [Test]

        public void VerifyNavigationToYMMPageSUV()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.SUVs, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageSedans()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Sedans, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageCoupes()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Coupes, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageTrucks()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Trucks, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageHatchbacks()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Hatchbacks, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageMiniVans()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Minivans, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]

        public void VerifyNavigationToYMMPageConvertibles()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Convertibles, make);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
        public void VerifyViewAllArticleBtnNavigatesToEditorialPage()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickOnViewAllArticleBtn();
            Assert.IsTrue(IsInCurrentUrl(make), "View all article btn is not redirecting to searchpage");
        }
        [Test]
        [Category("ResearchPage")]
        public void VerifyNavigationOfArticleToArticlePage()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.NavigateToArticlePage(ResearchPage.ArticlesCarouselImages);
            WaitForPageLoad(30);
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Article page is not working");
        }
        [Test]
        public void VerifyImagesAreLoadedArticles()
        {
            string make = "Honda";
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoadedForArticleWidgetResearchPage(ResearchPage.ArticlesCarouselImages), "image is missing");
        }
        [Test]
        public void VerifyNavigationOfViewAllDealsInventoryBtn()
        {
            string make = "Honda";
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.ClickViewAllDealsBtn();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl + make.ToLower()), "View all deals btn didn't  redirect to srp page");
        }
        [Test]
        public void VerifyNavigationToYMMPageUsingDropdown()
        {
            #region Variables
            String make = "Audi";
            String make1 = "Honda";
            String model1 = "Civic";
            String year1 = "2020";

            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower());
            Open();
            hubpages.NavigateToYMMPageUsingDropDown(make1, model1, year1,make);
           hubpages.WaitForPageLoad(make1.ToLower());
            Assert.IsTrue(hubpages.IsInCurrentUrl(make1), "BrowserUrl doesn't match with Canonical Url");

        }
    }
}
