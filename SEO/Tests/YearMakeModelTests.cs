using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections;
using UIAutomationLibrary.Locators;
using UIAutomationLibrary.Pages.Editorials;
using UIAutomationLibrary.Pages.HubPages;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace SEO.Tests
{
    public class YearMakeModelTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string researchURL;
        string baseURL;
        HubPagesMain hubpages;
        EditorialMain editorials;
        SRPMain srp;
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
            researchURL = hubpages.researchPageURL(language, testDataFile);
            editorials = new EditorialMain(driver, viewport, language);
            srp = new SRPMain(driver, viewport, language);

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
        public void VerifyAlltheSectionsAreShowingOnPage()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.HeroSection), "Hero section widget is not displaying");

                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.PhotosSection), "Photos widget is not displaying");

                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.ReviewsNewsWidget), "Reviews and News widget is not displaying");
                Assert.IsTrue(hubpages.IsRecallInfoWidgetDisplaying(), "Recall info widget is not displaying");
                Assert.IsTrue(hubpages.IsInventoryWidgetDisplaying(), "Inventory widget is not displaying");
                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.SimilarVehiclesWidget), "Similar vehicle widget is not displaying");
                Assert.IsTrue(hubpages.IsVehicleResearchDropDownDisplaying(), "Vehicle Research widget is not displaying");

                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.TrimComparison), "Trim Comparison widget is not displaying");
                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.ATScoresWidget), "Auto trdaer scores widget is not displaying");
            });

        }
        [Test]
        public void VerifyAlltheSectionsSpecificToYMMAreShowingOnPage()
        {
            #region Variables
            string make = (language.ToString() == "EN") ? GetTestData(testDataFile, "YearMakeModelData.ymm2.make") : GetTestData(testDataFile, "YearMakeModelData.ymm3.make");
            string model = (language.ToString() == "EN") ? GetTestData(testDataFile, "YearMakeModelData.ymm2.model") : GetTestData(testDataFile, "YearMakeModelData.ymm3.model");
            string year = (language.ToString() == "EN") ? GetTestData(testDataFile, "YearMakeModelData.ymm2.year") : GetTestData(testDataFile, "YearMakeModelData.ymm3.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.SocialIcons), "Social Icon widget is not displaying");
                Assert.IsTrue(hubpages.IsModelOverviewWidgetDisplaying(), "Model overview widget is not displaying");
               
            });
            if (language.ToString() == "EN")
            {
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.QnAWidget), "QnA widget is not displaying");

                    Assert.IsTrue(hubpages.AreYMMWidgetsDisplaying(YearMakeModelPage.VideoWidget), "Video widget is not displaying");

                });
            }


        }
        [Test]
        public void VerifyScoreMatchesWithHeroScore()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model =GetTestData(testDataFile, "YearMakeModelData.ymm.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            String heroScore = hubpages.GetATHeroScore();
            String badgeScore = hubpages.GetATBadgeScore();
            Assert.IsTrue(heroScore.Equals(badgeScore), "Scores don't match on hero section and AT widget");

        }
        [Test]
        public void VerifyATScoresReadMoreLinkNavigation()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            hubpages.ClickATScoreReadMoreLink();
            WaitForPageLoad(20);
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Read more link didn't redirect to article page");


        }

        [Test]
        public void VerifyNationalInventorySpecificToYMM()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            string title = hubpages.GetInventoryTitle();
            Assert.IsTrue(title.Contains(make), "Inventory is not mapped with vehicle");

        }

        [Test]
        public void VerifyNavigationOfViewAllDealsBtnNational()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year + "/");
            Open();
            hubpages.ClickViewAllDealsBtn();
            hubpages.CloseCookieBanner();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(IsInCurrentUrl(CanonicalUrl + make.ToLower() + "/" + model.ToLower() + "/" + year), "View all deals btn didn't  redirect to srp page");
                Assert.IsTrue(srp.GetTitleText().Contains(make), "srp page is broken");
            });

        }

        [Test]
        public void VerifyCalgaryInventoryMappedToCity()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            hubpages.ClickCityTabInventoryWidget(YearMakeModelPage.CalgaryTab);
            string city = hubpages.GetInventoryCity();
            Assert.IsTrue(city.Contains("AB"), "Calgary inventoryis not mapped with location");
        }


        [Test]
        public void VerifyArticlesNavigatingToCorrectUrls()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            hubpages.NavigateToArticlePage(ResearchPage.ArticlesCarouselImages);
            WaitForPageLoad(20);
            Assert.IsTrue(IsInCurrentUrl("editorial"), "Article didn't redirect to article page");
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Article page is not working");
        }
        [Test]
        public void VerifyIfArticlesAreDisplayingInChronologicalOrder()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            int count=hubpages.GetCountOfArticles();
            ArrayList publishedDates=hubpages.GetArticlesPublishedDates(count);
            Assert.IsTrue(hubpages.AreArticleInChronologicalOrder(publishedDates),"Articles are not displaying in chronological order");

        }

        [Test]
        public void VerifyIfArticlesAreDistinct()
        {
            #region Variables
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm1.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm1.year");
            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/" + year);
            Open();
            int count = hubpages.GetCountOfArticles();
           string[] articleTitles= hubpages.GetArticleTitles(count);
            int distinctCount = hubpages.GetCountOfDistinctArticles(articleTitles);
            Assert.IsTrue(count.Equals(distinctCount), "Duplicate Articles are showing in news reviews widget");

        }
       
    }
}
