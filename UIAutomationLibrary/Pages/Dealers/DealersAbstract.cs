using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Locators;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace UIAutomationLibrary.Pages.Dealers
{
    public abstract class DealersAbstract : Page
    {
        protected DealersAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }
        public bool IsDealerPageWidgetDisplaying(DealerPagesLocators.Dealer widget)
        {
            return IsElementVisible(By.CssSelector(widget.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickDealersTextLink(DealerPagesLocators.Dealer link)
        {
            IWebElement element = FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);

            ClickElement(element);
        }
        public string GetAltAttribute(DealerPagesLocators.Dealer element, string attributeName)
        {
           return GetElementAttribute(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description), attributeName);
         }
        public bool WaitUntilAttributeValueEquals(DealerPagesLocators.Dealer element, String attributeName, String attributeValue)
        {

           return WaitUntil(() => FindElement(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description)).GetAttribute(attributeName).Contains(attributeValue)) ;

        }
        public  string GetInventoryListingTitle(int ListingNo, DealerPagesLocators.Dealer listing)
        {
            IList<IWebElement> elements = FindElements(By.CssSelector(listing.GetAttribute<DescriptionAttribute>().Description));
            if (elements.Count < ListingNo)
            {
                throw new Exception("Listing Number should be less than total number of inventories on Dealer page.");
            }
            else
            {
                ScrollTo(elements[ListingNo]);
                return GetElementText(elements[ListingNo]);
            }
        }
        public string GetH1TagText(DealerPagesLocators.Dealer tag)
        {
            return GetElementText(FindElement(By.CssSelector(tag.GetAttribute<DescriptionAttribute>().Description)));
        }
        public void ClickOnNonDealerInventoryListings(int i)
        {
            IList<IWebElement> elements = FindElements(By.CssSelector(DealerPagesLocators.Dealer.NonDealerInventoryListings.GetAttribute<DescriptionAttribute>().Description));
            elements[i].Click();

        }
        public void ClickOnFindOutHowLink(int i)
        {
            IList<IWebElement> elements = FindElements(By.CssSelector(DealerPagesLocators.Dealer.FindOutHowLink.GetAttribute<DescriptionAttribute>().Description));
            elements[i].Click();

        }
    }
}
