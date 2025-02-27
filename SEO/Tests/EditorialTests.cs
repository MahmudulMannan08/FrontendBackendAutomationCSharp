using UIAutomationLibrary.Base;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using MarketPlaceWeb.Base;
using UIAutomationLibrary.Pages.Editorials;
using static UIAutomationLibrary.Locators.EditorialLocators;
using OpenQA.Selenium;
using static MarketPlaceWeb.Locators.SRPLocators;

namespace SEO.Tests.EditorialTests
{
    class EditorialTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string editorialUrl;
        string baseURL;
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
            editorials = new EditorialMain(driver, viewport, language);
            editorialUrl = editorials.editorialURL(language, testDataFile);


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
        public void VerifyNavigationToEditorialPage()
        {
            #region Variables
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.expertReviewsEn") : GetTestData(testDataFile, "pageTitle.expertReviewsFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.expertReviews") : baseURL + GetTestData(testDataFile, "urlsFr.expertReviews");

            #endregion
            url = new Uri(baseURL);
            Open();
            editorials.ClickOnReviewsAndAdvice();
            editorials.ClickOnEitorialLinkUnderReviewsAndAdvice();
            WaitForPageLoad(60);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyEditorialHomeLinkSecNav()
        {
            #region Variables
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.editorialHomePageEn") : GetTestData(testDataFile, "pageTitle.editorialHomePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.editorialHomePage") : baseURL + GetTestData(testDataFile, "urlsFr.editorialHomePage");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.EditorialHomeLink);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyAdviceLinkSecNav()
        {
            #region Variables

            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.AdviceLink);
            WaitForPageLoad(20);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyReviewsLinkSecNav()
        {
            #region Variables

            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.expertReviewsEn") : GetTestData(testDataFile, "pageTitle.expertReviewsFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.expertReviews") : baseURL + GetTestData(testDataFile, "urlsFr.expertReviews");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyNewsLinkSecNav()
        {
            #region Variables

            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.newsEn") : GetTestData(testDataFile, "pageTitle.newsFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.news") : baseURL + GetTestData(testDataFile, "urlsFr.news");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.Newslink);
            WaitForPageLoad(20);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyCoolStuffLinkSecNav()
        {
            #region Variables

            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.coolStuffEn") : GetTestData(testDataFile, "pageTitle.coolStuffFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.coolStuff") : baseURL + GetTestData(testDataFile, "urlsFr.coolStuff");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyVideosLinkSecNav()
        {
            #region Variables

            String PageTitle = GetTestData(testDataFile, "pageTitle.videosEn");
            String CanonicalUrl = baseURL + GetTestData(testDataFile, "urlsEn.videos");
            url = new Uri(baseURL + editorialUrl);
            #endregion
            if (language.ToString() == "EN")
            {
                Open();
                editorials.ClickOnSecondarynavLinks(SecondaryNav.VideosLink);
                WaitForPageLoad(20);
                Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
                Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
                Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            }
            else
            {
                Console.WriteLine("Video menu doesn't exist for French");
            }

        }
        [Test]
        public void VerifySearchLinkSecNav()
        {
            #region Variables
            String searchData = "Honda";
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");

            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.SearchLink);
            editorials.EnterValueInSearchField(searchData);
            WaitForPageLoad(20);
            Assert.AreEqual(PageTitle, editorials.GetPageTitle(), "Page Title is not correct");
            Assert.AreEqual(CanonicalUrl, editorials.GetCanonicalUrl(), "Canonical url is not correct");
            Assert.IsTrue(editorials.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");

        }
        [Test]
        public void VerifyHeroArticleNavigation()
        {
            url = new Uri(baseURL + editorialUrl);

            Open();
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.HeroArticle), "Article is not navigated to article page");


        }

        [Test]
        [Category("Editorial")]
        public void VerifyCarBuyingTipsNavigation()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByCarBuyingTips") : GetTestData(testDataFile, "linkTextFr.filterByCarBuyingTips");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carBuying") : baseURL + GetTestData(testDataFile, "urlsFr.carBuying");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.CarBuyingTips);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyCarSellingTipsNavigation()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByCarSellingTips") : GetTestData(testDataFile, "linkTextFr.filterByCarSellingTips");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carSelling") : baseURL + GetTestData(testDataFile, "urlsFr.carSelling");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.CarSellingTips);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyOwnerTipsNavigation()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByOwnerTips") : GetTestData(testDataFile, "linkTextFr.filterByOwnerTips");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.ownerTips") : baseURL + GetTestData(testDataFile, "urlsFr.ownerTips");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.OwnersTips);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyReviewMainArticleNavigation()
        {
            url = new Uri(baseURL + editorialUrl);
            Open();

            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ReviewsMainArticle), "Article is not navigated to article page");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToReviewsPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByAll") : GetTestData(testDataFile, "linkTextFr.filterByAll");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.expertReviews") : baseURL + GetTestData(testDataFile, "urlsFr.expertReviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyFindReviewByVehicleModel()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.expertReviews") : baseURL + GetTestData(testDataFile, "urlsFr.expertReviews");
            string make = "Acura";
            string model = "ILX";
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToReviewsResultsPage(make, model);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl + "?make=" + make.ToLower() +
                "&model=" + model.ToLower() + "&page=1"), "Find Reviews by vehicle model is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyExpertReviewsMainArticle()
        {
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ExpertReviewsMainArticle), "Article is not navigated to article page");


        }

        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToExpertReviewsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByExpertReviews") : GetTestData(testDataFile, "linkTextFr.filterByExpertReviews");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.reviews") : baseURL + GetTestData(testDataFile, "urlsFr.reviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.ExpertReviews);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");


        }

        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToNewcarPreviewsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByNewCarPreviews") : GetTestData(testDataFile, "linkTextFr.filterByNewCarPreviews");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.newCarPreviews") : baseURL + GetTestData(testDataFile, "urlsFr.newCarPreviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.NewCarPreviews);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToCarComparisonsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByCarComparisons") : GetTestData(testDataFile, "linkTextFr.filterByCarComparisons");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carComparisons") : baseURL + GetTestData(testDataFile, "urlsFr.carComparisons");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.CarComparisons);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToUsedCarReviewsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByUsedCarReviews") : GetTestData(testDataFile, "linkTextFr.filterByUsedCarReviews");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.usedCarReviews") : baseURL + GetTestData(testDataFile, "urlsFr.usedCarReviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.UsedCarReviews);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToCarproductReviewsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByCarProductReviews") : GetTestData(testDataFile, "linkTextFr.filterByCarProductReviews");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carProductReviews") : baseURL + GetTestData(testDataFile, "urlsFr.carProductReviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.CarProductReviews);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNewCarPreviewsMainArticle()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByNewCarPreviews") : GetTestData(testDataFile, "linkTextFr.filterByNewCarPreviews");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.newCarPreviews") : baseURL + GetTestData(testDataFile, "urlsFr.newCarPreviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.NewCarPreviews);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyCarComparisonsMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carComparisons") : baseURL + GetTestData(testDataFile, "urlsFr.carComparisons");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            editorials.ClickElement(editorials.GetSeeAllLinks()[5]);
            WaitForPageLoad(60);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");



        }
        [Test]
        [Category("Editorial")]
        public void VerifyUsedCarMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.usedCarReviews") : baseURL + GetTestData(testDataFile, "urlsFr.usedCarReviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            if (language.ToString() == "EN")
            {
                Open();
                editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
                editorials.ClickElement(editorials.GetSeeAllLinks()[7]);
                WaitForPageLoad(60);
                Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
                Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");
            }


        }

        [Test]
        [Category("Editorial")]
        public void VerifyCarProductReviewsMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carProductReviews") : baseURL + GetTestData(testDataFile, "urlsFr.carProductReviews");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.ExpertReviewsLink);
            if (language.ToString() == "EN")
            {
                editorials.ClickElement(editorials.GetSeeAllLinks()[9]);
            }
            else
            {
                editorials.ClickElement(editorials.GetSeeAllLinks()[7]);
            }
            WaitForPageLoad(60);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");

        }

        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToCoolStuffPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByAll") : GetTestData(testDataFile, "linkTextFr.filterByAll");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.coolStuff") : baseURL + GetTestData(testDataFile, "urlsFr.coolStuff");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Cool stuff url is not working");

        }

        [Test]
        [Category("Editorial")]
        public void VerifyCoolStuffMainArticle()
        {
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            Assert.IsTrue(editorials.NavigateToCoolStuffMainArticle(), "Article is not navigated to article page");
        }

        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToFunStuffCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByFunStuff") : GetTestData(testDataFile, "linkTextFr.filterByFunStuff");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.funStuff") : baseURL + GetTestData(testDataFile, "urlsFr.funStuff");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.FunStuff);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");



        }
        [Test]
        [Category("Editorial")]
        public void VerifyLoadMoreButtonOnFunStuffPage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.funStuff") : baseURL + GetTestData(testDataFile, "urlsFr.funStuff");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.FunStuff);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            if (language.ToString() == "EN")
                Assert.IsTrue(editorials.CheckIfLoadMoreResultsWorks(), "Load more button doesn't work");
        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToCarTechCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByCarTech") : GetTestData(testDataFile, "linkTextFr.filterByCarTech");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carTech") : baseURL + GetTestData(testDataFile, "urlsFr.carTech");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.CarTech);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");


        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToPopCultureCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByPopCulture") : GetTestData(testDataFile, "linkTextFr.filterByPopCulture");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.popCulture") : baseURL + GetTestData(testDataFile, "urlsFr.popCulture");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.PopCulture);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToAdventureCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByAdventure") : GetTestData(testDataFile, "linkTextFr.filterByAdventure");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.adventure") : baseURL + GetTestData(testDataFile, "urlsFr.adventure");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.Adventure);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");



        }
        [Test]
        [Category("Editorial")]
        public void VerifyNavigationToOpinionsCategoryPage()
        {
            #region Variables
            String LinkText = (language.ToString() == "EN") ? GetTestData(testDataFile, "linkTextEn.filterByOpinions") : GetTestData(testDataFile, "linkTextEn.filterByOpinions");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.opinions") : baseURL + GetTestData(testDataFile, "urlsFr.opinions");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.NavigateToCategoryPage(TertiaryHeaderLinks.Opinions);
            string activeOption = editorials.GetActiveLinkText();
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.AreEqual(LinkText, activeOption, "Car Buying Tips link is not working");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyCarTechMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.carTech") : baseURL + GetTestData(testDataFile, "urlsFr.carTech");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.ClickElement(editorials.GetSeeAllLinks()[3]);
            WaitForPageLoad(60);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");

        }
        [Test]
        [Category("Editorial")]
        public void VerifyPopCultureMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.popCulture") : baseURL + GetTestData(testDataFile, "urlsFr.popCulture");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.ClickElement(editorials.GetSeeAllLinks()[5]);
            WaitForPageLoad(60);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");


        }
        [Test]
        [Category("Editorial")]
        public void VerifyAdventureMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.adventure") : baseURL + GetTestData(testDataFile, "urlsFr.adventure");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            editorials.ClickElement(editorials.GetSeeAllLinks()[7]);
            WaitForPageLoad(60);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
            Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");


        }

        [Test]
        [Category("Editorial")]
        public void VerifyOpinionsMainArticle()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.opinions") : baseURL + GetTestData(testDataFile, "urlsFr.opinions");
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
            editorials.ClickOnSecondarynavLinks(SecondaryNav.CoolStuffLink);
            if (language.ToString() == "EN")
            {
                editorials.ClickElement(editorials.GetSeeAllLinks()[9]);
                WaitForPageLoad(60);
                Assert.IsTrue(IsInCurrentUrl(CanonicalUrl));
                Assert.IsTrue(editorials.NavigateToArticleLink(EditorialWidgets.ComparisonPageMainArticle), "Article is not navigated to article page");


            }
        }
        [Test]
        [Category("Editorial")]
        public void VerifyATVVideoRedirectingToVideoPage()
        {
            #region Variables
            String VideoPage = "/autotradertv/video";
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
           
            if (language.ToString() == "EN")
            {
                editorials.ClickOnSecondarynavLinks(SecondaryNav.VideosLink);
                String VideoName = editorials.GetVideoTitleOnATVPage(AutotraderTV.HeroVideoTitle);
                editorials.ClickOnVideoArticleATV(AutotraderTV.WatchNowBtn);
                WaitForPageLoad(60);
                String canonicalURL = editorials.GetCanonicalUrl();
                String VideoPageH1 = editorials.GetVideoTitleOnATVPage(AutotraderTV.VideoPageH1);
                Assert.IsTrue(canonicalURL.Contains(VideoPage), "Video page is not working");
                Assert.AreEqual(VideoName, VideoPageH1, "Video name is not same as H1 on video page");


            }
        }
        [Test]
        public void VerifyATVCollectionPageWorking()
        {
            #region Variables
            String CollectionPage = "/autotradertv/collection";
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
          
            if (language.ToString() == "EN")
            {
                editorials.ClickOnSecondarynavLinks(SecondaryNav.VideosLink);
                editorials.ClickOnVideoArticleATV(AutotraderTV.SeeALLCollectionLink);
                WaitForPageLoad(60);
                String canonicalURL = editorials.GetCanonicalUrl();
                String CollectionpageH1 = editorials.GetH1TitleATV(AutotraderTV.CollectionPageH1);

                Assert.IsTrue(canonicalURL.Contains(CollectionPage), "Video page is not working");
                Assert.AreEqual(CollectionpageH1, "Latest Videos", "See all link is not redirectyed to Collection page");

            }
        }
        [Test]
        public void VerifyATVPlaylistPageWorking()
        {
            #region Variables
            String PlaylistPage = "/autotradertv/playlist";
            #endregion
            url = new Uri(baseURL + editorialUrl);
            Open();
          
            if (language.ToString() == "EN")
            {
                editorials.ClickOnSecondarynavLinks(SecondaryNav.VideosLink);
                WaitForPageLoad(60);
                String PlaylistTitle = editorials.GetPlayListTitleOnATVPage(AutotraderTV.BuyingAVehicleTitle);
                Console.WriteLine(PlaylistTitle);
                editorials.ClickPlaylistTitle(AutotraderTV.BuyingVehiclePlaylistLink,2);
                WaitForPageLoad(60);
                String canonicalURL = editorials.GetCanonicalUrl();
                String PlaylistPageH1 = editorials.GetH1TitleATV(AutotraderTV.PlaylistPageH1);

                Assert.IsTrue(canonicalURL.Contains(PlaylistPage), "Video page is not working");
                Assert.AreEqual(PlaylistPageH1, PlaylistTitle, "See all link is not redirectyed to Collection page");

            }
        }
        [Test]
        public void VerifyReviewArticleHasAllSections()
        {
            #region Variables
            String reviewArticle = (language.ToString() == "EN") ? GetTestData(testDataFile, "ArticleUrlsEn.ReviewArticle") : GetTestData(testDataFile, "ArticleUrlsFr.ReviewArticle");
            #endregion
            url = new Uri(baseURL + reviewArticle);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleSummary), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleH1), "Article H1 is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleCategoryTag), "Article category tag is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.FeaturedImage), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.PublishedDate), "Published is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.Competitors), "Competitors widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.Specifications), "Specifications widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorWidget), "Author widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorName), "Author name is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AutoTraderScores), "Autotrader Scores widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ProsCons), "Pros cons widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.RelatedArticle), "Related articles widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.SocialIcons), "Social Icons widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.YouMayAlsoLikeWidget), "You may also like widget is not visible");


            });

        }
        [Test]
        public void VerifyNewsArticleHasAllTheSections()
        {
            #region Variables
            String newsArticle = (language.ToString() == "EN") ? GetTestData(testDataFile, "ArticleUrlsEn.NewsArticle") : GetTestData(testDataFile, "ArticleUrlsFr.NewsArticle");
            #endregion
            url = new Uri(baseURL + newsArticle);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleSummary), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleH1), "Article H1 is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleCategoryTag), "Article category tag is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.FeaturedImage), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.PublishedDate), "Published is not visible");
               
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorWidget), "Author widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorName), "Author name is not visible");
               Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.RelatedArticle), "Related articles widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.SocialIcons), "Social Icons widget is not visible");
        
            });

        }
        [Test]
        public void VerifyComparisonArticleHasAllTheSections()
        {
            #region Variables
            String comparisonArticle = (language.ToString() == "EN") ? GetTestData(testDataFile, "ArticleUrlsEn.ComparisonArticle") : GetTestData(testDataFile, "ArticleUrlsFr.ComparisonArticle");
            #endregion
            url = new Uri(baseURL + comparisonArticle);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleSummary), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleH1), "Article H1 is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ArticleCategoryTag), "Article category tag is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.FeaturedImage), "Article Summary is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.PublishedDate), "Published is not visible");
                
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorWidget), "Author widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.AuthorName), "Author name is not visible");
          
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.RelatedArticle), "Related articles widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.SocialIcons), "Social Icons widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.YouMayAlsoLikeWidget), "You may also like widget is not visible");
                Assert.IsTrue(editorials.IsArticleWidgetDisplaying(ArticlePage.ComparisonData), "Comparison data widget is not visible");

            });

        }
    }
}
