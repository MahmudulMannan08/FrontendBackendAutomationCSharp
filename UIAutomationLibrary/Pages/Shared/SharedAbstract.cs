using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace MarketPlaceWeb.Pages.Shared
{
    public abstract class SharedAbstract : Page
    {
        protected SharedAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        #region Homepage
        public abstract void ClickCategoryAbs(HeaderFooterLocators.Categories category);

        public abstract string ActiveCategoryAbs();

        public void WaitForCategoryStatus(IWebElement categoryStatusLocatorElement, bool isCategoryExpanded = true)
        {
            if (isCategoryExpanded)
            {
                WaitUntil(() => categoryStatusLocatorElement.GetAttribute("class").ToLower().Contains("open"));
            }
            else
            {
                WaitUntil(() => !categoryStatusLocatorElement.GetAttribute("class").ToLower().Contains("open"));
            }
        }
        #endregion

        #region BuyersHub
        public virtual void ClickOnBuyOnlineMenu(bool isHomeDeliveryEnabled)
        {
            string locator;
            IWebElement menuToggleLocator = FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(menuToggleLocator);
            try
            {
                WaitUntil(() => IsElementVisible(By.CssSelector(HeaderFooterLocators.Menus.MenuOpen.GetAttribute<DescriptionAttribute>().Description)), 20);
            }
            catch (Exception)
            {
                RefreshPage();
                ClickElement(menuToggleLocator);
            }
            if (isHomeDeliveryEnabled)
            {
                locator = (language.ToString() == "EN") ? HeaderFooterLocators.Menus.HomeDelivery.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Menus.HomeDelivery.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            }
            else
            {
                locator = (language.ToString() == "EN") ? HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionAttribute>().Description : HeaderFooterLocators.Menus.BuyOnline.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            }
            WaitForElementVisible(By.CssSelector(locator));
            ClickElement(FindElement(By.CssSelector(locator)));
            WaitForPageLoad(60);
        }

        public bool IsLogoDisplayed()
        {
            return IsElementVisible(By.CssSelector(HeaderFooterLocators.Header.Logo.GetAttribute<DescriptionAttribute>().Description));
        }

        public virtual bool AreAllCategoriesDisplayed()
        {
            if (!IsElementVisible(By.CssSelector(HeaderFooterLocators.Categories.CategoryDropDownBtn.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile)))
            {
                return false;
            }

            List<string> categoryLocators = new List<string> { (language.ToString() == "EN") ? HeaderFooterLocators.Categories.CarsTrucksSuv.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.CarsTrucksSuv.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Commercial.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Commercial.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Trailers.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Trailers.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.RVs.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.RVs.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Boats.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Boats.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Watercraft.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Watercraft.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Bikes.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Bikes.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Snowmobiles.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Snowmobiles.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.HeavyEquipment.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.HeavyEquipment.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench,
                (language.ToString() == "EN") ? HeaderFooterLocators.Categories.Farm.GetAttribute<DescriptionMobileAttribute>().DescriptionMobile : HeaderFooterLocators.Categories.Farm.GetAttribute<DescriptionMobileFrenchAttribute>().DescriptionMobileFrench };

            foreach (var categoryLocator in categoryLocators)
            {
                if (IsElementAvailable(By.CssSelector(categoryLocator)))
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

        public virtual bool AreAllMenusDisplayed()
        {
            if (!IsElementVisible(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)))
            {
                return false;
            }
            return true;
        }

        public virtual bool IsFooterDisplayed()
        {
            ScrollToBottom();

            List<string> footerLocators = new List<string> { HeaderFooterLocators.Footer.AboutUsContainer.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.DealerServicesContainer.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.ResoursesContainer.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.PartnersContainer.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.CustomerSupportContainer.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.LogoFooter.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.FacebookBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.TwitterBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.YoutubeBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.AndroidAppBadge.GetAttribute<DescriptionAttribute>().Description,
                HeaderFooterLocators.Footer.AppleAppBadge.GetAttribute<DescriptionAttribute>().Description,
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

        /// <summary>
        /// This method checkes array is sorted in descending order
        /// </summary>
        /// <param name="array">array to test</param>
        /// <returns><c>True</c> if sorted</returns>
        public bool IsArraySortedReverseOrder(double[] array)
        {
            int n = array.Length;
            if (n == 1 || n == 0)
                return true;
            for (int i = 1; i < n; i++)
            {
                if (array[i - 1] < array[i])
                    return false;
            }
            return true;
        }

        public static IEnumerable<TestCaseData> TestDataProvider(string testCaseNumber, string keyNodes = null)
        {
            #region Variables
            string baseURL;
            AzureConfig azureConfig;
            LocalConfig localConfig;
            Page page = new Page();
            dynamic testDataFile;
            string urls;
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            #endregion

            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            testDataFile = page.GetTestDataFile(baseURL);
            urls = page.GetTestData(testDataFile, testCaseNumber + keyNodes);

            dynamic deserializedObject = JsonConvert.DeserializeObject(urls);

            foreach (var item in deserializedObject)
            {
                var itemValue = item.Value;
                yield return new TestCaseData(itemValue.ToString());
            }
        }
    }
}