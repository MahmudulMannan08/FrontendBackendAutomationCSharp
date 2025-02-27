using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using UIAutomationLibrary.Pages.Editorials;

namespace SEO.Tests
{
    class SEOHomePageTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;
        SRPMain srp;
        HPMain hp;
        EditorialMain editorials;
        string baseURL;

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
            hp = new HPMain(driver, viewport, language);
            editorials = new EditorialMain(driver, viewport, language);
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
        public void VerifyAllSEOWidgetsAreDisplaying()
        {
            url = new Uri(baseURL);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.BrowseByBodyTypeWidget), "Browse By BodyType widget is not displaying");
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.AutoTraderAwardWinnersWidget), "Autotrdaer awards widget is not displaying");
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.LatestNewsReviewsWidget), "Latest News reviews widget is not displaying");

                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.NewArrivalWidget), "New arrival widget is not displaying");
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.MostPopularCarsWidget), "Most popular cars widget is not displaying");

            });
            if (language.ToString() == "EN")
            {
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.EditorialVideoWidget), "Video  widget is not displaying");
            }
        }
        [Test]
        public void VerifyNavigationOfSeeAllWinnersLink()
        {
            String AwardsTitle = (language.ToString() == "EN")? "Meet the Winners": "Voyez les gagnants";
            url = new Uri(baseURL);
            Open();
            WaitForPageLoad(60);
            hp.ClickSEOLink(HPLocators.SEOWidgets.SeeAllAwardsLink);
            Assert.IsTrue(editorials.GetAwardsPageH1Title().Equals(AwardsTitle), "See all link is not redirecting to awards page");

        }
        [Test]
        public void VerifyNavigationOfLearnMoreLinkToArticlePage()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.LearMoreAwardsLink);
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Learn more link didn't redirect to article page");
        }
        [Test]
        public void VerifyNavigationOfMMforSaleLinkToSRPPage()
        {
            url = new Uri(baseURL);
            Open();
            string MMName = hp.GetMMNameAwardWidget(HPLocators.SEOWidgets.AwardsMMName);
            hp.ClickSEOLink(HPLocators.SEOWidgets.AwardsMMForSaleLink);
            Assert.IsTrue(srp.GetTitleText().Contains(MMName), "MM for sale link didn't redirect to SRP page");
        }
        [Test]
        public void VerifyNavigationOfArticleToArticlePage()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.NewsReviewsArticleLink);
            WaitForPageLoad(20);
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Learn more link didn't redirect to article page");
        }
        [Test]
        public void VerifyNavigationOfViewAllLinkToEditorialPage()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.NewsReviewsViewAllLink);
            Assert.IsTrue(hp.IsInCurrentUrl("editorial"), "Learn more link didn't redirect to article page");
        }
        [Test]
        public void VerifyNewArrivalsExploreLinkNavToYMMpage()
        {
            String ResearchPage = (language.ToString() == "EN") ? "research" : "recherche";
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.NewArrivalsExploreLink);
            Assert.IsTrue(hp.IsInCurrentUrl(ResearchPage), "Learn more link didn't redirect to article page");
        }
        [Test]
        public void VerifyNewArrivalsViewInventoryLinkNavToSRPPage()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.NewArrivalsViewInventoryLink);
            Assert.IsTrue(srp.GetTitleText().Contains("2023"), "New arrival View inventlink didn't redirect to SRP page");
        }

        #region VideoWidget
        [Test]
        public void VerifyAutoATraderTVLogoIsDisplaying()
        {
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.EditorialVideoWidget), "Video  widget is not displaying");
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.AutoTraderTVLogo), "AutoTraderTV logo is not displaying");
            }
        }

        [Test]
        public void VerifyUpNextHeaderIsDisplaying()
        {
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.UpNextHeader), "UpNext Header is not displaying");
            }
        }

        [Test]
        public void VerifyLatestVideosAresDiplayed()
        {
            url = new Uri(baseURL);
            Open();

            if (language.ToString() == "EN")
            {
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.ATPrimaryVideoLinks), "Primary Video link is not displaying");
                    Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.ATVideoLinks), "Video links are not diplaying");
                    Assert.AreEqual(8, hp.LatestATVVideoLinks().Count, "Number of Latest videos displayed is incorrect");
                });
            }
        }

        [Test]
        public void VerifyClickingOnSeeAllLink()
        {
            String CanonicalUrl = "editorial/autotradertv/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                //SeeAll Link
                hp.ClickATVSEOLink(9);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found") );
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser is not redirected to AT TV homepage  ");
            }
        }
        [Test]
        public void VerifyClickingOnAllVideoLinks()
        {
            String CanonicalUrl = "editorial/autotradertv/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                var latestVideoLinks = hp.LatestATVVideoLinks();
                var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestVideoLinks.Count; i++)
                    {
                        Open();
                        hp.ClickATVSEOLink(i);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                    }
                });
            }
        }

        [Test]
        public void VerifyClickingOnPrimaryVideoLink()
        {
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                string tvTitle = hp.GetTVWidgetTitle(HPLocators.SEOWidgets.ATVtitle);
                hp.ClickSEOLink(HPLocators.SEOWidgets.ATVtitle);
                Assert.IsTrue(editorials.GetPageTitle().Contains(tvTitle), "AutoTrader TV video link didn't redirect to expected page");
            }

        }
        #endregion
        #region Article Widget
        [Test]
        public void VerifyEditorialWidgetIsDisplaying()
        {
            url = new Uri(baseURL);
            Open();
            Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.EditorialArticleWidget), "Editorial Article widget is not displaying");
        }
        [Test]
        public void VerifyEditorialLogoIsDisplaying()
        {
            url = new Uri(baseURL);
            Open();
            Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.EditorialArticleLogo), "Editorial Article widget is not displaying");
        }
        [Test]
        public void VerifyLatestEditorialTabIsHighlighted()
        {
            url = new Uri(baseURL);
            Open();
            Assert.IsTrue( hp.NavigateToHotOffTab(HPLocators.SEOWidgets.EditorialLatestTab), "The current Latest Tab isn't highlighted in Red");
        }

        [Test]
        public void VerifyEditorialCoolStuffTabIsHighlighted()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialCoolStuffTab);
            Assert.IsTrue(hp.NavigateToHotOffTab(HPLocators.SEOWidgets.EditorialCoolStuffTab), "The current CoolStuff Tab isn't highlighted in Red");
        }

        [Test]
        public void VerifyEditorialAdviceTabIsHighlighted()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialAdviceTab);
            Assert.IsTrue(hp.NavigateToHotOffTab(HPLocators.SEOWidgets.EditorialAdviceTab), "The current CoolStuff Tab isn't highlighted in Red");
        }

        [Test]
        public void VerifyEditorialReviewsTabIsHighlighted()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialReviewsTab);
            Assert.IsTrue(hp.NavigateToHotOffTab(HPLocators.SEOWidgets.EditorialReviewsTab), "The current CoolStuff Tab isn't highlighted in Red");
        }

        [Test]
        public void VerifyEditorialNewsTabIsHighlighted()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialNewsTab);
            Assert.IsTrue(hp.NavigateToHotOffTab(HPLocators.SEOWidgets.EditorialNewsTab), "The current CoolStuff Tab isn't highlighted in Red");
        }

        [Test]
        public void VerifyLatestArtivlesAresDiplayed()
        {
            url = new Uri(baseURL);
            Open();

            if (language.ToString() == "EN")
            {
                Assert.IsTrue(hp.IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets.EditorialLatestLeftArticle), "EditorialMain Article is missing");
                Assert.AreEqual(4, hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialLatestArticles).Count, "Number of Latest videos displayed is incorrect");
            }
        }
        [Test]
        public void VerifyClickingOnAllLatestArticleLinks()
        {
            String CanonicalUrl = "editorial/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
               var latestArticleLinks = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialLatestArticles);
               var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestArticleLinks.Count; i++)
                    {
                        Open();
                        string expectedTitle = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialLatestArticles)[i].Text;
                        hp.ClickEditorialLinks(i, HPLocators.SEOWidgets.EditorialLatestArticles);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                        Assert.IsTrue(editorials.GetPageTitle().Contains(expectedTitle), "Editorial Article link didn't redirect to expected page");
                    }
                });
            }
        }

        [Test]
        public void VerifyClickingOnAllAdvicetArticleLinks()
        {
            String CanonicalUrl = "editorial/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                var latestArticleLinks = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialAdviceArticles);
                var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestArticleLinks.Count; i++)
                    {
                        Open();
                        hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialAdviceTab);
                        string expectedTitle = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialAdviceArticles)[i].Text;
                        hp.ClickEditorialLinks(i, HPLocators.SEOWidgets.EditorialAdviceArticles);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                        Assert.IsTrue(editorials.GetPageTitle().Contains(expectedTitle), "Editorial Article link didn't redirect to expected page");
                    }
                });
            }
        }

        [Test]
        public void VerifyClickingOnAllReviewsArticleLinks()
        {
            String CanonicalUrl = "editorial/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                var latestArticleLinks = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialReviewtArticles);
                var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestArticleLinks.Count; i++)
                    {
                        Open();
                        hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialReviewsTab);
                        string expectedTitle = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialReviewtArticles)[i].Text;
                        hp.ClickEditorialLinks(i,HPLocators.SEOWidgets.EditorialReviewtArticles);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                        Assert.IsTrue(editorials.GetPageTitle().Contains(expectedTitle), "Editorial Article link didn't redirect to expected page");
                    }
                });
            }
        }

        [Test]
        public void VerifyClickingOnAllNewsArticleLinks()
        {
            String CanonicalUrl = "editorial/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                var latestArticleLinks = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialNewsArticles);
                var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestArticleLinks.Count; i++)
                    {
                        Open();
                        hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialNewsTab);
                        string expectedTitle = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialNewsArticles)[i].Text;
                        hp.ClickEditorialLinks(i, HPLocators.SEOWidgets.EditorialNewsArticles);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                        Assert.IsTrue(editorials.GetPageTitle().Contains(expectedTitle), "Editorial Article link didn't redirect to expected page");
                    }
                });
            }
        }

        [Test]
        public void VerifyClickingOnAllCoolStuffArticleLinks()
        {
            String CanonicalUrl = "editorial/";
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                var latestArticleLinks = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialCoolStuffArticles);
                var isValidLink = true;
                Assert.Multiple(() =>
                {
                    for (int i = 0; i < latestArticleLinks.Count; i++)
                    {
                        Open();
                        hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialCoolStuffTab);
                        string expectedTitle = hp.EditoralArticleLinks(HPLocators.SEOWidgets.EditorialCoolStuffArticles)[i].Text;
                        hp.ClickEditorialLinks(i, HPLocators.SEOWidgets.EditorialCoolStuffArticles);
                        if (!(hp.IsInCurrentUrl(CanonicalUrl)) || driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"))
                        { isValidLink = false; }
                        Assert.True(isValidLink, $"Video link is broken");
                        isValidLink = true;
                        Assert.IsTrue(editorials.GetPageTitle().Contains(expectedTitle), "Editorial Article link didn't redirect to expected page");
                    }
                });
            }
        }

        [Test]
        public void VerifyLatestAticleSeeAllLink()
        {
            String CanonicalUrl = "editorial/";
            string expectedTitle = language.ToString() == "EN" ? GetTestData(testDataFile, "pageTitle.editorialHomePageEn") : GetTestData(testDataFile, "pageTitle.editorialHomePageFr");
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialLatestSeeAllLink);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"));
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser didn't redirect to editorial page");
                string actualTitle = driver.Title;
                Assert.AreEqual(expectedTitle,actualTitle,"Browser didn't redirect to editorial page");
            }
        }

        [Test]
        public void VerifyAdviceAticleSeeAllLink()
        {
            String CanonicalUrl = "editorial/";
            string expectedTitle = language.ToString() == "EN" ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialAdviceTab);
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialAdviceSeeAllLink);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"));
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser didn't redirect to editorial page");
                string actualTitle = driver.Title;
                Assert.AreEqual(expectedTitle, actualTitle, "Browser didn't redirect to editorial page");
            }
        }

        [Test]
        public void VerifyReviewsAticleSeeAllLink()
        {
            String CanonicalUrl = "editorial/";
            string expectedTitle = language.ToString() == "EN" ? GetTestData(testDataFile, "pageTitle.expertReviewsEn") : GetTestData(testDataFile, "pageTitle.expertReviewsFr");
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialReviewsTab);
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialReviewsSeeAllLink);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"));
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser didn't redirect to editorial page");
                string actualTitle = driver.Title;
                Assert.AreEqual(expectedTitle, actualTitle, "Browser didn't redirect to editorial page");
            }
        }

        [Test]
        public void VerifyNewsAticleSeeAllLink()
        {
            String CanonicalUrl = "editorial/";
            string expectedTitle = language.ToString() == "EN" ? GetTestData(testDataFile, "pageTitle.newsEn") : GetTestData(testDataFile, "pageTitle.newsFr");
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialNewsTab);
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialNewsSeeAllLink);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"));
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser didn't redirect to editorial page");
                string actualTitle = driver.Title;
                Assert.AreEqual(expectedTitle, actualTitle, "Browser didn't redirect to editorial page");
            }
        }
        [Test]
        public void VerifyCoolStuffAticleSeeAllLink()
        {
            String CanonicalUrl = "editorial/";
            string expectedTitle = language.ToString() == "EN" ? GetTestData(testDataFile, "pageTitle.coolStuffEn") : GetTestData(testDataFile, "pageTitle.coolStuffFr");
            url = new Uri(baseURL);
            Open();
            if (language.ToString() == "EN")
            {
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialCoolStuffTab);
                hp.ClickSEOLink(HPLocators.SEOWidgets.EditorialCoolStuffSeeAllLink);
                Assert.IsFalse(driver.Title.Contains("Oops") || driver.Title.Contains("404") || driver.Title.Contains("Page not found"));
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "Browser didn't redirect to editorial page");
                string actualTitle = driver.Title;
                Assert.AreEqual(expectedTitle, actualTitle, "Browser didn't redirect to editorial page");
            }
        }

        #endregion

    }
    }
