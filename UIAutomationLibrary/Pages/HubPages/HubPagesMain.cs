using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace UIAutomationLibrary.Pages.HubPages
{
    public class HubPagesMain : Page
    {
        private const string _defaultValue = null;
        HubPagesAbstract hp;
        public HubPagesMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    hp = new HubPagesLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    hp = new HubPagesXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    hp = new HubPagesSmall(driver, viewport, language);
                    break;
            }

        }
        public string researchPageURL(Language language, dynamic testDataFile)
        {
            return (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.researchPage") : GetTestData(testDataFile, "urlsFr.researchPage");
        }
        public void ClickOnReviewsAndAdvice()
        {
            hp.ClickOnReviewsAndAdvice();
        }
        public void ClickOnVehicleResearchLinkUnderReviewsAndAdvice()
        {
            hp.ClickOnVehicleResearchLinkUnderReviewsAndAdvice();
        }
        public string GetCanonicalUrl()
        {
            return hp.GetCanonicalUrl();
        }
        public void ClickHomeBreadCrumb()
        {
            hp.ClickHomeBreadCrumb();
        }
        public void NavigateToYMMPageUsingDropDown(string make, string model, string year, string urlMake=_defaultValue,string urlModel=_defaultValue)
        {
            hp.NavigateToYMMPageUsingDropDown(make, model, year,urlMake,urlModel );

        }
        public void AddVehiclesInComparisonTool(String make1, String model1, String year1, String make2, String model2, String year2)
        {


            hp.AddVehiclesInComparisonTool(make1, model1, year1, make2, model2, year2);
        }
        public string GetVehicleTitle(int i)
        {
            IList<IWebElement> VehicleNames =driver.FindElements(By.CssSelector(ComparisonPage.FirstVehicleTitle.GetAttribute<DescriptionAttribute>().Description));
            return VehicleNames[i].Text;
        }
        public string GetH1TagText()
        {
            return hp.GetH1TagText();
        }
        public void WaitForPageLoad(String urlContains)
        {
            WaitUntil(() => ExpectedConditions.UrlContains(urlContains));
        }
        public void WaitUntilAttributeValueEquals(IWebElement webElement, String attributeName, String attributeValue)
        {
            hp.WaitUntilAttributeValueEquals(webElement, attributeName, attributeValue);


        }

        public bool NavigateToSectionUsingStickyNav(StickyNavs tab)
        {

            return hp.NavigateToSectionUsingStickyNav(tab);

        }
        public void NavigateToMakePageUsingResearchByMakeLogo()
        {
            hp.NavigateToMakePageUsingResearchByMakeLogo();
        }
        public IList<IWebElement> GetElementsByJS(String query)
        {
            return GetElementsByJS(query);
        }
        public bool CheckImageIsloaded(IWebElement imageWebElement)
        {
            return hp.CheckImageIsloaded(imageWebElement);
        }
        public bool CheckIfAllImagesAreLoaded(ResearchPage selector, string make = _defaultValue)
        {
            return hp.CheckIfAllImagesAreLoaded(selector, make);
        }
        public void ClickOnViewDetailsBtn(ResearchPage locator, string make = _defaultValue)
        {
            hp.ClickOnViewDetailsBtn(locator, make);
        }
        public bool AreHeroScoresPresentOnYMMPage()
        {
            return hp.AreHeroScoresPresentOnYMMPage();
        }
        public void ClickOnViewAllArticleBtn()
        {

            hp.ClickOnViewAllArticleBtn();
        }
        public bool CheckIfAllImagesAreLoadedForArticleWidgetResearchPage(ResearchPage selector)
        {

            return hp.CheckIfAllImagesAreLoadedForArticleWidgetResearchPage(selector);
        }

        public void NavigateToArticlePage(ResearchPage link)
        {
            hp.NavigateToArticlePage(link);
        }
      
        public void ClickModelLinkOnMakeModelWidget()
        {
            hp.ClickModelLinkOnMakeModelWidget();

        }
        public void ClickViewAllDealsBtn()
        {
            hp.ClickViewAllDealsBtn();

        }
        public void ClickTransportCanadaLink()
        {
            hp.ClickTransportCanadaLink();

        }
        public void ClickOwnerReviewsLink()
        {
            hp.ClickOwnerReviewsLink();
        }
        public bool IsModelOverviewWidgetDisplaying()
        {
           return hp.IsModelOverviewWidgetDisplaying();
        }
        public bool AreExploreLinksDisplaying()
        {
            return hp.AreExploreLinksDisplaying();
        }
        public bool IsPhotoWidgetDisplaying()
        {
            return hp.IsPhotoWidgetDisplaying();
        }
        public bool IsReviewsNewsWidgetDisplaying()
        {
           return hp.IsReviewsNewsWidgetDisplaying();
        }
        public bool IsOwnerScoresWidgetDisplaying()
        {
            return hp.IsOwnerScoresWidgetDisplaying();
        }
        public bool IsRecallInfoWidgetDisplaying()
        {
            return hp.IsRecallInfoWidgetDisplaying();
        }
        public bool IsInventoryWidgetDisplaying()
        {
            return hp.IsInventoryWidgetDisplaying();
        }
        public bool IsVehicleResearchDropDownDisplaying()
        {
            return hp.IsVehicleResearchDropDownDisplaying();
        }
        public void ClickExploreMakeModelYearLink()
        {
            hp.ClickExploreMakeModelYearLink();
        }
        public string GetOwnerReviewsHeader()
        {
            return hp.GetOwnerReviewsHeader();
        }
        public string GetSRPHeader()
        {
            return hp.GetSRPHeader();
        }
        public bool AreYMMWidgetsDisplaying(YearMakeModelPage Widget)
        {
            return hp.AreYMMWidgetsDisplaying(Widget);
        }
      
        public string GetATHeroScore()
        { return hp.GetATHeroScore(); ; }
        public string GetATBadgeScore()
        { return hp.GetATBadgeScore(); }
        public void ClickATScoreReadMoreLink()
        {
            hp.ClickATScoreReadMoreLink();
        }
        public string GetInventoryTitle()
        {
            return hp.GetInventoryTitle();

        }
      
        public void ClickCityTabInventoryWidget(YearMakeModelPage tab)

        {
            hp.ClickCityTabInventoryWidget(tab);
        }
        public string GetInventoryCity()
        {
            return hp.GetInventoryCity();

        }

        public int GetCountOfArticles()
        {
            return hp.GetCountOfArticles();
        }
        public ArrayList GetArticlesPublishedDates(int count)
        {
            return hp.GetArticlesPublishedDates(count);
        }
        public bool AreArticleInChronologicalOrder(ArrayList dates)
        {
            return hp.AreArticleInChronologicalOrder(dates);

        }
        public int GetCountOfDistinctArticles(string[] articles)

        {
            return hp.GetCountOfDistinctArticles(articles);
        }
        public string[] GetArticleTitles(int count)
        {
            return hp.GetArticleTitles(count);
        }
       
    }
}
