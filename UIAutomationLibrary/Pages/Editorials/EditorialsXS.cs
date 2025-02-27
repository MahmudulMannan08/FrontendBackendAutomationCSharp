using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System.ComponentModel;
using MarketPlaceWeb.Locators;

namespace UIAutomationLibrary.Pages.Editorials
{
    public class EditorialsXS : EditorialsAbstract
    {
        public EditorialsXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickOnReviewsAndAdvice()
        {
            ClickElement(FindElement(By.CssSelector(Locators.HubPagesLocators.ResearchPage.HamburgurMenu.GetAttribute<DescriptionAttribute>().Description)));
            string AdviceLinkLocator = (language.ToString() == "EN") ? (Locators.HubPagesLocators.ResearchPage.ReviewsAdviceLink.GetAttribute<DescriptionAttribute>().Description) : (Locators.HubPagesLocators.ResearchPage.ReviewsAdviceLink.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench);
            ClickElement(FindElement(By.LinkText(AdviceLinkLocator)));
        }
      

    }
}
