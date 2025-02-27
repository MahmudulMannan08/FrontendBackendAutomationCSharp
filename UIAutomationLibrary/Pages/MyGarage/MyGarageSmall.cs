using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Linq;

namespace MarketPlaceWeb.Pages.MyGarage
{
    public class MyGarageSmall : MyGarageAbstract
    {
        public MyGarageSmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region MyGarageCommon
        public override void SwitchToMyGarageTab(MyGarageTabs myGarageTabs)
        {
            WaitForElementVisible(By.CssSelector(MyGarageLocators.MyGarageTabs.MenuContainer.GetAttribute<DescriptionAttribute>().Description), 20);
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
        public override void ClickShowAllPriceAlert()
        {
            IWebElement element = FindElement(By.XPath(MyGarageLocators.Home.SeeAllPriceAlerts.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForMyGarageLoad();
            WaitForPageLoad(10);
        }

        public override void UnsubscribeUnSaveVehicleInWidget(string vdpTitle)
        {
            var allVdpWidgetCardElements = FindElements(By.CssSelector(MyGarageLocators.Home.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            int allVdpWidgetCardsCount = allVdpWidgetCardElements != null ? allVdpWidgetCardElements.Count : throw new Exception("Vdp card list found empty.");

            IWebElement vdpWidgetCardElement = allVdpWidgetCardElements.FirstOrDefault(x => vdpTitle.Contains(x.FindElement(By.CssSelector("div.name")).Text));
            IWebElement vdpWidgetUnsubscribeBtnElement = vdpWidgetCardElement != null ? vdpWidgetCardElement.FindElement(By.CssSelector("div.icon")) : throw new Exception("Can't find expected VDP in My Garage.");

            ScrollTo(vdpWidgetUnsubscribeBtnElement);
            ClickElement(vdpWidgetUnsubscribeBtnElement);
            WaitForElementVisible(By.CssSelector(MyGarageLocators.HomeSmall.SavedSubscribedVehicleWidgetRemoveBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.HomeSmall.SavedSubscribedVehicleWidgetRemoveBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitUntil(() => GetAllVdpCardsCountInWidget() < allVdpWidgetCardsCount);
        }
        #endregion

        #region MyVehicles
        public override void EnterTrimMileageDetails(MyVehicle myVehicle, AddVehicleModal addVehicleModal)
        {
            CustomSelectFirstOption(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownContent.GetAttribute<DescriptionAttribute>().Description));
            WaitForMyGarageLoad();

            switch (addVehicleModal)
            {
                case AddVehicleModal.AddNewVehicle:
                    ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.TrimMileageNextBtn.GetAttribute<DescriptionAttribute>().Description)));
                    break;
                case AddVehicleModal.AddVinToExistingVehicle:
                    ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVinNextBtn.GetAttribute<DescriptionAttribute>().Description)));
                    break;
            }
            WaitForMyGarageLoad();
        }

        public override void EnterYmmDetails(MyVehicle myVehicle)
        {
            if (string.IsNullOrEmpty(myVehicle.Year) || string.IsNullOrEmpty(myVehicle.Make))
            {
                throw new Exception("Missing data to add my vehicle, Year: " + myVehicle.Year + " Make: " + myVehicle.Make);
            }
            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.YearDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.YearDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Year);
            WaitForMyGarageLoad();

            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.MakeDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.MakeDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Make);
            WaitForMyGarageLoad();

            if (string.IsNullOrEmpty(myVehicle.Model))
            {
                CustomSelectFirstOption(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownContent.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Model);
            }
            WaitForMyGarageLoad();

            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VinYmmNextBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForMyGarageLoad();
        }
        public override void EnterTrimMileageColorDetails(MyVehicle myVehicle)
        {
            if (string.IsNullOrEmpty(myVehicle.Trim) || string.IsNullOrEmpty(myVehicle.Colour))
            {
                throw new Exception("Missing data to update my vehicle, Trim: " + myVehicle.Trim + " Colour: " + myVehicle.Colour);
            }
            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownIcon.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Trim);
            WaitForMyGarageLoad();

            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.KilometresInputTxt.GetAttribute<DescriptionAttribute>().Description)));
            EnterTextUsingJS(By.CssSelector(MyGarageLocators.MyVehicles.KilometresInputTxt.GetAttribute<DescriptionAttribute>().Description), myVehicle.Kilometres);

            SelectColourByName(myVehicle.Colour);
            WaitForMyGarageLoad();
        }

        public override void SelectColourByName(string rgbColor)
        {
            WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.ColorsContainerDiv.GetAttribute<DescriptionAttribute>().Description));
            try
            {
                var colorSquares = FindElements(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleColors.GetAttribute<DescriptionAttribute>().Description));
                foreach (var colorSquare in colorSquares)
                {
                    string styleAttribute = colorSquare.GetAttribute("style");
                    if (styleAttribute.Contains($"background-color: {rgbColor}"))
                    {
                        ClickElement(colorSquare);
                        return;
                    }
                }
                if (!IsColourSelected(rgbColor))
                {
                    throw new Exception($"Selected color '{rgbColor}' is not displayed besides vehicle colour");
                }
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Element with color name '{rgbColor}' not found.");
            }
            catch (Exception)
            {
                throw new Exception("An error occured selecting color from color picker for My Vehicle.");
            }
        }
        #endregion

        #region PriceAlert
        public override void UnsubscribeUnSaveVehicleInMyGarage(string vdpTitle)
        {
            var allVdpCardElements = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            int allVdpCardsCount = allVdpCardElements != null ? allVdpCardElements.Count : throw new Exception("Vdp card list found empty.");

            IWebElement vdpCardElement = allVdpCardElements.FirstOrDefault(x => vdpTitle.Contains(x.FindElement(By.CssSelector("div.name")).Text));
            IWebElement vdpUnsubscribeBtnElement = vdpCardElement != null ? vdpCardElement.FindElement(By.CssSelector("div.icon")) : throw new Exception("Can't find expected VDP in My Garage.");

            ScrollTo(vdpUnsubscribeBtnElement);
            ClickElement(vdpUnsubscribeBtnElement);
            WaitForElementVisible(By.CssSelector(MyGarageLocators.SmallLocators.SavedSubscribedVehicleTabRemoveBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.SmallLocators.SavedSubscribedVehicleTabRemoveBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitUntil(() => GetAllVdpCardsCountInMyGarage() < allVdpCardsCount);
        }

        public override void RedirectToVDPFromPriceAlertSavedVehicles(string vdpTitle)
        {
            IWebElement savedPriceAlertElement = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleTitles.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => vdpTitle.Contains(x.Text));
            ClickElement(savedPriceAlertElement);
            Wait(3);  //Browserstack iPad is slow sometimes and gets page ready status even before redirection happens.
            WaitForPageLoad(60);
        }
        #endregion
    }
}
