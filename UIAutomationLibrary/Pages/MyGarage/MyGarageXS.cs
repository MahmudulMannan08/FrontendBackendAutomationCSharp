using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System.ComponentModel;
using System.Linq;

namespace MarketPlaceWeb.Pages.MyGarage
{
    public class MyGarageXS : MyGarageAbstract
    {
        public MyGarageXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region MyGarageCommon
        public override void SwitchToMyGarageTab(MyGarageTabs myGarageTabs)
        {
            var allTabs = FindElements(By.CssSelector(MyGarageLocators.MyGarageTabs.AllTabs.GetAttribute<DescriptionAttribute>().Description));
            switch (myGarageTabs)
            {
                case MyGarageTabs.Home:
                    IWebElement homeTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.Home.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.Home.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(homeTab);
                    ClickElement(homeTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.MyVehicles:
                    IWebElement myVehiclesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.MyVehicles.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.MyVehicles.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(myVehiclesTab);
                    ClickElement(myVehiclesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.SavedVehicles:
                    IWebElement savedVehiclesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedVehicles.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedVehicles.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(savedVehiclesTab);
                    ClickElement(savedVehiclesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.PriceAlerts:
                    IWebElement priceAlertsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.PriceAlerts.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.PriceAlerts.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(priceAlertsTab);
                    ClickElement(priceAlertsTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.SavedSearches:
                    IWebElement savedSearchesTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedSearches.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.SavedSearches.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(savedSearchesTab);
                    ClickElement(savedSearchesTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.ManageAds:
                    IWebElement manageAdsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.ManageAds.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.ManageAds.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(manageAdsTab);
                    ClickElement(manageAdsTab);
                    WaitForPageLoad(60);  //Not angular page yet
                    break;
                case MyGarageTabs.AccountSettings:
                    IWebElement accountSettingsTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.AccountSettings.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.AccountSettings.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(accountSettingsTab);
                    ClickElement(accountSettingsTab);
                    WaitForMyGarageLoad();
                    WaitForPageLoad(10);
                    break;
                case MyGarageTabs.PreferenceCentre:
                    IWebElement preferenceCentreTab = allTabs.FirstOrDefault(x => x.Text.Contains(MyGarageLocators.MyGarageTabs.PreferenceCentre.GetAttribute<DescriptionAttribute>().Description) || x.Text.Contains(MyGarageLocators.MyGarageTabs.PreferenceCentre.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
                    ScrollTo(preferenceCentreTab);
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
            IWebElement element = FindElement(By.CssSelector(MyGarageLocators.AccountSettings.DeleteMyAccount.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
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
            By savedSearchLabelLocator = By.XPath(MyGarageLocators.XSLocators.SeeAllSavedSearchLabel.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(savedSearchLabelLocator);
            IWebElement savedSearchLabel = FindElement(savedSearchLabelLocator);
            ScrollTo(savedSearchLabel);
            IWebElement saveSearchLabelParent = savedSearchLabel.FindElement(By.XPath(".."));
            IWebElement saveSearchLink = saveSearchLabelParent.FindElement(By.CssSelector(MyGarageLocators.XSLocators.SeeAllSavedSearchLink.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(saveSearchLink);
        }

        public override void ClickShowAllPriceAlert()
        {
            IWebElement element = FindElement(By.XPath(MyGarageLocators.HomeXS.SeeAllPriceAlerts.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForMyGarageLoad();
            WaitForPageLoad(10);
        }
        #endregion

        #region MyVehicles
        public override void OpenAddVinModal()
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVinBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForAddVehicalModal();
        }
        #endregion
    }
}
