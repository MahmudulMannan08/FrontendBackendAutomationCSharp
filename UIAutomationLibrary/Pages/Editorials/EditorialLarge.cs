using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System.ComponentModel;
using System;
using UIAutomationLibrary.Locators;

namespace UIAutomationLibrary.Pages.Editorials
{
    public class EditorialLarge : EditorialsAbstract
    {
        public EditorialLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickOnReviewsAndAdvice()
        {
            ClickElement(FindElement(By.CssSelector(HubPagesLocators.ResearchPage.ReviewsAdviceLink.GetAttribute<DescriptionAttribute>().Description)));
        }
       

    }
}
