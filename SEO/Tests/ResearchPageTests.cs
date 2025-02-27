using MarketPlaceWeb.Base;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Locators;
using UIAutomationLibrary.Pages.Editorials;
using UIAutomationLibrary.Pages.HubPages;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace SEO.Tests
{
    public class ResearchPageTests : Page
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
       
        public void VerifyNavigationToResearchPage()
        {

            #region Variables
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.researchPageEn") : GetTestData(testDataFile, "pageTitle.researchPageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.researchPage") : baseURL + GetTestData(testDataFile, "urlsFr.researchPage");

            #endregion
            url = new Uri(baseURL);
            Open();

            hubpages.ClickOnReviewsAndAdvice();
            hubpages.ClickOnVehicleResearchLinkUnderReviewsAndAdvice();
            WaitForPageLoad(20);
            Assert.AreEqual(PageTitle, hubpages.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, hubpages.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(hubpages.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");


        }
        [Test]
        
        public void VerifyHomeBreadCrumb()
        {
            #region Variables
           
            String CanonicalUrl =  baseURL ;

            #endregion
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickHomeBreadCrumb();
            Assert.IsTrue(hubpages.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");


        }
       
        [Test]
        [Category("ResearchPage")]
        public void VerifyNavigationToYMMPageUsingDropdown()
        {
            #region Variables
            String make = "Honda";
            String model = "Civic";
            String year = "2020";
           
            #endregion
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.NavigateToYMMPageUsingDropDown(make,model,year);
            WaitForPageLoad(90);
            Assert.IsTrue(hubpages.IsInCurrentUrl(make), "BrowserUrl doesn't match with Canonical Url");


        }
        [Test]
        public void VerifyCompareVehicles()
        {
            #region Variables
            String make1 = "Buick";
            String model1 = "Enclave";
            String year1 = "2020";
            String make2 = "Audi";
            String model2 = "A5";
            String year2 = "2020";

            #endregion
            string H1Tag = (language.ToString() == "EN")?"Compare":"Comparer";
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.AddVehiclesInComparisonTool(make1,model1,year1,make2,model2,year2);
            Wait(5);
            Assert.IsTrue(hubpages.GetVehicleTitle(2).Contains(make1), "It's not redirecting to correct page");
        }
       

        [Test]
        public void VerifyResearchByMakeNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.ResearchmakeStickyTab), "it didn't navigate to Reseach by make section");
        }
        [Test]
        
        public void VerifyCompareVehiclesNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.CompareStickyTab), "it didn't navigate to compare vehicles section");
        }
        [Test]
       
        public void VerifySUVsStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.SUVsStickyTab), "it didn't navigate to SUVs section");
        }
        [Test]
        
        public void VerifyTrucksStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.TrucksStickyTab), "it didn't navigate toTrucks section");
        }
        [Test]
        public void VerifySedansStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.SedansStickyTab), "it didn't navigate to Sedans section");
        }
        [Test]
        public void VerifyCoupesStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.CoupesStickyTab), "it didn't navigate to Coupes section");
        }
        [Test]
        public void VerifyHatchBacksStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.HatchbacksStickyTab), "it didn't navigate to Hatchbacks section");
        }
        [Test]
        public void VerifyWagonsStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.WagonsStickyTab), "it didn't navigate to Wagons section");
        }
        [Test]
        public void VerifyMinivansStickyNav()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.NavigateToSectionUsingStickyNav(HubPagesLocators.StickyNavs.MinivansStickyTab), "it didn't navigate to Minivans section");
        }
        [Test]
       
        public void VerifyNavigationOfAllLogosTOMakePage()
        {
            #region Variables
            string H1 = "Ford";
            #endregion

            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.NavigateToMakePageUsingResearchByMakeLogo();
            string h1Tag=hubpages.GetH1TagText();
            Assert.AreEqual(H1, h1Tag, "H1 tag doesn't match");
            Assert.IsTrue(IsInCurrentUrl(H1.ToLower()), "Url is not correct");
            
        }
        [Test]
      
        public void VerifyImagesAreLoadedSUVs()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.SUVs), "image is missing");
        }
        [Test]
        
        public void VerifyNavigationToYMMPageSUV()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.SUVs);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");
           
        }
        [Test]
       
        public void VerifyImagesAreLoadedTrucks()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Trucks), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageTrucks()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Trucks);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
       
        public void VerifyImagesAreLoadedSedans()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Sedans), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageSedans()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Sedans);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
       
        public void VerifyImagesAreLoadedCoupes()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Coupes), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageCoupes()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Coupes);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
        
        public void VerifyImagesAreLoadedHatchbacks()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Hatchbacks), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageHatchbacks()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Hatchbacks);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
       
        public void VerifyImagesAreLoadedWagons()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Wagons), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageWagons()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Wagons);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
        public void VerifyImagesAreLoadedMinivans()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoaded(ResearchPage.Minivans), "image is missing");
        }
        [Test]

        public void VerifyNavigationToYMMPageMinivans()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewDetailsBtn(ResearchPage.Minivans);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
        public void VerifyViewAllArticleBtnNavigatesToEditorialPage()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.ClickOnViewAllArticleBtn();
            Assert.IsTrue(IsInCurrentUrl("editorial"), "View all article btn is not redirecting to editorialpage");
        }

        [Test]
        public void VerifyImagesAreLoadedArticles()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            Assert.IsTrue(hubpages.CheckIfAllImagesAreLoadedForArticleWidgetResearchPage(ResearchPage.ArticlesCarouselImages), "image is missing");
        }
        [Test]
        [Category("ResearchPage")]
        public void VerifyNavigationOfArticleToArticlePage()
        {
            url = new Uri(baseURL + researchURL);
            Open();
            hubpages.NavigateToArticlePage(ResearchPage.ArticlesCarouselImages);
            WaitForPageLoad(20);
            Assert.IsTrue(IsInCurrentUrl("editorial"), "Article didn't redirect to article page");
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(),"Article page is not working");
        }
    }
}
