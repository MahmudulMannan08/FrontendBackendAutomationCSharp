using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;

namespace MarketPlaceWeb.Pages.Shared
{
    public class SharedMain : Page
    {
        SharedAbstract shared;
        private const string _defaultValue = null;

        public SharedMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    shared = new SharedLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    shared = new SharedXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    shared = new SharedSmall(driver, viewport, language);
                    break;
            }
        }

        public string ActiveCategory()
        {
            return shared.ActiveCategoryAbs();
        }

        public void ClickCategory(HeaderFooterLocators.Categories category)
        {
            shared.ClickCategoryAbs(category);
        }

        #region BuyersHub
        public void GoToBuyOnlinePage(bool isHomeDeliveryEnabled=false)
        {
            shared.ClickOnBuyOnlineMenu(isHomeDeliveryEnabled);
        }

        public bool IsLogoDisplayed()
        {
            return shared.IsLogoDisplayed();
        }

        public bool AreAllCategoriesDisplayed()
        {
            return shared.AreAllCategoriesDisplayed();
        }

        public bool AreAllMenusDisplayed()
        {
            return shared.AreAllMenusDisplayed();
        }

        public bool IsFooterDisplayed()
        {
            return shared.IsFooterDisplayed();
        }
        #endregion

        public bool IsArraySortedReverseOrder(double[] array)
        {
            return shared.IsArraySortedReverseOrder(array);
        }
    }
}