using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System.ComponentModel;
using MarketPlaceWeb.Locators;
using UIAutomationLibrary.Locators;

namespace UIAutomationLibrary.Pages.HubPages
{
    public class HubPagesLarge : HubPagesAbstract
    {
        public HubPagesLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickOnReviewsAndAdvice()
        {
            ClickElement(FindElement(By.CssSelector(HubPagesLocators.ResearchPage.ReviewsAdviceLink.GetAttribute<DescriptionAttribute>().Description)));
        }
       
    }
}
