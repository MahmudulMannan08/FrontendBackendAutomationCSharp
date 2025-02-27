using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.BuyOnline
{
    public class BuyOnlineLarge : BuyOnlineAbstract
    {
        public BuyOnlineLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsGreatPriceBadgeAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GreatPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsFairPriceBadgeAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.FairPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsGoodPriceBadgeAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GoodPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsAbovePriceBadgeAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.AbovePriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override string GetLiveChatCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Carousels.GetAttribute<DescriptionAttribute>().Description))[0];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override string GetHomeTestDriveCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Carousels.GetAttribute<DescriptionAttribute>().Description))[1];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override string GetVirtualAppraisalCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Carousels.GetAttribute<DescriptionAttribute>().Description))[2];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override string GetOnlineReservationCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Carousels.GetAttribute<DescriptionAttribute>().Description))[3];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override string GetLocalDeliveryCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Carousels.GetAttribute<DescriptionAttribute>().Description))[4];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override void WaitForSRPPageLoad(int timeOut = 60)
        {
            WaitUntil(() => driver.FindElements(By.CssSelector(SRPLocators.LargeLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).Count > 0 ? driver.FindElements(By.CssSelector(SRPLocators.LargeLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Displayed : false, timeOut);
        }
    }
}
