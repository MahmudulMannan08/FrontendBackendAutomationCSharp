using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace MarketPlaceWeb.Pages.MyGarage
{
    public class MyGarageMain : Page
    {
        MyGarageAbstract myGaragePage;

        public MyGarageMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    myGaragePage = new MyGarageLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    myGaragePage = new MyGarageXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    myGaragePage = new MyGarageSmall(driver, viewport, language);
                    break;
            }
        }

        public bool CheckExistingValuess(IDictionary<MyGarageLocators.AccountSettings, string> existingValues) => myGaragePage.CheckExistingValues(existingValues);

        #region MyGarageCommon
        public void SwitchToMyGarageTab(MyGarageTabs myGarageTabs) => myGaragePage.SwitchToMyGarageTab(myGarageTabs);
        #endregion

        #region PriceAlert
        public bool IsVehicleDisplayedInList(string vdpTitle, bool isDisplayed = true)
        {
            return myGaragePage.IsVehicleDisplayedInList(vdpTitle, isDisplayed);
        }

        public void UnsubscribeUnSaveVehicleInMyGarage(string vdpTitle) => myGaragePage.UnsubscribeUnSaveVehicleInMyGarage(vdpTitle);

        public int GetAllVdpCardsCountInMyGarage() => myGaragePage.GetAllVdpCardsCountInMyGarage();

        public void RedirectToVDPFromPriceAlertSavedVehicles(string vdpTitle) => myGaragePage.RedirectToVDPFromPriceAlertSavedVehicles(vdpTitle);

        public bool? IsNavigatedToPriceAlertsTab()
        {
            return myGaragePage.IsNavigatedToPriceAlertsTab();
        }
        #endregion

        #region AccountSettings
        public void DeleteSsoAccount() => myGaragePage.DeleteSsoAccount();

        public bool? VerifyAccountDetailsAreCorrect(string username, string firstName, string lastName) => myGaragePage.VerifyAccountDetailsAreCorrect(username, firstName, lastName);

        public void ClickEditPersonalDetails() => myGaragePage.ClickEditPersonalDetails();

        public void UpdatePersonalDetails(IDictionary<MyGarageLocators.AccountSettings, string> toUpdate) => myGaragePage.UpdatePersonalDetails(toUpdate);

        public bool ValidateUpdatedPersonalDetails(IDictionary<MyGarageLocators.AccountSettings, string> toUpdate) => myGaragePage.ValidateUpdatedPersonalDetails(toUpdate);

        public void ClickChangePassword() => myGaragePage.ClickChangePassword();

        public void UpdatePassword(string updatePassword, string oldPassword) => myGaragePage.UpdatePassword(updatePassword, oldPassword);

        public bool VerifyAccountDeatilsPageDoesntHaveUpdatePasswordForSocialAccounts() => myGaragePage.VerifyAccountDeatilsPageDoesntHaveUpdatePasswordForSocialAccounts();
        #endregion

        #region SavedSearch
        public void ClickSavedSearchLink()
        {
            myGaragePage.ClickSavedSearchLink();
        }
        public void ClickEditSavedSearchLink(string searchName)
        {
            myGaragePage.ClickEditSavedSearchLink(searchName);
        }

        public bool IsEditSavedSearchDisplayed()
        {
            return myGaragePage.IsEditSavedSearchDisplayed();
        }
        public void UpdateSelectedSavedSearch(string newSearchName)
        {
            myGaragePage.UpdateSelectedSavedSearch(newSearchName);
        }
        public string GetToasterMessage(string selectorType)
        {
            return myGaragePage.GetToasterMessage(selectorType);
        }
        public bool IsSavedSearchAvailable(string searchName)
        {
            return myGaragePage.IsSavedSearchAvailable(searchName);
        }
        public void RedirectToSRPFromSavedSearch(string searchName)
        {
            myGaragePage.RedirectToSRPFromSavedSearch(searchName);
        }
        public void DeleteSelectedSavedSearch()
        {
            myGaragePage.DeleteSelectedSavedSearch();
        }
        public void ClickUndoLink()
        {
            myGaragePage.ClickUndoLink();
        }
        public void WaitForToasterMsgNotVisible(string selectorType)
        {
            myGaragePage.WaitForToasterMsgNotVisible(selectorType);
        }

        public bool IsSavedSearchDisplayedForLeadFormSubsciption()
        {
            return myGaragePage.IsSavedSearchDisplayedForLeadFormSubsciption();
        }
        #endregion

        #region Home
        public void ClickShowAllPriceAlert()
        {
            myGaragePage.ClickShowAllPriceAlert();
        }

        public bool IsSavedSubscribedVehicleDisplayedInWidget(string vdpTitle, bool isDisplayed = true) => myGaragePage.IsSavedSubscribedVehicleDisplayedInWidget(vdpTitle, isDisplayed);

        public void UnsubscribeUnSaveVehicleInWidget(string vdpTitle) => myGaragePage.UnsubscribeUnSaveVehicleInWidget(vdpTitle);
        #endregion

        #region PreferenceCentre
        public void UpdatePreferenceCentreSettings(IDictionary<MyGarageLocators.PreferenceCentreSettings, bool> preferenceCentreSettings) => myGaragePage.UpdatePreferenceCentreSettings(preferenceCentreSettings);
        public bool IsPreferenceCentreModalDisplayed() => myGaragePage.IsPreferenceCentreModalDisplayed();
        public void ClickPreferenceCenterUpdateBtn() => myGaragePage.ClickPreferenceCenterUpdateBtn();
        public void ClickPreferenceCenterUnsubscribeLink() => myGaragePage.ClickPreferenceCenterUnsubscribeLink();
        #endregion

        #region MyVehicles
        public bool IsMyVehicleEmptyStateDisplayed() => myGaragePage.IsMyVehicleEmptyStateDisplayed();

        public void AddMyVehicle(MyVehicle myVehicle, AddVehicleModal addVehicleModal = AddVehicleModal.AddNewVehicle, bool isEmptyState = true) => myGaragePage.AddMyVehicle(myVehicle, addVehicleModal, isEmptyState);

        public void AddVinToMyVehicle(MyVehicle myVehicle, AddVehicleModal addVehicleModal = AddVehicleModal.AddVinToExistingVehicle) => myGaragePage.AddVinToMyVehicle(myVehicle, addVehicleModal);

        public void EditMyVehicle(MyVehicle myVehicle) => myGaragePage.EditMyVehicle(myVehicle);
        public void DeleteMyVehicleFromEditVehicle() => myGaragePage.DeleteMyVehicleFromEditVehicle();

        public bool IsAddMyVehicleSuccess() => myGaragePage.IsAddMyVehicleSuccess();

        public bool IsAddMyVehicleSuccessModalDisplayed() => myGaragePage.IsAddMyVehicleSuccessModalDisplayed();

        public void CloseAddVehicleSuccessModal() => myGaragePage?.CloseAddVehicleSuccessModal();

        public bool IsAddVehicleTabOnCarrouselDisplayed() => myGaragePage.IsAddVehicleTabOnCarrouselDisplayed();

        public int GetMyVehiclesCount() => myGaragePage.GetMyVehiclesCount();

        public string GetVehicleTitle() => myGaragePage.GetVehicleTitle();

        public string GetVehicleTrimDetail() => myGaragePage.GetVehicleTrimDetail();

        public string GetVehicleColorDetail() => myGaragePage.GetVehicleColorDetail();

        public string GetVehicleMileageDetail() => myGaragePage.GetVehicleMileageDetail();

        public string GetVehicleVinDetail() => myGaragePage.GetVehicleVinDetail();
        #endregion
    }
}
