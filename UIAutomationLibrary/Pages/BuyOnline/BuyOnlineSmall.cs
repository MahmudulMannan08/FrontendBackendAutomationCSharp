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
    public class BuyOnlineSmall : BuyOnlineAbstract
    {
        public BuyOnlineSmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsGreatPriceBadgeAvailable()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.SmallLocators.GoodPriceBadge.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.SmallLocators.GreatPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsGoodPriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.SmallLocators.GoodPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsFairPriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.SmallLocators.FairPriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override bool IsAbovePriceBadgeAvailable()
        {
            return IsElementAvailable(By.CssSelector(BuyOnlineLocators.SmallLocators.AbovePriceBadge.GetAttribute<DescriptionAttribute>().Description));
        }

        public override string GetLiveChatCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[0];
            ScrollTo(element);
            return GetElementText(element).ToUpper();
        }

        public override string GetHomeTestDriveCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[1];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[0];
            try
            {
                if (!element.Displayed)
                {
                    SwipeLeft(element, elementTo);
                    return GetElementText(element).ToUpper();
                }
                return GetElementText(element).ToUpper();
            }
            catch (Exception)
            {
                ScrollTo(element);
                return GetElementText(element).ToUpper();
            }
        }

        public override string GetVirtualAppraisalCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[2];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[1];
            try
            {
                if (!element.Displayed)
                {
                    SwipeLeft(element, elementTo);
                    return GetElementText(element).ToUpper();
                }
                return GetElementText(element).ToUpper();
            }
            catch (Exception)
            {
                ScrollTo(element);
                return GetElementText(element).ToUpper();
            }
        }

        public override string GetOnlineReservationCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[3];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[2];
            try
            {
                if (!element.Displayed)
                {
                    SwipeLeft(element, elementTo);
                    return GetElementText(element).ToUpper();
                }
                return GetElementText(element).ToUpper();
            }
            catch (Exception)
            {
                ScrollTo(element);
                return GetElementText(element).ToUpper();
            }
        }

        public override string GetLocalDeliveryCarouselTitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[4];
            IWebElement elementTo = FindElements(By.CssSelector(BuyOnlineLocators.SmallLocators.Carousels.GetAttribute<DescriptionAttribute>().Description))[3];
            try
            {
                if (!element.Displayed)
                {
                    SwipeLeft(element, elementTo);
                    return GetElementText(element).ToUpper();
                }
                return GetElementText(element).ToUpper();
            }
            catch (Exception)
            {
                ScrollTo(element);
                return GetElementText(element).ToUpper();
            }
        }

        public override void WaitForSRPPageLoad(int timeOut = 60)
        {
            WaitUntil(() => driver.FindElements(By.CssSelector(SRPLocators.SmallLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).Count > 0 ? driver.FindElements(By.CssSelector(SRPLocators.SmallLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Displayed : false, timeOut);
            Wait(2);  //Sometimes iPad takes time to load the error page
        }
    }
}
