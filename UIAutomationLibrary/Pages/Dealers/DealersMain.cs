using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Locators;
using UIAutomationLibrary.Pages.Editorials;

namespace UIAutomationLibrary.Pages.Dealers
{
    public class DealersMain : Page
    {
        DealersAbstract dealerPage;
        public DealersMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    dealerPage = new DealersLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    dealerPage = new DealersXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    dealerPage = new DealersSmall(driver, viewport, language);
                    break;
            }

        }
        public string dealersURL(Language language, dynamic testDataFile)
        {
            return (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.dealers") : GetTestData(testDataFile, "urlsFr.dealers");
        }
        public bool IsDealerPageWidgetDisplaying(DealerPagesLocators.Dealer widget)
        {
            return dealerPage.IsDealerPageWidgetDisplaying(widget);
        }
        public void ClickDealersTextLink(DealerPagesLocators.Dealer link)
        {
            dealerPage.ClickDealersTextLink(link);
        }
        public string GetAltAttribute(DealerPagesLocators.Dealer element, string attributeName)
        {
            return dealerPage.GetAltAttribute(element, attributeName);
        }
        public bool WaitUntilAttributeValueEquals(DealerPagesLocators.Dealer webElement, String attributeName, String attributeValue)
        {

           return  dealerPage.WaitUntilAttributeValueEquals(webElement, attributeName, attributeValue);

        }
        public string GetInventoryListingTitle(int ListingNo, DealerPagesLocators.Dealer listing)
        {
            return dealerPage.GetInventoryListingTitle(ListingNo, listing);
        }
        public string GetH1TagText(DealerPagesLocators.Dealer tag )
        {
            return dealerPage.GetH1TagText(tag);
        }
        public void WaitForPageLoad(String urlContains)
        {
            WaitUntil(() => ExpectedConditions.UrlContains(urlContains));
        }
        public void ClickOnNonDealerInventoryListings(int i)
        {
            dealerPage.ClickOnNonDealerInventoryListings(i);
        }
        public void ClickOnFindOutHowLink(int i)
        { dealerPage.ClickOnFindOutHowLink(i);
        }
    }
}
