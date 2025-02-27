using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static UIAutomationLibrary.Locators.EditorialLocators;
using DescriptionFrenchAttribute = UIAutomationLibrary.Locators.EditorialLocators.DescriptionFrenchAttribute;

namespace UIAutomationLibrary.Pages.Editorials
{
    public abstract class EditorialsAbstract : Page
    {
        protected EditorialsAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }
      
        public abstract void ClickOnReviewsAndAdvice();
        public void ClickOnEitorialLinkUnderReviewsAndAdvice()
        {
            string EdioriallinkLocator = (language.ToString() == "EN") ? Locators.EditorialLocators.SecondaryNav.EditorialLink.GetAttribute<DescriptionAttribute>().Description : (Locators.EditorialLocators.SecondaryNav.EditorialLink.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench);
            ClickElement(FindElement(By.LinkText(EdioriallinkLocator)));
        }
        public void ClickOnSecondarynavLinks(SecondaryNav link)
        {

            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));

        }
      

        public void EnterValueInSearchField(string make)
        {
            EnterText(By.CssSelector(SecondaryNav.SearchLink.GetAttribute<DescriptionAttribute>().Description), make + "\n", 20);
        }
        public void NavigateToCategoryPage(TertiaryHeaderLinks link)
        {
            string locator = (language.ToString() == "EN") ? link.GetAttribute<DescriptionAttribute>().Description : link.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            ClickElement(FindElement(By.LinkText(locator)));
            WaitForPageLoad(60);


        }
        public string GetActiveLinkText()
        {
            IWebElement activeElement = FindElement(By.CssSelector(EditorialWidgets.TertiaryHeaderMenu.GetAttribute<DescriptionAttribute>().Description));
            return activeElement.Text;

        }
        public string GetActiveSecondarynav()
        {
            IWebElement activeElement = FindElement(By.CssSelector(EditorialWidgets.SecondaryNavActiveLink.GetAttribute<DescriptionAttribute>().Description));
            return activeElement.Text;
        }
        public bool NavigateToArticleLink(EditorialWidgets link)
        {

            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(60);
            WaitForElementVisible((By.CssSelector(ArticlePage.ArticleSummary.GetAttribute<DescriptionAttribute>().Description)));
            return true;

        }
        public bool NavigateToCoolStuffMainArticle()
        {
            String ArticleLink=(language.ToString() == "EN") ? Locators.EditorialLocators.EditorialWidgets.CoolStuffMainArticle.GetAttribute<DescriptionAttribute>().Description : (Locators.EditorialLocators.EditorialWidgets.CoolStuffMainArticle.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench);
            driver.FindElement(By.CssSelector(ArticleLink)).Click();
            WaitForPageLoad(60);
            WaitForElementVisible((By.CssSelector(ArticlePage.ArticleSummary.GetAttribute<DescriptionAttribute>().Description)));
            return true;
        }
        public void NavigateToReviewsResultsPage(string make, string model)
        {
            SelectByText((By.CssSelector(EditorialWidgets.FindReviewsMake.GetAttribute<DescriptionAttribute>().Description)), make);
            Wait(10);

            SelectByText((By.CssSelector(EditorialWidgets.FindReviewsModel.GetAttribute<DescriptionAttribute>().Description)), model);
            ClickElement(FindElement(By.CssSelector(EditorialWidgets.FindReviewsSearchBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(EditorialWidgets.FindReviewsResults.GetAttribute<DescriptionAttribute>().Description));
        }
        public List<IWebElement> GetSeeAllLinks()
        {
            return FindElements(By.CssSelector(EditorialWidgets.SeeAllLinks.GetAttribute<DescriptionAttribute>().Description)).ToList();
        }
        public bool CheckIfLoadMoreResultsWorks()
        {

            WaitForElementVisible((By.CssSelector(EditorialWidgets.LoadMoreBtn.GetAttribute<DescriptionAttribute>().Description)));
            ScrollTo(FindElement(By.CssSelector(EditorialWidgets.LoadMoreBtn.GetAttribute<DescriptionAttribute>().Description)));
            ClickElement(FindElement(By.CssSelector(EditorialWidgets.LoadMoreBtn.GetAttribute<DescriptionAttribute>().Description)));
            return true;

        }
        public string GetCanonicalUrl()
        {
            return FindElement(By.CssSelector(EditorialWidgets.CanonicalUrl.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("href");
        }
        public string GetH1TitleATV(AutotraderTV text)
        {
            return GetElementText(FindElement(By.CssSelector(text.GetAttribute<DescriptionAttribute>().Description)));
        }
        public bool IsArticleSummaryAvailable()
        {
            return IsElementVisible(By.CssSelector(ArticlePage.ArticleSummary.GetAttribute<DescriptionAttribute>().Description));
        }
        public string GetAwardsPageH1Title()
        {
            return GetElementText(FindElement(By.CssSelector(AwardsPage.AwardsPageH1Title.GetAttribute<DescriptionAttribute>().Description)));
        }
        public void ClickOnVideoArticleATV(AutotraderTV link)

        {
            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));

        }
        public string GetVideoTitleOnATVPage(AutotraderTV name)
        {
            return GetElementText(FindElement(By.CssSelector(name.GetAttribute<DescriptionAttribute>().Description)));

        }
        public string GetPlayListTitle(AutotraderTV name)
        {
            return GetElementText(FindElement(By.CssSelector(name.GetAttribute<DescriptionAttribute>().Description)));

        }
        public void ClickPlaylistTitle(AutotraderTV link, int n)
        {
            IList<IWebElement> PlayListTitles = FindElements(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description));
            PlayListTitles[n].Click();
        }
        public bool IsArticleWidgetDisplaying(ArticlePage widget)
        {
            return IsElementVisible(By.CssSelector(widget.GetAttribute<DescriptionAttribute>().Description));
        }
    }
}
