using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UIAutomationLibrary.Locators;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace UIAutomationLibrary.Pages.HubPages
{
    public abstract class HubPagesAbstract : Page
    {
        private const string _defaultValue = null;
        protected HubPagesAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public abstract void ClickOnReviewsAndAdvice();
        public void ClickOnVehicleResearchLinkUnderReviewsAndAdvice()
        {
            string ResearchLinkLocator = (language.ToString() == "EN") ? (Locators.HubPagesLocators.ResearchPage.VehicleResearchLink.GetAttribute<DescriptionAttribute>().Description) : (Locators.HubPagesLocators.ResearchPage.VehicleResearchLink.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench);
            ClickElement(FindElement(By.LinkText(ResearchLinkLocator)));
        }

        public string GetCanonicalUrl()
        {
            return driver.FindElement(By.CssSelector(ResearchPage.CanonicalUrl.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("href");
        }
        public void ClickHomeBreadCrumb()
        {
            string locator = (language.ToString() == "EN") ? (ResearchPage.HomeBreadCrumb.GetAttribute<DescriptionAttribute>().Description) : (ResearchPage.HomeBreadCrumb.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench);
            ClickElement(FindElement(By.LinkText(locator)));
        }
        public void NavigateToYMMPageUsingDropDown(string make, string model, string year, string urlMake, string urlModel)
        {
            if (urlModel != _defaultValue)
            {
               
                SelectByText(By.CssSelector(ResearchPage.MakeDropDown.GetAttribute<DescriptionMakeModelAttribute>().DescriptionMakeModel), make);
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.ModelDropDown.GetAttribute<DescriptionMakeModelAttribute>().DescriptionMakeModel), model);
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.YearDropDown.GetAttribute<DescriptionMakeModelAttribute>().DescriptionMakeModel), year);
                ClickElement(FindElement(By.CssSelector(ResearchPage.SearchBtn.GetAttribute<DescriptionMakeModelAttribute>().DescriptionMakeModel)));
                Wait(3);
            }
            else if (urlMake == _defaultValue)
            {
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.MakeDropDown.GetAttribute<DescriptionAttribute>().Description), make);
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.ModelDropDown.GetAttribute<DescriptionAttribute>().Description), model);
                Wait(3);
                SelectByValue(By.CssSelector(ResearchPage.YearDropDown.GetAttribute<DescriptionAttribute>().Description), year);
                ClickElement(FindElement(By.CssSelector(ResearchPage.SearchBtn.GetAttribute<DescriptionAttribute>().Description)));
                Wait(3);
            }
            else
            {
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.MakeDropDown.GetAttribute<DescriptionMakeAttribute>().DescriptionMake), make);
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.ModelDropDown.GetAttribute<DescriptionMakeAttribute>().DescriptionMake), model);
                Wait(3);
                SelectByText(By.CssSelector(ResearchPage.YearDropDown.GetAttribute<DescriptionMakeAttribute>().DescriptionMake), year);
                ClickElement(FindElement(By.CssSelector(ResearchPage.SearchBtn.GetAttribute<DescriptionMakeAttribute>().DescriptionMake)));
                Wait(3);
            }


        }
        public void AddVehiclesInComparisonTool(String make1, String model1, String year1, String make2, String model2, String year2)
        {

            SelectByText(By.CssSelector(ResearchPage.Make1CompareTool.GetAttribute<DescriptionAttribute>().Description), make1);
            Wait(3);
            SelectByText(By.CssSelector(ResearchPage.Model1CompareTool.GetAttribute<DescriptionAttribute>().Description), model1);
            Wait(3);
            SelectByText(By.CssSelector(ResearchPage.Year1CompareTool.GetAttribute<DescriptionAttribute>().Description), year1);
            Wait(3);
            SelectByText(By.CssSelector(ResearchPage.Make2CompareTool.GetAttribute<DescriptionAttribute>().Description), make2);
            Wait(3);
            SelectByText(By.CssSelector(ResearchPage.Model2CompareTool.GetAttribute<DescriptionAttribute>().Description), model2);
            Wait(3);
            SelectByText(By.CssSelector(ResearchPage.Year2CompareTool.GetAttribute<DescriptionAttribute>().Description), year2);


            ClickElement(FindElement(By.CssSelector(ResearchPage.CompareBtn.GetAttribute<DescriptionAttribute>().Description)));



        }
        public string GetH1TagText()
        {
            return GetElementText(FindElement(By.TagName(ResearchPage.H1Tag.GetAttribute<DescriptionAttribute>().Description)));
        }
        public void WaitForPageLoad(String urlContains)
        {
            WaitUntil(() => ExpectedConditions.UrlContains(urlContains));
        }
        public void WaitUntilAttributeValueEquals(IWebElement webElement, String attributeName, String attributeValue)
        {

            WaitUntil(() => webElement.GetAttribute(attributeName).Contains(attributeValue));

        }

        public bool NavigateToSectionUsingStickyNav(StickyNavs tab)
        {

            ScrollCount(6);
            ClickElementJS(tab.GetAttribute<DescriptionAttribute>().Description);
            WaitUntilAttributeValueEquals(FindElement(By.CssSelector(tab.GetAttribute<DescriptionAttribute>().Description)), "class", "nav-item active");
            String redHighLight = FindElement(By.CssSelector(tab.GetAttribute<DescriptionAttribute>().Description)).GetCssValue("border-bottom-color");

            return redHighLight.Equals("rgba(199, 2, 0, 1)");

        }
        public void NavigateToMakePageUsingResearchByMakeLogo()
        {
            ClickElement(FindElement(By.LinkText(ResearchPage.ResearchByMakeLink.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(60);
        }
        public IList<IWebElement> GetElementsByJS(String query)
        {
            return (IList<IWebElement>)(((IJavaScriptExecutor)driver).ExecuteScript(query));
        }
        public bool CheckImageIsloaded(IWebElement imageWebElement)
        {

            return (Boolean)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", imageWebElement);
        }
        public bool CheckIfAllImagesAreLoaded(ResearchPage selector, string make = _defaultValue)
        {
            string query;
            if (make == _defaultValue)
            {
                query = $"return document.getElementById('{selector.GetAttribute<DescriptionAttribute>().Description}').querySelectorAll('.hubHome__vehicles__carouselItemImage__image')";
            }
            else
            {
                query = $"return document.getElementById('{selector.GetAttribute<DescriptionMakeAttribute>().DescriptionMake}').querySelectorAll('.hubMake__vehicles__carouselItemImage')";

            }
            IList<IWebElement> imageElements = GetElementsByJS(query);
            int count = imageElements.Count;

            for (int i = 0; i < count; i++)
            {
                if (!CheckImageIsloaded(imageElements[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public void ClickOnViewDetailsBtn(ResearchPage locator, string make = _defaultValue)
        {
            if (make == _defaultValue)
                ClickElement(FindElement(By.Id(locator.GetAttribute<DescriptionAttribute>().Description)).FindElement(By.CssSelector(ResearchPage.ViewDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
            else
                ClickElement(FindElement(By.Id(locator.GetAttribute<DescriptionMakeAttribute>().DescriptionMake)).FindElement(By.CssSelector(ResearchPage.ViewDetailsBtn.GetAttribute<DescriptionMakeAttribute>().DescriptionMake)));
            WaitForPageLoad(60);
        }
        public bool AreHeroScoresPresentOnYMMPage()
        {
            return IsElementVisible(By.CssSelector(YearMakeModelPage.HeroScores.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickOnViewAllArticleBtn()
        {

            ClickElement(FindElement(By.CssSelector(ResearchPage.ViewAllArticlesBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad("editorial");
        }
        public bool CheckIfAllImagesAreLoadedForArticleWidgetResearchPage(ResearchPage selector)
        {

            IList<IWebElement> Images = FindElements(By.CssSelector(selector.GetAttribute<DescriptionAttribute>().Description));
            int count = Images.Count;

            for (int i = 0; i < count; i++)
            {
                if (!CheckImageIsloaded(Images[i]))
                {

                    return false;
                }

            }
            return true;
        }

        public void NavigateToArticlePage(ResearchPage link)
        {
            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad("editorial");
        }

        public void ClickModelLinkOnMakeModelWidget()
        {
            ClickElement(FindElement(By.CssSelector(MakePage.MakeModelLink.GetAttribute<DescriptionAttribute>().Description)));
        }
        public void ClickViewAllDealsBtn()
        {
            ClickElement(FindElement(By.CssSelector(MakePage.ViewAllDealsBtn.GetAttribute<DescriptionAttribute>().Description)));

        }
        public void ClickTransportCanadaLink()
        {
            ClickElement(FindElement(By.CssSelector(MakeModelPage.TransportCanadaLink.GetAttribute<DescriptionAttribute>().Description)));

        }
        public void ClickOwnerReviewsLink()
        {
            ClickElement(FindElement(By.CssSelector(MakeModelPage.ViewAllOwnerReviews.GetAttribute<DescriptionAttribute>().Description)));

        }
        public bool IsModelOverviewWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.ModelOverview.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool AreExploreLinksDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.ExploreLinks.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsPhotoWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.PhotosSection.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsReviewsNewsWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.ReviewsNewsWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsOwnerScoresWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.OwnerScoresWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsRecallInfoWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.RecallInfoWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsInventoryWidgetDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.InventoryWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsVehicleResearchDropDownDisplaying()
        {
            return IsElementVisible(By.CssSelector(MakeModelPage.VehicleResearchSection.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickExploreMakeModelYearLink()
        {
            ClickElement(FindElement(By.CssSelector(MakeModelPage.ExploreLink.GetAttribute<DescriptionAttribute>().Description)));

        }
        public string GetOwnerReviewsHeader()
        {
            return GetElementText(FindElement(By.CssSelector(MakeModelPage.OwnerReviewsPageHeader.GetAttribute<DescriptionAttribute>().Description)));
        }
        public string GetSRPHeader()
        {
            return GetElementText(FindElement(By.CssSelector(MakeModelPage.SrpHeader.GetAttribute<DescriptionAttribute>().Description)));
        }
        public bool AreYMMWidgetsDisplaying(YearMakeModelPage Widget)
        {
            return IsElementVisible(By.CssSelector(Widget.GetAttribute<DescriptionAttribute>().Description));
        }

        public string GetATHeroScore()
        { return GetElementAttribute(By.CssSelector(YearMakeModelPage.HeroATScore.GetAttribute<DescriptionAttribute>().Description), "data-value"); }
        public string GetATBadgeScore()
        { return GetElementText(FindElement(By.CssSelector(YearMakeModelPage.ATBadgeScore.GetAttribute<DescriptionAttribute>().Description))); }
        public void ClickATScoreReadMoreLink()
        {
            ClickElement(FindElement(By.CssSelector(YearMakeModelPage.ATScoreReadMoreLink.GetAttribute<DescriptionAttribute>().Description)));
        }
        public string GetInventoryTitle()
        {
            ScrollTo(FindElement(By.CssSelector(YearMakeModelPage.InventoryWidgetTitle.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(YearMakeModelPage.InventoryTitles.GetAttribute<DescriptionAttribute>().Description));
            return GetElementText(FindElement(By.CssSelector(YearMakeModelPage.InventoryTitles.GetAttribute<DescriptionAttribute>().Description)));

        }


        public void ClickCityTabInventoryWidget(YearMakeModelPage tab)

        {
            ClickElement(FindElement(By.CssSelector(tab.GetAttribute<DescriptionAttribute>().Description)));
            
        }

        public string GetInventoryCity()
        {
            ScrollTo(FindElement(By.CssSelector(YearMakeModelPage.InventoryWidgetTitle.GetAttribute<DescriptionAttribute>().Description)));
            WaitUntilAttributeValueEquals(FindElement(By.CssSelector(YearMakeModelPage.CalgaryTab.GetAttribute<DescriptionAttribute>().Description)), "class", "active");
            return GetElementText(FindElement(By.CssSelector(YearMakeModelPage.InventoryCity.GetAttribute<DescriptionAttribute>().Description)));

        }
        public string[] GetArticleTitles(int count)
        {
            string[] articleTitles = new string[count];
            for (var j = 0; j < count; j++)
            {
                string jsQuery = "return document.querySelectorAll('"+YearMakeModelPage.ArticleTitles.GetAttribute<DescriptionAttribute>().Description+"')[" + j + "].innerText;";
                string articleTitle = GetValueByJS(jsQuery);
                articleTitles[j] = articleTitle;

            }
            return articleTitles;
        }
        public int GetCountOfArticles()
        {
            return FindElements(By.CssSelector(YearMakeModelPage.ReviewsNewsArticles.GetAttribute<DescriptionAttribute>().Description)).Count;
        }
        public int GetCountOfDistinctArticles(string[] articles)

        {
            HashSet<string> s = new HashSet<string>(articles);
            return s.Count;
        }
        public ArrayList GetArticlesPublishedDates(int count)
        {
            ArrayList dates = new ArrayList();
            for (int i = 0; i < count; i++) 
            {
                string jsQuery = "return document.querySelectorAll('"+ YearMakeModelPage.ArticleDates.GetAttribute<DescriptionAttribute>().Description +" ')[" + i + "].innerText;";
                string pubDate = GetValueByJS(jsQuery);

                dates.Add(pubDate);
            }
            return dates;
        }
        public bool AreArticleInChronologicalOrder(ArrayList dates)
        {


            List<DateTime> list = new List<DateTime>();

            foreach (string articleDate in dates)
            {
                if (language.ToString() == "FR")
                {
                    var format = "dd MMM yyyy";

                    DateTime parsedDate = DateTime.ParseExact(articleDate, format, new CultureInfo("fr-CA"));
                    list.Add(parsedDate);
                }
                else
                {
                    DateTime parsedDate = DateTime.Parse(articleDate);
                    list.Add(parsedDate);
                }
            }
            for (int i = 1; i < list.Count; i++)
            {
                int v = DateTime.Compare(list[i - 1], list[i]);
                if (v < 0)
                {
                    return false;
                }
            }

            return true;

        }
      
    }
}
