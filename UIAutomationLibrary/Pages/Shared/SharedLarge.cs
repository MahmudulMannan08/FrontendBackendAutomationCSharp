using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.ComponentModel;

namespace MarketPlaceWeb.Pages.Shared
{
    public class SharedLarge : SharedAbstract
    {
        public SharedLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override void ClickCategoryAbs(HeaderFooterLocators.Categories category)
        {
            string locator = (language.ToString() == "EN") ? category.GetAttribute<DescriptionAttribute>().Description : category.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            ClickElement(FindElement(By.CssSelector(locator)));
            WaitForPageLoad(45);
        }

        public override string ActiveCategoryAbs()
        {
            return FindElement(By.CssSelector("#desktopMicrosites a[class='active']")).Text.ToString();
        }

        #region BuyersHub
        public override void ClickOnBuyOnlineMenu(bool isHomeDeliveryEnabled)
        {
            string locator; 
            if (isHomeDeliveryEnabled)
            {
                locator = (language.ToString() == "EN") ? HeaderFooterLocators.Menus.HomeDelivery.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Menus.HomeDelivery.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            }
            else
            {
                locator = (language.ToString() == "EN") ? HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            }
            ClickElement(FindElement(By.CssSelector(locator)));
            WaitForPageLoad(30);
        }

        public override bool AreAllCategoriesDisplayed()
        {
            List<string> categoryLocators = new List<string> { (language.ToString() == "EN") ? HeaderFooterLocators.Categories.CarsTrucksSuv.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.CarsTrucksSuv.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Commercial.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Commercial.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Trailers.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Trailers.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.RVs.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.RVs.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Boats.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Boats.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Watercraft.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Watercraft.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Bikes.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Bikes.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Snowmobiles.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Snowmobiles.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.HeavyEquipment.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.HeavyEquipment.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Farm.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Categories.Farm.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench };

            foreach (var categoryLocator in categoryLocators)
            {
                if (IsElementVisible(By.CssSelector(categoryLocator)))
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

        public override bool AreAllMenusDisplayed()
        {
            List<string> menuLocators = new List<string> { (language.ToString() == "EN") ? HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench,
                HeaderFooterLocators.Menus.SellMyCar.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Menus.ShopElectric.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Menus.ReviewsAndAdvice.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Menus.WhatsMyCarWorth.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Menus.SignIn.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Menus.LanguageToggle.GetAttribute<DescriptionAttribute>().Description };

            foreach (var menuLocator in menuLocators)
            {
                if (IsElementVisible(By.CssSelector(menuLocator)))
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