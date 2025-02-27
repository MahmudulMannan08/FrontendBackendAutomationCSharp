using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System.ComponentModel;

namespace MarketPlaceWeb.Pages.Shared
{
    public class SharedSmall : SharedAbstract
    {
        public SharedSmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickCategoryAbs(HeaderFooterLocators.Categories category)
        {
            ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.SmallLocators.CategoryDropdown.GetAttribute<DescriptionAttribute>().Description)));
            WaitForCategoryStatus(FindElement(By.CssSelector(HeaderFooterLocators.SmallLocators.CategoryDropdownStatusLocator.GetAttribute<DescriptionAttribute>().Description)));
            string locator = (language.ToString() == "EN") ? category.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : category.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench;
            ClickElement(FindElement(By.CssSelector(locator)));
            WaitForPageLoad(60);
        }

        public override string ActiveCategoryAbs()
        {
            return FindElement(By.CssSelector("#mobileMicrosites a.btn")).Text.ToString();
        }
    }
}