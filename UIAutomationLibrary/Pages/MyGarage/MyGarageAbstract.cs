using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MarketPlaceWeb.Pages.MyGarage
{
    public abstract class MyGarageAbstract : Page
    {
        protected MyGarageAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public bool CheckExistingValues(IDictionary<MyGarageLocators.AccountSettings, string> existingValues)
        {
            bool matchStatus = false;
            foreach (KeyValuePair<MyGarageLocators.AccountSettings, string> kvp in existingValues)
            {
                matchStatus = true;
                string value = GetElementText(FindElement(By.CssSelector(kvp.Key.GetAttribute<DescriptionAttribute>().Description))).Trim().ToLower();
                if (!value.Equals(kvp.Value.Trim().ToLower()))
                {

                    matchStatus = false;
                    break;
                }
            }
            return matchStatus;
        }

        #region MyGarageCommon
        public abstract void SwitchToMyGarageTab(MyGarageTabs myGarageTabs);

        public void WaitForMyGarageLoad()
        {
            WaitForAllElementsNotVisible(By.CssSelector(MyGarageLocators.CommonLocators.MyGarageLoading.GetAttribute<DescriptionAttribute>().Description));
        }
        #endregion

        #region PriceAlert
        public bool IsVehicleDisplayedInList(string vdpTitle, bool isDisplayed = true)
        {
            var allSavedVehicleTitleElements = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleTitles.GetAttribute<DescriptionAttribute>().Description));
            if (!isDisplayed)
            {
                return allSavedVehicleTitleElements != null ? allSavedVehicleTitleElements.FirstOrDefault(x => vdpTitle.Contains(x.Text)) == null : true;
            }
            else
            {
                return allSavedVehicleTitleElements != null ? allSavedVehicleTitleElements.FirstOrDefault(x => vdpTitle.Contains(x.Text)) != null : false;
            }
        }

        public virtual void UnsubscribeUnSaveVehicleInMyGarage(string vdpTitle)
        {
            var allVdpCardElements = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            int allVdpCardsCount = allVdpCardElements != null ? allVdpCardElements.Count : throw new Exception("Vdp card list found empty.");
            
            IWebElement vdpCardElement = allVdpCardElements.FirstOrDefault(x => vdpTitle.Contains(x.FindElement(By.CssSelector("div.name")).Text));
            IWebElement vdpUnsubscribeBtnElement = vdpCardElement != null ? vdpCardElement.FindElement(By.CssSelector("div.icon")) : throw new Exception("Can't find expected VDP in My Garage.");
            
            ScrollTo(vdpUnsubscribeBtnElement);
            ClickElement(vdpUnsubscribeBtnElement);
            WaitForElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.SavedSubscribedVehicleTabRemoveBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.CommonLocators.SavedSubscribedVehicleTabRemoveBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitUntil(() => GetAllVdpCardsCountInMyGarage() < allVdpCardsCount);
        }

        public int GetAllVdpCardsCountInMyGarage()
        {
            var allVdpCardElements = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            return allVdpCardElements != null ? allVdpCardElements.Count : 0;
        }

        public virtual void RedirectToVDPFromPriceAlertSavedVehicles(string vdpTitle)
        {
            IWebElement savedPriceAlertElement = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.AllSavedVehicleTitles.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => vdpTitle.Contains(x.Text));
            ClickElement(savedPriceAlertElement);
            WaitForPageLoad(60);
        }

        internal bool? IsNavigatedToPriceAlertsTab()
        {
            string urlWithoutQuery = new Uri(driver.Url).GetLeftPart(UriPartial.Path);
            return urlWithoutQuery.Contains("my-garage/price-alerts");
        }
        #endregion

        #region AccountSettings
        public abstract void DeleteSsoAccount();

        public void WaitForDeleteSsoModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.AccountSettings.DeleteSsoModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.AccountSettings.DeleteSsoModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public void WaitForChangePasswordModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.AccountSettings.ChangePasswordModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.AccountSettings.ChangePasswordModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool? VerifyAccountDetailsAreCorrect(string username, string firstName, string lastName)
        {
            WaitForElementVisible(By.CssSelector(MyGarageLocators.AccountSettings.FirstName.GetAttribute<DescriptionAttribute>().Description));
            return (GetElementText(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.FirstName.GetAttribute<DescriptionAttribute>().Description))).Trim().Equals(firstName)
                   && GetElementText(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.LastName.GetAttribute<DescriptionAttribute>().Description))).Trim().Equals(lastName)
                   && GetElementText(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.Email.GetAttribute<DescriptionAttribute>().Description))).Trim().Equals(username)
                   && GetElementText(FindElement(By.CssSelector(MyGarageLocators.AccountSettings.DisplayName.GetAttribute<DescriptionAttribute>().Description))).Trim().StartsWith(firstName));
        }

        public void UpdatePersonalDetails(IDictionary<MyGarageLocators.AccountSettings, string> toUpdate)
        {
            By btnSave = By.CssSelector(MyGarageLocators.EditAccountDetails.SaveChanges.GetAttribute<DescriptionAttribute>().Description);
            By firstName = By.CssSelector(MyGarageLocators.EditAccountDetails.FirstName.GetAttribute<DescriptionAttribute>().Description);
            By lastName = By.CssSelector(MyGarageLocators.EditAccountDetails.LastName.GetAttribute<DescriptionAttribute>().Description);
            By postCode = By.CssSelector(MyGarageLocators.EditAccountDetails.PostCode.GetAttribute<DescriptionAttribute>().Description);
            By phoneNumber = By.CssSelector(MyGarageLocators.EditAccountDetails.PhoneNumber.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(btnSave);
            ClearField(FindElement(firstName));
            EnterText(firstName, toUpdate[MyGarageLocators.AccountSettings.FirstName]);
            ClearField(FindElement(lastName));
            ClearField(FindElement(lastName));
            EnterText(lastName, toUpdate[MyGarageLocators.AccountSettings.LastName]);
            ClearField(FindElement(postCode));
            EnterText(postCode, toUpdate[MyGarageLocators.AccountSettings.PostalCode]);
            ClearField(FindElement(phoneNumber));
            EnterText(phoneNumber, toUpdate[MyGarageLocators.AccountSettings.PhoneNumber]);
            ClickElement(FindElement(btnSave));
            By btnCloseUpdateSucceessFul = By.CssSelector(MyGarageLocators.EditAccountDetails.CloseUpdateSuccessful.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(btnCloseUpdateSucceessFul);
            WaitForElementClickable(btnCloseUpdateSucceessFul);
            ClickElement(FindElement(btnCloseUpdateSucceessFul));
            WaitForElementNotVisible(btnCloseUpdateSucceessFul);

        }

        public bool VerifyAccountDeatilsPageDoesntHaveUpdatePasswordForSocialAccounts()
        {
            By updatePasswrod = By.CssSelector(MyGarageLocators.AccountSettings.ChangPassword.GetAttribute<DescriptionAttribute>().Description);
            return !IsElementVisible(updatePasswrod);
        }

        public void UpdatePassword(string updatePassword, string oldPassword)
        {
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => EnterText(By.CssSelector(MyGarageLocators.EditAccountDetails.Oldpassword.GetAttribute<DescriptionAttribute>().Description), oldPassword));
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => EnterText(By.CssSelector(MyGarageLocators.EditAccountDetails.NewPassword.GetAttribute<DescriptionAttribute>().Description), updatePassword));
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => EnterText(By.CssSelector(MyGarageLocators.EditAccountDetails.ConfirmNewPassword.GetAttribute<DescriptionAttribute>().Description), updatePassword));
            if (viewport == Viewport.XS)
            {
                InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.EditAccountDetails.ConfirmNewPassword.GetAttribute<DescriptionAttribute>().Description)), 3));
            }
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => ClickElement(FindElement(By.CssSelector(MyGarageLocators.EditAccountDetails.UpdatePassword.GetAttribute<DescriptionAttribute>().Description)), 15));
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => WaitForElementVisible(By.CssSelector(MyGarageLocators.EditAccountDetails.PasswordUpdatedHeaderTxt.GetAttribute<DescriptionAttribute>().Description), 30));
            InteractOnIframe(By.CssSelector(MyGarageLocators.EditAccountDetails.iframe.GetAttribute<DescriptionAttribute>().Description),
                () => ClickElement(FindElement(By.CssSelector(MyGarageLocators.EditAccountDetails.ClosePasswordUpdateSuccessful.GetAttribute<DescriptionAttribute>().Description))));
            WaitForChangePasswordModalStatus(false);
        }

        public void ClickChangePassword()
        {
            By updatePasswrod = By.CssSelector(MyGarageLocators.AccountSettings.ChangPassword.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementClickable(updatePasswrod);
            ClickElement(FindElement(updatePasswrod));
            WaitForChangePasswordModalStatus();
        }

        public bool ValidateUpdatedPersonalDetails(IDictionary<MyGarageLocators.AccountSettings, string> toUpdate)
        {
            bool matchStatus = false;
            WaitForPageLoad(3);
            WaitForElementClickable(By.CssSelector(MyGarageLocators.AccountSettings.EidtPersonalDetails.GetAttribute<DescriptionAttribute>().Description));
            foreach (KeyValuePair<MyGarageLocators.AccountSettings, string> kvp in toUpdate)
            {
                matchStatus = true;
                string value = GetElementText(FindElement(By.CssSelector(kvp.Key.GetAttribute<DescriptionAttribute>().Description))).Trim().ToLower();
                Console.WriteLine(value);
                if (!value.Equals(kvp.Value.Trim().ToLower()))
                {
                    matchStatus = false;
                    break;
                }
            }
            return matchStatus;
        }

        public void ClickEditPersonalDetails()
        {
            By editPersonalDetails = By.CssSelector(MyGarageLocators.AccountSettings.EidtPersonalDetails.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementClickable(editPersonalDetails);
            ClickElement(FindElement(editPersonalDetails));
        }
        #endregion

        #region Saved Searches
        public virtual void ClickSavedSearchLink()
        {
            By SeeAllSavedSearchesLink = By.XPath(MyGarageLocators.CommonLocators.SeeAllSavedSearchLink.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(SeeAllSavedSearchesLink, 30);
            IWebElement savedSearchLink = FindElement(SeeAllSavedSearchesLink);
            ScrollTo(savedSearchLink);
            ClickElement(savedSearchLink);
        }
       
        public bool IsSavedSearchAvailable(string searchName)
        {
            try
            {
                return FindElements(By.CssSelector(MyGarageLocators.CommonLocators.SavedSearchItemTitle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.ToLower().Contains(searchName.ToLower())) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RedirectToSRPFromSavedSearch(string searchName)
        {
            IWebElement savedSearchElement = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.SavedSearchItemTitle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.ToLower().Contains(searchName.ToLower()));
            ClickElement(savedSearchElement);
            WaitForPageLoad(60);
        }

        public void ClickEditSavedSearchLink(string searchName)
        {
            IWebElement searchElement = FindElements(By.CssSelector(MyGarageLocators.CommonLocators.SavedSearchItemTitle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.ToLower().Contains(searchName.ToLower()));

            if (searchElement != null)
            {
                IWebElement editElement = searchElement.FindElement(By.XPath(MyGarageLocators.CommonLocators.SavedSearchEdit.GetAttribute<DescriptionAttribute>().Description));
                ClickElement(editElement);
                WaitForEditSaveSearchModalStatus();
            }
            else
            {
                throw new Exception(searchName + " search name doesn't exist in MyGarage list");
            }
        }

        public void WaitForEditSaveSearchModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.CommonLocators.EditSavedSearchModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.EditSavedSearchModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsEditSavedSearchDisplayed()
        {
            return IsElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.EditSavedSearchModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public void UpdateSelectedSavedSearch(string newSearchName)
        {
            IWebElement editSearchElement = FindElement(By.CssSelector(MyGarageLocators.CommonLocators.SavedSeachName.GetAttribute<DescriptionAttribute>().Description));
            ClearField(editSearchElement);
            editSearchElement.SendKeys(newSearchName);
            IWebElement saveChangesElement = FindElement(By.CssSelector(MyGarageLocators.CommonLocators.SavedSearchBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(saveChangesElement);
            WaitForEditSaveSearchModalStatus(false);
        }

        public void WaitForToasterMsgNotVisible(string selectorType)
        {
            By toasterMsgLocator = null;
            if (selectorType == ToasterSelector.SaveSearch.ToString())
            {
                toasterMsgLocator = By.CssSelector(MyGarageLocators.CommonLocators.ToasterMsg.GetAttribute<DescriptionAttribute>().Description);
            }
            else if (selectorType == ToasterSelector.PreferenceCenter.ToString())
            {
                toasterMsgLocator = By.CssSelector(MyGarageLocators.CommonLocators.PreferenceCentreToasterMsg.GetAttribute<DescriptionAttribute>().Description);
            }
            else
            {
                throw new ArgumentException("Unsupported selectorType");
            }
            WaitForElementNotVisible(toasterMsgLocator, 15);
        }

        public string GetToasterMessage(string selectorType)
        {
            try
            {
                By toasterLocator, toasterMsgLocator = null;
                if (selectorType == ToasterSelector.SaveSearch.ToString())
                {
                    toasterLocator = By.CssSelector(MyGarageLocators.CommonLocators.Toaster.GetAttribute<DescriptionAttribute>().Description);
                    toasterMsgLocator = By.CssSelector(MyGarageLocators.CommonLocators.ToasterMsg.GetAttribute<DescriptionAttribute>().Description);
                    WaitForElementVisible(toasterLocator, 30);
                    IWebElement toasterMessageElement = FindElement(toasterMsgLocator);
                    return GetElementText(toasterMessageElement);
                }
                else if (selectorType == ToasterSelector.PreferenceCenter.ToString())
                {
                    toasterLocator = By.CssSelector(MyGarageLocators.CommonLocators.PreferenceCentreToaster.GetAttribute<DescriptionAttribute>().Description);
                    toasterMsgLocator = By.CssSelector(MyGarageLocators.CommonLocators.PreferenceCentreToasterMsg.GetAttribute<DescriptionAttribute>().Description);
                    WaitForElementVisible(toasterLocator, 30);
                    IWebElement toasterMessageElement = FindElement(toasterMsgLocator);
                    return GetElementText(toasterMessageElement);
                }
                else if (selectorType == ToasterSelector.SavedVehicles.ToString())
                {
                    toasterLocator = By.CssSelector(MyGarageLocators.CommonLocators.Toaster.GetAttribute<DescriptionAttribute>().Description);
                    toasterMsgLocator = By.CssSelector(MyGarageLocators.CommonLocators.ToasterMsg.GetAttribute<DescriptionAttribute>().Description);
                    WaitForElementVisible(toasterLocator, 30);
                    string toasterMsg = GetElementText(FindElement(toasterMsgLocator));
                    WaitForElementNotVisible(toasterLocator);
                    return toasterMsg;
                }
                else
                {
                    throw new ArgumentException("Unsupported selectorType");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void DeleteSelectedSavedSearch()
        {
            IWebElement deleteSearchElement = FindElement(By.CssSelector(MyGarageLocators.CommonLocators.DeleteSavedSearchBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(deleteSearchElement);
            WaitForEditSaveSearchModalStatus(false);
        }

        public void ClickUndoLink()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.Toaster.GetAttribute<DescriptionAttribute>().Description));
                IWebElement undoLink = FindElement(By.CssSelector(MyGarageLocators.CommonLocators.UndoLink.GetAttribute<DescriptionAttribute>().Description));
                ClickElement(undoLink);
            }
            catch (Exception)
            {
                throw new Exception("Undo link has issue or undo link doen't display");
            }
        }

        public bool IsSavedSearchDisplayedForLeadFormSubsciption()
        {
            return IsElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.SavedSearchItem.GetAttribute<DescriptionAttribute>().Description));
        }
        #endregion

        #region Home
        public abstract void ClickShowAllPriceAlert();

        //Saved Vehicles, Price Alert Widget
        public bool IsSavedSubscribedVehicleDisplayedInWidget(string vdpTitle, bool isDisplayed = true)
        {
            var allSavedVehicleTitleElements = FindElements(By.CssSelector(MyGarageLocators.Home.AllSavedVehicleTitles.GetAttribute<DescriptionAttribute>().Description));
            if (!isDisplayed)
            {
                return allSavedVehicleTitleElements != null ? allSavedVehicleTitleElements.FirstOrDefault(x => vdpTitle.Contains(x.Text)) == null : true;
            }
            else
            {
                return allSavedVehicleTitleElements != null ? allSavedVehicleTitleElements.FirstOrDefault(x => vdpTitle.Contains(x.Text)) != null : false;
            }
        }

        public virtual void UnsubscribeUnSaveVehicleInWidget(string vdpTitle)
        {
            var allVdpWidgetCardElements = FindElements(By.CssSelector(MyGarageLocators.Home.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            int allVdpWidgetCardsCount = allVdpWidgetCardElements != null ? allVdpWidgetCardElements.Count : throw new Exception("Vdp card list found empty.");

            IWebElement vdpWidgetCardElement = allVdpWidgetCardElements.FirstOrDefault(x => vdpTitle.Contains(x.FindElement(By.CssSelector("div.name")).Text));
            IWebElement vdpWidgetUnsubscribeBtnElement = vdpWidgetCardElement != null ? vdpWidgetCardElement.FindElement(By.CssSelector("div.icon")) : throw new Exception("Can't find expected VDP in My Garage.");

            ScrollTo(vdpWidgetUnsubscribeBtnElement);
            ClickElement(vdpWidgetUnsubscribeBtnElement);
            WaitForElementVisible(By.CssSelector(MyGarageLocators.Home.SavedSubscribedVehicleWidgetRemoveBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.Home.SavedSubscribedVehicleWidgetRemoveBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitUntil(() => GetAllVdpCardsCountInWidget() < allVdpWidgetCardsCount);
        }

        public int GetAllVdpCardsCountInWidget()
        {
            var allVdpWidgetCardElements = FindElements(By.CssSelector(MyGarageLocators.Home.AllSavedVehicleCards.GetAttribute<DescriptionAttribute>().Description));
            return allVdpWidgetCardElements != null ? allVdpWidgetCardElements.Count : 0;
        }
        #endregion

        #region PreferenceCentre
        public void UpdatePreferenceCentreSettings(IDictionary<MyGarageLocators.PreferenceCentreSettings, bool> preferenceCentreSettings)
        {
            foreach (var pcs in preferenceCentreSettings)
            {
                IWebElement element = FindElement(By.CssSelector(pcs.Key.GetAttribute<DescriptionAttribute>().Description));
                if (IsCheckboxChecked(element) != pcs.Value)
                {
                    ClickElement(FindElement(By.CssSelector(pcs.Key.GetAttribute<DescriptionAttribute>().Description + " ~ span")));
                }
            }
        }
        public void ClickPreferenceCenterUpdateBtn()
        {
            IWebElement updateBtn = FindElement(By.CssSelector(MyGarageLocators.CommonLocators.PreferenceCentreUpdateBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(updateBtn);
        }
        public bool IsPreferenceCentreModalDisplayed()
        {
            return IsElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.UnsubscribeModal.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickPreferenceCenterUnsubscribeLink()
        {
            By unsubscribeLink = By.CssSelector(MyGarageLocators.CommonLocators.UnsubscribeLink.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementClickable(unsubscribeLink);
            ClickElement(FindElement(unsubscribeLink));
            WaitForUnsubscribeModalStatus(false);
        }

        public void WaitForUnsubscribeModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.CommonLocators.UnsubscribeModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.CommonLocators.UnsubscribeModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        #endregion

        #region MyVehicles
        public void CustomSelectByText(IWebElement dropdownElement, By dropdownContentLocator, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Dropdown selection text can not be null: ");
            }
            ScrollTo(dropdownElement);
            ClickElement(dropdownElement);
            WaitForElementVisible(dropdownContentLocator);

            var option = FindElement(dropdownContentLocator).FindElements(By.XPath(".//span[text()='" + text + "']")).FirstOrDefault();
            if (option == null)
            {
                throw new Exception("Dropdown does not contain selection text: " + text);
            }
            try
            {
                option.Click();
            }
            catch (Exception)
            {
                ScrollTo(option);
                option.Click();
            }
            WaitForMyGarageLoad();
        }

        public void CustomSelectFirstOption(IWebElement dropdownElement, By dropdownContentLocator)
        {
            ScrollTo(dropdownElement);
            ClickElement(dropdownElement);
            WaitForElementVisible(dropdownContentLocator);

            var option = FindElement(dropdownContentLocator).FindElements(By.CssSelector(".dropdown-options")).FirstOrDefault();
            if (option == null)
            {
                throw new Exception("Dropdown does not contain any selection option");
            }
            try
            {
                option.Click();
            }
            catch (Exception)
            {
                ScrollTo(option);
                option.Click();
            }
            WaitForMyGarageLoad();
        }

        private void OpenAddVehicleModal(bool isEmptyState = true)
        {
            if (!isEmptyState)
            {
                IWebElement element = FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVehicleCarrouselBtn.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(element);
                ClickElement(element);
                WaitForAddVehicalModal();
            }
            else
            {
                ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVehicleEmptyStateBtn.GetAttribute<DescriptionAttribute>().Description)));
                WaitForAddVehicalModal();
            }
        }

        public virtual void OpenAddVinModal()
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVinBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForAddVehicalModal();
        }

        public void WaitForAddVehicalModal(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.MyVehicles.AddVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.AddVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        private void WaitForDeleteVehicleModal(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.MyVehicles.DeleteVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.DeleteVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        private void WaitForEditVehicleModal(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.MyVehicles.EditVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.EditVehicleModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public void OpenEditVehicleModal()
        {
            IWebElement element = FindElement(By.CssSelector(MyGarageLocators.MyVehicles.EditVehicleLink.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForEditVehicleModal();
        }
        public void OpenDeleteVehicleModal()
        {
            IWebElement element = FindElement(By.CssSelector(MyGarageLocators.MyVehicles.DeleteVehicleLink.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForDeleteVehicleModal();
        }

        public virtual void EnterYmmDetails(MyVehicle myVehicle)
        {
            if (string.IsNullOrEmpty(myVehicle.Year) || string.IsNullOrEmpty(myVehicle.Make))
            {
                throw new Exception("Missing data to add my vehicle, Year: " + myVehicle.Year + " Make: " + myVehicle.Make);
            }
            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.YearDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.YearDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Year);
            WaitForMyGarageLoad();

            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.MakeDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.MakeDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Make);
            WaitForMyGarageLoad();

            if (string.IsNullOrEmpty(myVehicle.Model))
            {
                CustomSelectFirstOption(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownContent.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.ModelDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Model);
            }
            WaitForMyGarageLoad();

            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VinYmmNextBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForMyGarageLoad();
        }

        public void EnterVinDetail(MyVehicle myVehicle, AddVehicleModal addVehicleModal)
        {
            EnterText(By.CssSelector(MyGarageLocators.MyVehicles.VinInputTxt.GetAttribute<DescriptionAttribute>().Description), myVehicle.Vin);
            WaitForMyGarageLoad();
            if (viewport == Viewport.XS) { UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VinInputTxt.GetAttribute<DescriptionAttribute>().Description))); }

            switch (addVehicleModal)
            {
                case AddVehicleModal.AddNewVehicle:
                    ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VinYmmNextBtn.GetAttribute<DescriptionAttribute>().Description)));
                    break;
                case AddVehicleModal.AddVinToExistingVehicle:
                    ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.AddVinNextBtn.GetAttribute<DescriptionAttribute>().Description)));
                    break;
            }
            WaitForMyGarageLoad();
        }

        public virtual void EnterTrimMileageDetails(MyVehicle myVehicle, AddVehicleModal addVehicleModal)
        {
            CustomSelectFirstOption(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownContent.GetAttribute<DescriptionAttribute>().Description));
            WaitForMyGarageLoad();

            EnterText(By.CssSelector(MyGarageLocators.MyVehicles.KilometresInputTxt.GetAttribute<DescriptionAttribute>().Description), myVehicle.Kilometres);
            if (viewport == Viewport.XS) { UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.KilometresInputTxt.GetAttribute<DescriptionAttribute>().Description))); }

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
       
        public void SelectColor(bool chooseFirstColor = true)
        {
            WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.ColorsContainerDiv.GetAttribute<DescriptionAttribute>().Description));
            if (!chooseFirstColor)
            {
                ClickElement(FindElements(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleColors.GetAttribute<DescriptionAttribute>().Description)).LastOrDefault());
            }
            else
            {
                ClickElement(FindElements(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleColors.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault());
            }
        }

        public void SaveMyVehicle()
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.SaveVehicleBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForMyGarageLoad();
            WaitForPageLoad(30);
        }

        public void AddMyVehicle(MyVehicle myVehicle, AddVehicleModal addVehicleModal, bool isEmptyState)
        {
            OpenAddVehicleModal(isEmptyState);
            if (!string.IsNullOrEmpty(myVehicle.Vin))
            {
                EnterVinDetail(myVehicle, addVehicleModal);
            }
            else
            {
                EnterYmmDetails(myVehicle);
            }
            EnterTrimMileageDetails(myVehicle, addVehicleModal);
            SelectColor();
            SaveMyVehicle();
        }

        public void AddVinToMyVehicle(MyVehicle myVehicle, AddVehicleModal addVehicleModal)
        {
            if (string.IsNullOrEmpty(myVehicle.Vin))
            {
                throw new Exception("Missing data to add Vin to my vehicle, Vin: " + myVehicle.Vin);
            }
            OpenAddVinModal();
            EnterVinDetail(myVehicle, addVehicleModal);
            EnterTrimMileageDetails(myVehicle, addVehicleModal);
        }

        public void DeleteMyVehicleFromEditVehicle()
        {
            OpenDeleteVehicleModal();
            ClickOnDeleteBtn();
        }
        public void ClickOnDeleteBtn()
        {
            IWebElement deleteBtn = FindElement(By.CssSelector(MyGarageLocators.MyVehicles.DeleteVehicleBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(deleteBtn);
            WaitForMyGarageLoad();
            WaitForPageLoad(30);
        }
        public void ClickOnSaveVehicleOnEditModal()
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.SaveVehicleChangesBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForMyGarageLoad();
            WaitForPageLoad(30);
        }
        public void EditMyVehicle(MyVehicle myVehicle)
        {
            OpenEditVehicleModal();
            EnterTrimMileageColorDetails(myVehicle);
            ClickOnSaveVehicleOnEditModal();
        }
        public virtual void EnterTrimMileageColorDetails(MyVehicle myVehicle)
        {
            if (string.IsNullOrEmpty(myVehicle.Trim) || string.IsNullOrEmpty(myVehicle.Colour))
            {
                throw new Exception("Missing data to update my vehicle, Trim: " + myVehicle.Trim + " Colour: " + myVehicle.Colour);
            }
            CustomSelectByText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdown.GetAttribute<DescriptionAttribute>().Description)), By.CssSelector(MyGarageLocators.MyVehicles.TrimDropdownContent.GetAttribute<DescriptionAttribute>().Description), myVehicle.Trim);
            WaitForMyGarageLoad();

            EnterText(By.CssSelector(MyGarageLocators.MyVehicles.KilometresInputTxt.GetAttribute<DescriptionAttribute>().Description), myVehicle.Kilometres);

            SelectColourByName(myVehicle.Colour);
            WaitForMyGarageLoad();
        }

        public bool IsColourSelected(string colourName)
        {
            try
            {
                var colourTitleElement = FindElement(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleColorTitle.GetAttribute<DescriptionAttribute>().Description));
                string displayColour = colourTitleElement.Text.Split(':')[1].Trim().ToLower();
                bool isColourDisplayed = displayColour.Contains(colourName.ToLower());
                return isColourDisplayed;

            }
            catch (Exception)
            {
                throw new Exception("An error occured fetching selected color name");
            }
            
        }
        public virtual void SelectColourByName(string rgbColor)
        {
            WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.ColorsContainerDiv.GetAttribute<DescriptionAttribute>().Description));
            try
            {
                var colorSquares = FindElements(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleColors.GetAttribute<DescriptionAttribute>().Description));
                foreach(var colorSquare in colorSquares)
                {
                    string styleAttribute = colorSquare.GetAttribute("style");
                    if (styleAttribute.Equals($"background: {rgbColor};", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickElement(colorSquare);
                        return;
                    }
                }
                if (!IsColourSelected(rgbColor))
                {
                   throw new Exception ( $"Selected color '{rgbColor}' is not displayed besides vehicle colour");
                }
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Element with color name '{rgbColor}' not found.");
            }
            catch (Exception)
            {
                throw new Exception("An error occured selecting color from color picker for My Vehicle");
            }
        }
        public void ReloadMyVehicleOnError()
        {
            ReloadOnErrorDisplay(By.CssSelector(MyGarageLocators.MyVehicles.ErrorStateDiv.GetAttribute<DescriptionAttribute>().Description), 10);
            WaitForMyGarageLoad();
        }

        public bool IsAddMyVehicleSuccess()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.FilledStateDiv.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception)
            {
                ReloadMyVehicleOnError();
                return IsElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.FilledStateDiv.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsAddMyVehicleSuccessModalDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleSuccessModal.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }

        public void CloseAddVehicleSuccessModal()
        {
            if (IsElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleSuccessModal.GetAttribute<DescriptionAttribute>().Description)))
            {
                ClickElement(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
                WaitForElementNotVisible(By.CssSelector(MyGarageLocators.MyVehicles.MyVehicleSuccessModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsAddVehicleTabOnCarrouselDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.AddVehicleCarrouselBtn.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }

        public int GetMyVehiclesCount()
        {
            var vehicleTabsList = FindElements(By.CssSelector(MyGarageLocators.MyVehicles.VehicleTabs.GetAttribute<DescriptionAttribute>().Description));
            return CountElements(vehicleTabsList);
        }

        public bool IsMyVehicleEmptyStateDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.EmptyStateDiv.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception)
            {
                ReloadMyVehicleOnError();
                try
                {
                    WaitForElementVisible(By.CssSelector(MyGarageLocators.MyVehicles.EmptyStateDiv.GetAttribute<DescriptionAttribute>().Description));
                    return true;
                }
                catch (Exception) { return false; }
            }
        }
        
        public string GetVehicleTitle()
        {
            return GetElementText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VehicleTitle.GetAttribute<DescriptionAttribute>().Description))).ToLower();
        }

        public string GetVehicleTrimDetail()
        {
            return GetElementText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VehicleTrim.GetAttribute<DescriptionAttribute>().Description))).ToLower();
        }

        public string GetVehicleColorDetail()
        {
            return GetElementText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VehicleColor.GetAttribute<DescriptionAttribute>().Description))).ToLower();
        }

        public string GetVehicleMileageDetail()
        {
            string mileage = GetElementText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VehicleMileage.GetAttribute<DescriptionAttribute>().Description)));
            return Extensions.RemoveAllMatchingChar(mileage, new List<char>() { ',', ' ', 'k', 'K', 'm', 'M' });
        }

        public string GetVehicleVinDetail()
        {
            return GetElementText(FindElement(By.CssSelector(MyGarageLocators.MyVehicles.VehicleVin.GetAttribute<DescriptionAttribute>().Description))).ToLower();
        }
        #endregion
    }

    public class SsoLogin
    {
        public SsoAccountType accountType { get; set; }
        public string LocalAccountEmail { get; set; }
        public string LocalAccountPassword { get; set; }
        public string SocialAccountFBEmail { get; set; }
        public string SocialAccountFBPassword { get; set; }
        public string SocialAccountGoogleEmail { get; set; }
        public string SocialAccountGooglePassword { get; set; }
        public string SocialAccountAppleEmail { get; set; }
        public string SocialAccountApplePassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public dynamic InvalidUser { get; set; }
        public dynamic InvalidPassword { get; set; }
        public dynamic UpdatedFirstName { get; set; }
        public dynamic UpdatedLastName { get; set; }
        public dynamic UpdatedPostalCode { get; set; }
        public dynamic UpdatedPhoneNumber { get; set; }
        public dynamic UpdatedPassword { get; set; }

        public enum SsoAccountType
        {
            LocalAccount,
            SocialAccountFB,
            SocialAccountGoogle,
            SocialAccountApple
        }
    }

    public class Mailinator
    {
        public string MailinatorApiToken { get; set; }
        public string MailinatorPrivateDomainName { get; set; }
        public string MailinatorPrivateInboxName { get; set; }
        public string MailinatorPrivateInboxName_2 { get; set; }
    }

    public class SsoRegistration
    {
        public string NewSsoAccountEmail { get; set; }
        public string NewSsoAccountEmail_2 { get; set; }
        public string NewSsoAccountPassword { get; set; }
        public string NewSsoAccountFirstName { get; set; }
        public string NewSsoAccountLastName { get; set; }      
    }

    public class MyVehicle
    {
        public string Vin { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Kilometres { get; set; }
        public string Trim { get; set; }
        public string Colour { get; set; }
    }

    public enum MyGarageTabs
    {
        Home,
        MyVehicles,
        SavedVehicles,
        PriceAlerts,
        SavedSearches,
        ManageAds,
        AccountSettings,
        PreferenceCentre
    }

    public enum AddVehicleModal
    {
        AddNewVehicle,
        AddVinToExistingVehicle
    }

    public enum AzureTableProfileHeader
    {
        PdnEnabled,
        SavedSearchEnabled,
        SimilarSearchEnabled,
        NewsLettersEnabled,
        SurveysEnabled,
        NewProductsEnabled,
        SpecialOffersEnabled
    }

    public enum ToasterSelector
    {
        SaveSearch,
        PreferenceCenter,
        SavedVehicles
    }

    public enum AzureProfileTableSavedVehicleFlags
    {
        PdnEnabled,
        PdnLeadEnabled,
        SavedEnabled
    }
}
