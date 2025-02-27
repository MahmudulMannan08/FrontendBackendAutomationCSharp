using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Linq;

namespace MarketPlaceWeb.Pages.MyGarage
{
    public class MyGarageLarge : MyGarageAbstract
    {
        public MyGarageLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region MyGarageCommon
        public override void SwitchToMyGarageTab(MyGarageTabs myGarageTabs)
        {
            var allTabs = FindElements(By.CssSelector(MyGarageLocators.MyGarageTabs.AllTabs.GetAttribute<DescriptionAttribute>().Description));
            switch (myGarageTabs)
            {
                case MyGarageTabs.Home:
                    IWebElement homeTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.Home.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.Home.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(homeTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.MyVehicles:
                    IWebElement myVehiclesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.MyVehicles.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.MyVehicles.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(myVehiclesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.SavedVehicles:
                    IWebElement savedVehiclesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedVehicles.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedVehicles.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(savedVehiclesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.PriceAlerts:
                    IWebElement priceAlertsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.PriceAlerts.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.PriceAlerts.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(priceAlertsTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.SavedSearches:
                    IWebElement savedSearchesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedSearches.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedSearches.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(savedSearchesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.ManageAds:
                    IWebElement manageAdsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.ManageAds.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.ManageAds.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(manageAdsTab);
                    WaitForPageLoad(60);  //Not angular page yet
                    break;
                case MyGarageTabs.AccountSettings:
                    IWebElement accountSettingsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.AccountSettings.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.AccountSettings.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(accountSettingsTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.PreferenceCentre:
                    IWebElement preferenceCentreTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.PreferenceCentre.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.PreferenceCentre.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ClickElement(preferenceCentreTab);
                    WaitForPageLoad(60);  //Not angular page yet
                    break;
            }

            CloseSurveyCampaignDialog();
        }
        #endregion

        #region AccountSettings
        public override void DeleteSsoAccount()
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.DeleteMyAccount.GetAttribute<DescriptionAttribute>().Description)));
            WaitForDeleteSsoModalStatus();
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.DeleteAccountBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.AccountSettings.DeleteAccSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.DeleteAccSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForDeleteSsoModalStatus(false);
            WaitForPageLoad(60);
        }
        #endregion

        #region Home
        public override void ClickSavedSearchLink()
        {
            By SeeAllSavedSearchesLink = By.XPath(MyGarageLocators.CommonLocators.SeeAllSavedSearchLink.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(SeeAllSavedSearchesLink, 30);
            IWebElement savedSearchLink = FindElement(SeeAllSavedSearchesLink);
            ScrollTo(savedSearchLink);
            ClickElement(savedSearchLink);
        }

        public override void ClickShowAllPriceAlert()
        {
            IWebElement element = FindElement(By.XPath(MyGarageLocators.Home.SeeAllPriceAlerts.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForMyGarageLoad();
            WaitForPageLoad(10);
        }
        #endregion
    }
}
