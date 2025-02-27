using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.ComponentModel;

namespace MarketPlaceWeb.Pages.Shared
{
    public class SharedXS : SharedAbstract
    {
        public SharedXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickCategoryAbs(HeaderFooterLocators.Categories category)
        {
            string locator = (language.ToString() == "EN") ? category.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : category.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench;
            ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.CategoryDropdown.GetAttribute<DescriptionAttribute>().Description)));
            WaitForCategoryStatus(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.CategoryDropdownStatusLocator.GetAttribute<DescriptionAttribute>().Description)));
            ClickElement(FindElement(By.CssSelector(locator)));
            WaitForPageLoad(30);
        }

        public override string ActiveCategoryAbs()
        {
            return GetElementText(FindElement(By.CssSelector("#mobileMicrosites a.btn")));
        }

        #region BuyersHub
        public override bool IsFooterDisplayed()
        {
            ScrollToBottom();
            List<string> footerLocators = new List<string> { HeaderFooterLocators.Footer.LogoFooter.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.FacebookBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.TwitterBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.YoutubeBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.CopyRightLbl.GetAttribute<DescriptionAttribute>().Description };
            foreach (var footerLocator in footerLocators)
            {
                if (IsElementAvailable(By.CssSelector(footerLocator)))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}