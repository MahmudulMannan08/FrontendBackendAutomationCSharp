using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static UIAutomationLibrary.Locators.EditorialLocators;

namespace UIAutomationLibrary.Pages.Editorials
{
   
    public class EditorialMain : Page
    {
        EditorialsAbstract ePage;
       
        public EditorialMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    ePage = new EditorialLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    ePage = new EditorialsXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    ePage = new EditorialsSmall(driver, viewport, language);
                    break;
            }

        }
        public string editorialURL(Language language, dynamic testDataFile)
        {
            return (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.editorialHomePage") : GetTestData(testDataFile, "urlsFr.editorialHomePage");
        }
        public void ClickOnReviewsAndAdvice()
        {
            ePage.ClickOnReviewsAndAdvice();
        }
        public void ClickOnEitorialLinkUnderReviewsAndAdvice()
        {
            ePage.ClickOnEitorialLinkUnderReviewsAndAdvice();
        }
        public void ClickOnSecondarynavLinks(SecondaryNav link)
        {

            ePage.ClickOnSecondarynavLinks(link);

        }
        public void EnterValueInSearchField(string make)
        {
            ePage.EnterValueInSearchField(make);
        }
        public void NavigateToCategoryPage(TertiaryHeaderLinks link)
        {
            ePage.NavigateToCategoryPage(link);

        }
        public string GetActiveLinkText()
        {
            return ePage.GetActiveLinkText();

        }
        public string GetActiveSecondarynav()
        {
            return ePage.GetActiveSecondarynav();
        }
        public bool NavigateToArticleLink(EditorialWidgets link)
        {

            return ePage.NavigateToArticleLink(link);

        }
        public void NavigateToReviewsResultsPage(string make, string model)
        {
            ePage.NavigateToReviewsResultsPage(make, model);
        }
        public List<IWebElement> GetSeeAllLinks()
        {
            return ePage.GetSeeAllLinks();
        }
        public bool CheckIfLoadMoreResultsWorks()
        {

            return ePage.CheckIfLoadMoreResultsWorks();

        }
        public string GetCanonicalUrl()
        {
            return ePage.GetCanonicalUrl();
        }
        public bool IsArticleSummaryAvailable()
        {
            return ePage.IsArticleSummaryAvailable();
        }
        public string GetAwardsPageH1Title()
        {
            return ePage.GetAwardsPageH1Title();
        }
        public void ClickOnVideoArticleATV(AutotraderTV link)
        { ePage.ClickOnVideoArticleATV(link); }
        public string GetVideoTitleOnATVPage(AutotraderTV name)
        { return ePage.GetVideoTitleOnATVPage(name); }
        public string GetH1TitleATV(AutotraderTV text)
        { return ePage.GetH1TitleATV(text); }
        public string GetPlayListTitleOnATVPage(AutotraderTV name)
        { return ePage.GetPlayListTitle(name); }
        public void ClickPlaylistTitle(AutotraderTV link, int n)
        { ePage.ClickPlaylistTitle(link, n); }
        public bool IsArticleWidgetDisplaying(ArticlePage widget)
        {
            return ePage.IsArticleWidgetDisplaying(widget);
        }
        public bool NavigateToCoolStuffMainArticle()
        {
            return ePage.NavigateToCoolStuffMainArticle();
        }
    }
}
