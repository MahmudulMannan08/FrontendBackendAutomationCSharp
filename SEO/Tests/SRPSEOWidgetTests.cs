using MarketPlaceWeb.Base;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using System;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Locators;
using static MarketPlaceWeb.Locators.SRPLocators;
using UIAutomationLibrary.Pages.HubPages;
using MarketPlaceWeb.Pages.HP;


namespace SEO.Tests
{
    class SRPSEOWidgetTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string srpUrl;
        string baseURL;
        SRPMain srp;
        HPMain hp;
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
            hp = new HPMain(driver, viewport, language);

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

        #region YMM SRP
        [Test]
        public void VerifySEOwidgetDisplayingOnYMMSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm4.year");

            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            Assert.IsTrue(srp.IsSEOWidgetDisplayingOnSRPPage(SRPLocators.SEOLinks.SEOWidget), "Widget is not showing");

        }
        [Test]
        public void VerifyIntroDescForYMMSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.YMM") : GetTestData(testDataFile, "IntroDescFr.YMM");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm4.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc, "Description is wrong");
        }
        [Test]
        public void VerifyResearchMoreLinkNaviagtionOnYMMSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm4.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ResearchLink);
            WaitForPageLoad(20);
            if (language.ToString() == "EN")
                Assert.IsTrue(hubpages.GetH1TagText().Contains(year + " " + make + " " + model), "Research more link is not navigating to Ymm hubpage");
            else
                Assert.IsTrue(hubpages.GetH1TagText().Contains(make + " " + model + " " + year), "Research more link is not navigating to Ymm hubpage");



        }
        [Test]
        public void VerifyByClickingOnYearLink()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm5.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm5.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm5.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(2, SEOLinks.SeoYearsLinks);
            WaitForPageLoad(20);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + model), "Year link is not redirecting to Hub page ");
        }
        [Test]
        public void VerifyYearsLinkByClickingOnShowMoreYearsOnYMMWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm5.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm5.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm5.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickShowMoreYearsLink();
            srp.ClickOnLinkFromList(7, SEOLinks.SeoYearsLinks);
            WaitForPageLoad(20);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + model), "Year link is not redirecting to Hub page ");
        }

        [Test]
        public void VerifyByClickingOnInventoryLinkYMMSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm4.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(0, SEOLinks.MakeModelnames);
            srp.ClickOnLinkFromList(0, SEOLinks.ViewInventoryLinks);
            hubpages.WaitForPageLoad(mmName.ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }
        [Test]
        public void VerifyByClickingOnCompareLinkYMMSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm4.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(1, SEOLinks.CompareLinks);
            WaitForPageLoad(30);
            if (language.ToString() == "EN")
                Assert.True(hubpages.GetH1TagText().Contains(year + " " + model));
            else
                Assert.True(hubpages.GetH1TagText().Contains(model + " " + year));
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkYMMPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            WaitForPageLoad(20);
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllYMMPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm7.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm7.model");
            string year = GetTestData(testDataFile, "YearMakeModelData.ymm7.year");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/" + year);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {
                srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
                hubpages.WaitForPageLoad("editorial");
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                    Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
                });
            }

        }
        #endregion
        #region MM SRP


        [Test]
        public void VerifySEOwidgetDisplayingOnMMSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");


            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            Assert.IsTrue(srp.IsSEOWidgetDisplayingOnSRPPage(SRPLocators.SEOLinks.SEOWidget), "Widget is not showing");

        }
        [Test]
        public void VerifyIntroDescForMMSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.MM") : GetTestData(testDataFile, "IntroDescFr.MM");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");


            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc);
        }
        [Test]
        public void VerifyResearchMoreLinkNaviagtionOnMMpage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");

            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ResearchLink);
            WaitForPageLoad(20);

            Assert.IsTrue(hubpages.GetH1TagText().Contains(make + " " + model), "Research more link is not navigating to Ymm hubpage");



        }

        [Test]
        public void VerifyByClickingOnYearLinkMMPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm4.model");

            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(2, SEOLinks.SeoYearsLinks);
            WaitForPageLoad(20);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + model), "Year link is not redirecting to Hub page ");
        }
        [Test]
        public void VerifyYearsLinkByClickingOnShowMoreYearsOnMMWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm5.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm5.model");

            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickShowMoreYearsLink();
            srp.ClickOnLinkFromList(7, SEOLinks.SeoYearsLinks);
            WaitForPageLoad(20);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + model), "Year link is not redirecting to Hub page ");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsOnMMPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm.model");

            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllLinkMMPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm.make");
            string model = GetTestData(testDataFile, "YearMakeModelData.ymm.model");
            #endregion
            url = new Uri(srpUrl + make + "/" + model + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            hubpages.WaitForPageLoad("editorial");
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });

        }

        #endregion

        #region Make SRP SEO
        [Test]
        public void VerifySEOwidgetDisplayingOnMakeSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");


            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            Assert.IsTrue(srp.IsSEOWidgetDisplayingOnSRPPage(SRPLocators.SEOLinks.SEOWidget), "Widget is not showing");

        }
        [Test]
        public void VerifyIntroDescForMakeSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.Make") : GetTestData(testDataFile, "IntroDescFr.Make");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");

            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc);
        }
        [Test]
        public void VerifyResearchMoreLinkNaviagtionOnMakepage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");

            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.ResearchLink);
            WaitForPageLoad(20);
            Assert.IsTrue(hubpages.GetH1TagText().Contains(make), "Research more link is not navigating to Make hubpage");

        }
        [Test]
        public void VerifyByClickingOnExpertReviewsOnMakePage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");


            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }
        [Test]
        public void VerifyByClickingCoupeModelViewInventoryLink()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm1.make");
            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(3, SEOLinks.DiscoverModelTabs);
            string ModelName = srp.GetElementFromListSEO(1, SEOLinks.CoupeModels);
            srp.ClickOnLinkFromList(1, SEOLinks.CoupeViewInventoryLinks);
            // WaitForPageLoad(60);
            hubpages.WaitForPageLoad(ModelName);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + ModelName));

        }
        [Test]
        public void VerifyByClickingSedanModelViewInventoryLink()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm6.make");

            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(1, SEOLinks.DiscoverModelTabs);
            string ModelName = srp.GetElementFromListSEO(1, SEOLinks.SedanModels);
            srp.ClickOnLinkFromList(1, SEOLinks.SedanViewInventoryLinks);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + ModelName));

        }
        [Test]
        public void VerifyByClickingHatchbackModelLearnMoreLink()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm2.make");

            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(4, SEOLinks.DiscoverModelTabs);
            string ModelName = srp.GetElementFromListSEO(1, SEOLinks.HatchbackModels);
            srp.ClickOnLinkFromList(1, SEOLinks.HatchbackLearnMoreLinks);
            WaitForPageLoad(30);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + ModelName));

        }
        [Test]
        public void VerifyByClickingShowMoreSUVsLink()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            string make = GetTestData(testDataFile, "YearMakeModelData.ymm2.make");


            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickOnLinkFromList(0, SEOLinks.DiscoverModelTabs);
            string ModelName = srp.GetElementFromListSEO(7, SEOLinks.SUVModels);
            srp.ClickSEOWidgetLink(SEOLinks.ShowMoreSUVsLink);
            srp.ClickOnLinkFromList(7, SEOLinks.SUVLearnMoreLinks);
            Assert.True(hubpages.GetH1TagText().Contains(make + " " + ModelName));

        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllLinkMakePage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            string make = GetTestData(testDataFile, "YearMakeModelData.ymm4.make");

            #endregion
            url = new Uri(srpUrl + make + "/");
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
            });

        }

        #endregion

        #region General SRPs


        [Test]
        public void VerifySEOwidgetDisplayingOnGeneralSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
            Assert.IsTrue(srp.IsSEOWidgetDisplayingOnSRPPage(SRPLocators.SEOLinks.SEOWidget), "Widget is not showing");

        }
        [Test]
        public void VerifyIntroDescForGeneralSRP()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralSRP");

            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), IntroDesc, "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsArticleLinkGeneralPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");

            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string articleTitle = srp.GetElementFromListSEO(0, SEOLinks.ArticleTitles);
            srp.ClickOnLinkFromList(0, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");
            Assert.True(hubpages.GetH1TagText().Equals(articleTitle));
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllGeneralPage()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.advicePage") : baseURL + GetTestData(testDataFile, "urlsFr.advicePage");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.advicePageEn") : GetTestData(testDataFile, "pageTitle.advicePageFr");
            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnInventoryLinkGeneralSRPSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName));

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkGeneralSEOWidget()
        {
            #region Variables

            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            #endregion
            url = new Uri(srpUrl);
            Open();
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName));


        }

        #endregion

        #region BOdy type SRPs
        [Test]
        public void VerifyIntroDescForSUVSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllSUVPage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnExpertReviewsOnSUVPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkSUVSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkSUVSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");

        }
        [Test]
        public void VerifyDiscoverModelsSUVSEOWidget()
        {
            string[] mmNames = { "Ford Escape", "Honda CR-V", "Hyundai Kona", "Mazda CX-5", "Nissan Rogue", "Toyota RAV4" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }
        [Test]
        public void VerifyIntroDescForSedanSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "berline";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllSedanPage()
        {
            #region Variables

            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnExpertReviewsOnSedanPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkSedanSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkSedanSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");

        }
        [Test]
        public void VerifyDiscoverModelsSedanSEOWidget()
        {
            string[] mmNames = { "BMW 3 Series", "Honda Civic", "Hyundai Elantra", "Toyota Camry", "Toyota Corolla", "Volkswagen Jetta" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }
        [Test]
        public void VerifyIntroDescForMinivanSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "mini-fourgonnette";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllMinivanPage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {
                srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
                WaitForPageLoad(30);
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                    Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
                });
            }

        }
        [Test]
        public void VerifyByClickingOnExpertReviewsOnMinivanPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {

                string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

                srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
                hubpages.WaitForPageLoad("editorial");

                Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
            }
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkMinivanSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkMinivanSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");
        }
        [Test]
        public void VerifyDiscoverModelsMinivanSEOWidget()
        {
            string[] mmNames = { "Chrysler Grand Caravan", "Chrysler Pacifica", "Dodge Grand Caravan", "Honda Odyssey", "Kia Carnival", "Toyota Sienna" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyIntroDescForTruckSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Truck" : "camion";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllTruckPage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnExpertReviewsOnTruckPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkTruckSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkTruckSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");
        }
        [Test]
        public void VerifyDiscoverModelsTruckSEOWidget()
        {
            string[] mmNames = { "Chevrolet Silverado 1500", "Ford F-150", "Ford Ranger", "GMC Sierra 1500", "Ram 1500", "Toyota Tacoma" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }

        [Test]
        public void VerifyIntroDescForCoupeSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "coupés";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllCoupePage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {
                srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
                WaitForPageLoad(30);
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                    Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
                });
            }

        }
        [Test]
        public void VerifyByClickingOnExpertReviewsOnCoupePage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkCoupeSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkCoupeSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");
        }
        [Test]
        public void VerifyDiscoverModelsCoupeSEOWidget()
        {
            string[] mmNames = { "BMW 2 Series", "Chevrolet Camaro", "Chevrolet Corvette", "Ford Mustang", "Porsche 911", "Subaru BRZ" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }
        [Test]
        public void VerifyIntroDescForConvertibleSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "décapotable";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllConvertiblePage()
        {
            #region Variables
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnExpertReviewsOnConvertiblePage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkConvertibleSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkConvertibleSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");

        }
        [Test]
        public void VerifyDiscoverModelsConvertibleSEOWidget()
        {
            string[] mmNames = { "BMW Z4", "Chevrolet Corvette", "Ford Mustang", "Mazda MX-5", "Porsche 718 Boxster", "Porsche 911" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }
        [Test]
        public void VerifyIntroDescForHatchbackSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "hayon";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllHatchbackPage()
        {
            #region Variables
             String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
             String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
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
        public void VerifyByClickingOnExpertReviewsOnHatchbackPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

            srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
            hubpages.WaitForPageLoad("editorial");

            Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkHatchbackSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkHatchbackSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");


        }
        [Test]
        public void VerifyDiscoverModelsHatchbackSEOWidget()
        {
            string[] mmNames = { "Chevrolet Spark", "Honda Civic Hatchback", "Kia Rio 5-door", "Mazda Mazda3", "Toyota Corolla Hatchback", "Volkswagen Golf" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }
        [Test]
        public void VerifyIntroDescForWagonSRP()
        {
            #region Variables

            string IntroDesc = (language.ToString() == "EN") ? GetTestData(testDataFile, "IntroDescEn.GeneralBodytypeSRP") : GetTestData(testDataFile, "IntroDescFr.GeneralBodytypeSRP");
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "familiale";

            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            Assert.AreEqual(srp.GetIntroDescSEOWidget(), String.Format(IntroDesc, bodyType), "Description is wrong");
        }
        [Test]
        public void VerifyByClickingOnExpertReviewsSeeAllWagonPage()
        {
            #region Variables
            String PageTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "pageTitle.searchEn") : GetTestData(testDataFile, "pageTitle.searchFr");
            String CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.search") : baseURL + GetTestData(testDataFile, "urlsFr.search");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {
                srp.ClickSEOWidgetLink(SEOLinks.EditorialSeeAllLink);
                WaitForPageLoad(30);
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(srp.IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
                    Assert.AreEqual(PageTitle, srp.GetPageTitle(), "Page Title is not correct");
                });
            }

        }
        [Test]
        public void VerifyByClickingOnExpertReviewsOnWagonPage()
        {

            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            if (language.ToString() == "EN")
            {
                string articleTitle = srp.GetElementFromListSEO(1, SEOLinks.ArticleTitles);

                srp.ClickOnLinkFromList(1, SEOLinks.ArticleTitles);
                hubpages.WaitForPageLoad("editorial");

                Assert.True(hubpages.GetH1TagText().Equals(articleTitle), "Article is not working");
            }
        }
        [Test]
        public void VerifyByClickingOnInventoryLinkWagonSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            string[] words = mmName.Split(' ');
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPViewInventoryLink);
            hubpages.WaitForPageLoad(words[0].ToLower());
            Assert.True(hubpages.GetH1TagText().Contains(mmName), "Inventory link is not redirecting to hub page");

        }
        [Test]
        public void VerifyByClickingOnLearnMoreLinkWagonSEOWidget()
        {
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);

            string mmName = srp.GetElementFromListSEO(1, SEOLinks.GeneralSRPMMName);
            srp.ClickOnLinkFromList(1, SEOLinks.GeneralSRPLearnMoreLink);

            Assert.True(hubpages.GetH1TagText().Equals(mmName), "Learn more link is not working");


        }
        [Test]
        public void VerifyDiscoverModelsWagonSEOWidget()
        {
            string[] mmNames = { "Chevrolet Silverado 1500", "Ford F-150", "Ford Ranger", "GMC Sierra 1500", "Ram 1500", "Toyota Tacoma" };
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            srp.CloseCookieBanner();
            srp.ClickSEOWidgetLink(SEOLinks.ExpandAllLink);
            string[] mmName = srp.GetDiscoverModelsSRPSEOWidgets(SEOLinks.DiscoverModelNames);

            for (int i = 0; i < mmNames.Length; i++)
            {
                Assert.AreEqual(mmNames[i], mmName[i], "Models names mismatch");
            }

        }

        #endregion
    }
}
