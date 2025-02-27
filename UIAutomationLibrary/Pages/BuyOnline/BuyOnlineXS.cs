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
    public class BuyOnlineXS : BuyOnlineAbstract
    {
        public BuyOnlineXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsGreatPriceBadgeAvailable()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.XSLocators.GreatPriceBadge.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.XSLocators.GreatPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsGoodPriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.XSLocators.GoodPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsFairPriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.XSLocators.FairPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsAbovePriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.XSLocators.AbovePriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override string GetLiveChatCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[0];
            ScrollTo(element);
            return GetElementText(element).ToUpper();  //On iOS devices GetElementText returns Clavardage (chat) whereas Desktop and Android returns Clavardage (Chat)
        }

        public override string GetHomeTestDriveCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[1];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[0];
            SwipeLeft(element, elementTo);
            return GetElementText(element).ToUpper();
        }

        public override string GetVirtualAppraisalCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[2];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[1];
            SwipeLeft(element, elementTo);
            return GetElementText(element).ToUpper();
        }

        public override string GetOnlineReservationCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[3];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[2];
            SwipeLeft(element, elementTo);
            return GetElementText(element).ToUpper();
        }

        public override string GetLocalDeliveryCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[4];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.XSLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[3];
            SwipeLeft(element, elementTo);
            return GetElementText(element).ToUpper();
        }

        public override void WaitForSRPPageLoad(int timeOut = 60)
        {
            WaitUntil(() => driver.FindElements(By.CssSelector(SRPLocators.XSLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).Count > 0 ? driver.FindElements(By.CssSelector(SRPLocators.XSLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Displayed : false, timeOut);
        }
    }
}
