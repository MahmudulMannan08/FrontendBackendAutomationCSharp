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
    public class MakeModelTests : Page
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
            editorials = new EditorialMain(driver, viewport, language);
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
        public void VerifyViewInventoryBtnNavigation()
        {
            string make = "Honda";
            string model = "Civic";
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/");
            Open();
            hubpages.ClickViewAllDealsBtn();
            WaitForPageLoad(20);
            hubpages.CloseCookieBanner();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl + make.ToLower() + "/" + model.ToLower()), "View all deals btn didn't  redirect to srp page");
            Assert.IsTrue(hubpages.GetSRPHeader().Contains(make), "srp page is broken");
        }
        [Test]
        public void VerifyAlltheSectionsAreShowingOnPage()
        {
            string make = "Honda";
            string model = "Civic";
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/");
            Open();
            Assert.IsTrue(hubpages.IsModelOverviewWidgetDisplaying(), "Model overview widget is not displaying");
            Assert.IsTrue(hubpages.AreExploreLinksDisplaying(), "Explore links widget is not displaying");
            Assert.IsTrue(hubpages.IsPhotoWidgetDisplaying(), "Photos widget is not displaying");
            Assert.IsTrue(hubpages.IsOwnerScoresWidgetDisplaying(), "Owner scores widget is not displaying");
            Assert.IsTrue(hubpages.IsReviewsNewsWidgetDisplaying(), "Reviews and News widget is not displaying");
            Assert.IsTrue(hubpages.IsRecallInfoWidgetDisplaying(), "Recall info widget is not displaying");
            Assert.IsTrue(hubpages.IsInventoryWidgetDisplaying(), "Inventory widget is not displaying");
            Assert.IsTrue(hubpages.IsVehicleResearchDropDownDisplaying(), "Vehicle Research widget is not displaying");


        }
        [Test]
        public void VerifyNavigationOfExploreYMMLink()
        {
            string make = "Honda";
            string model = "Civic";
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/");
            Open();
            hubpages.ClickExploreMakeModelYearLink();
           WaitForPageLoad(20);
            Assert.IsTrue(hubpages.AreHeroScoresPresentOnYMMPage(), "View details button redirects to YMM page");

        }
        [Test]
        public void VerifyNavigationOfArticlelinkToArticlePage()
        {
            string make = "Honda";
            string model = "Civic";
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower());
            Open();
            hubpages.NavigateToArticlePage(ResearchPage.ArticlesCarouselImages);
            WaitForPageLoad(20);
            Assert.IsTrue(IsInCurrentUrl("editorial"), "Article didn't redirect to article page");
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Article page is not working");
        }

        [Test]
        public void VerifyNavigationOfViewAllOwnerReviewsLink()
        {
            string make = "Honda";
            string model = "Civic";

            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower());
            Open();
            hubpages.ClickOwnerReviewsLink();
            WaitForPageLoad(30);
            Assert.IsTrue(IsInCurrentUrl("reviews"), "Article didn't redirect to article page");
        }
        [Test]
        public void VerifyTransportCanadaLink()
        {
            string make = "Honda";
            string model = "Civic";

            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower());
            Open();
            hubpages.ClickTransportCanadaLink();
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            WaitForPageLoad(90);
            Assert.IsTrue(IsInCurrentUrl("tc.canada.ca"), "Transport Canada link is not working");
        }
        [Test]
        public void VerifyNavigationToYMMPageUsingDropdown()
        {
            #region Variables
            string make = "Honda";
            string model = "Civic";
            string make1 = "Audi";
            string model1 = "A3";
            string year1 = "2022";

            #endregion
            url = new Uri(baseURL + researchURL + make.ToLower() + "/" + model.ToLower() + "/");
            Open();
            hubpages.NavigateToYMMPageUsingDropDown(make1, model1, year1, make, model);
            WaitForPageLoad(20);
            Assert.IsTrue(hubpages.IsInCurrentUrl(make1), "BrowserUrl doesn't match with Canonical Url");
        }
    }
}
