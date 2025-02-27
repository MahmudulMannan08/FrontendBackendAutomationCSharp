using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MarketPlaceWeb.Pages.VDP
{
    public abstract class VDPAbstract : Page
    {
        protected VDPAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        #region Leads
        public string GetLeadAttribute(Enum enumValue, EmailLeadForm emailLeadForm)
        {
            switch (emailLeadForm.LeadForm)
            {
                case EmailLeadForm.EmailLeads.BookAnAppointment:
                case EmailLeadForm.EmailLeads.GeneralInquiry:
                case EmailLeadForm.EmailLeads.MakeAnOffer:
                case EmailLeadForm.EmailLeads.CSBadgeBookAnAppointment:
                case EmailLeadForm.EmailLeads.CSBadgeHomeTestDrive:
                case EmailLeadForm.EmailLeads.CSBadgeDelivery:
                    return enumValue.GetAttribute<DescriptionGeneralLeadAttribute>().DescriptionGeneralLead;
                case EmailLeadForm.EmailLeads.ViewMapLead:
                    return enumValue.GetAttribute<DescriptionViewMapLeadAttribute>().DescriptionViewMapLead;
                case EmailLeadForm.EmailLeads.GalleryLead:
                    return enumValue.GetAttribute<DescriptionGalleryLeadAttribute>().DescriptionGalleryLead;
                default:
                    return enumValue.GetAttribute<DescriptionAttribute>().Description;
            }
        }

        public void SendEmailLeadAbs(EmailLeadForm emailLeadForm, bool IsPriceAlertSubscribed = false)
        {
            OpenLeadAbs(emailLeadForm.LeadForm);
            if (emailLeadForm.Name != null)
            {
                EnterNameOnEmailLeadForm(emailLeadForm, emailLeadForm.Name);
            }
            EnterEmailOnEmailLeadForm(emailLeadForm, emailLeadForm.Email);
            if (emailLeadForm.PhoneNumber != null)
            {
                EnterPhoneNumberOnEmailLeadForm(emailLeadForm, emailLeadForm.PhoneNumber);
            }
            if (emailLeadForm.Message != null)
            {
                EnterMessageOnEmailLeadForm(emailLeadForm, emailLeadForm.Message);
            }
            if (emailLeadForm.MyOffer != null)
            {
                EnterMyOfferOnEmailLeadForm(emailLeadForm.MyOffer);
            }
            if (emailLeadForm.PreferredDate != null)
            {
                EnterPrefferedDateOnEmailLeadForm(emailLeadForm, emailLeadForm.PreferredDate);
            }
            if (emailLeadForm.TimeOfDay != null)
            {
                EnterTimeOfDayOnEmailLeadForm(emailLeadForm, emailLeadForm.TimeOfDay);
            }
            if (IsPriceAlertSubscribed)
            {
                SelectPriceAlertOnEmailLeadForm();
            }
            ClickSubmissionButtonOnLeadForm(emailLeadForm);
        }
       

        public abstract void OpenLeadAbs(EmailLeadForm.EmailLeads leadTab);

        
        public virtual bool VerifyMIQueryParams(Viewport viewport, Uri testURL)
        {
            string[] hrefs = new string[3]; // Array to store href values

            // Get href values (You may replace these with your existing method calls)
            hrefs[0] = GetElementAttribute(By.CssSelector(VDPLocators.CommonLocators.PaymentcalcLeadFormCTA.GetAttribute<DescriptionAttribute>().Description), "href");
            hrefs[1] = GetElementAttribute(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcPill.GetAttribute<DescriptionAttribute>().Description), "href");
            hrefs[2] = GetElementAttribute(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcWidgetButton.GetAttribute<DescriptionAttribute>().Description), "href");

            // Expected parameter values based on the test URL
            NameValueCollection expectedParams = HttpUtility.ParseQueryString(testURL.Query);
            string expectedPlatform = viewport == Viewport.XS ? "Mobile" : "Desktop";
            string expectedActiveUpsells = expectedParams["ursrc"];
            if (string.IsNullOrEmpty(expectedActiveUpsells) && !string.IsNullOrEmpty(expectedParams["orup"]))
            {                
                    expectedActiveUpsells = "organic";
             
            }
            string testURLPath = testURL.AbsolutePath;
            string adidDealerIdPattern = @"\/(\d{1,}_\d{1,})_(\d{1,})\/";
            Match match = Regex.Match(testURLPath, adidDealerIdPattern);
            if (!match.Success)
            {
                adidDealerIdPattern = @"/(\d{1,}_\w+)_([\w\d]+)/";

                match = Regex.Match(testURLPath, adidDealerIdPattern);

                if (!match.Success)
                    return false;
            }
            

            string adidDealerId = match.Groups[0].Value;
            string expectedAdid = match.Groups[1].Value;
            string expectedDealerId = match.Groups[2].Value.ToUpper();


            foreach (string href in hrefs)
            {
                // Parse href to extract query parameters
                NameValueCollection queryParams = HttpUtility.ParseQueryString(new Uri(href).Query);

                // Get the values of the required parameters
                string platform = queryParams["platform"];
                string activeUpsells = queryParams["activeUpsells"];
                string dealerId = queryParams["dealerId"];
                string adid = queryParams["adid"];

                // Check if all required parameters exist and have the expected values
                bool isValid = !string.IsNullOrEmpty(platform) &&
                               !string.IsNullOrEmpty(activeUpsells) &&
                               !string.IsNullOrEmpty(dealerId) &&
                               !string.IsNullOrEmpty(adid) &&
                               platform == expectedPlatform &&
                               activeUpsells == expectedActiveUpsells &&
                               dealerId == expectedDealerId &&
                               adid == expectedAdid;

                if (!isValid)
                {
                    return false;
                }
            }

            return true;
        }
      


        internal bool? VerifyICOWidgetrIsNotDisplayed()
        {
            return !IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.ICOWidget.GetAttribute<DescriptionAttribute>().Description));
        }

        internal bool? VerifyReserveItWidgetRankingWhenDealBuilderIsEnabled()
        {
            return (IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveItVDPWidget.GetAttribute<DescriptionAttribute>().Description))
                    && IsElementVisible(By.XPath("//vdp-reserve-it/preceding-sibling::vdp-condition-analysis"))
                    && IsElementVisible(By.XPath("//vdp-reserve-it/following-sibling::vdp-fuel-economy-v2")));
            
            //Using xpaths since checking ranking
        }

        public bool? CheckLeadFormAbs(EmailLeadForm.DealerType dealerType)
        {

            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description)));
            bool dealerinfoFooter = false;            
            switch (dealerType)
            {
                case EmailLeadForm.DealerType.Traditional:
                case EmailLeadForm.DealerType.Hybrid:
                    {
                        dealerinfoFooter = ((IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoFooterNonVirtual.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description)))
                            || IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.VisitWebsite.GetAttribute<DescriptionAttribute>().Description)));
                        break;
                    }                
                case EmailLeadForm.DealerType.Virtual:
                    {
                        dealerinfoFooter = (IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoFooterVirtual.GetAttribute<DescriptionAttribute>().Description))
                            && !IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description))
                            && !IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.VisitWebsite.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.VisitDealerWebsite.GetAttribute<DescriptionAttribute>().Description)));

                        break;
                    }
            }

            return (!IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadContainerTab.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoHeader.GetAttribute<DescriptionAttribute>().Description))
                && dealerinfoFooter);

        }

        public void EnterNameOnEmailLeadForm(EmailLeadForm emailLeadForm, string name)
        {
            By nameFieldLocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadNameField, emailLeadForm));
            ScrollTo(FindElement(nameFieldLocator));
            EnterText(nameFieldLocator, name);
        }

        public void EnterEmailOnEmailLeadForm(EmailLeadForm emailLeadForm, string email)
        {
            By emailFieldLocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadEmailField, emailLeadForm));
            ScrollTo(FindElement(emailFieldLocator));
            EnterText(emailFieldLocator, email);
        }

        public void EnterPhoneNumberOnEmailLeadForm(EmailLeadForm emailLeadForm, string phoneNumber)
        {
            By phoneNumberFieldLocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadPhoneNumberField, emailLeadForm));
            ScrollTo(FindElement(phoneNumberFieldLocator));
            EnterText(phoneNumberFieldLocator, phoneNumber);
        }

        internal virtual bool? CheckPaymentCalculatorOnVDPPills()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcPill.GetAttribute<DescriptionAttribute>().Description));
        }

        internal virtual bool? CheckPaymentCalculatorOnVDPLeadForms()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.PaymentcalcLeadFormCTA.GetAttribute<DescriptionAttribute>().Description));            
            bool isClickable = element != null && element.Displayed && element.Enabled;
            
            if (isClickable)
            {
                try
                {
                    element.Click();                                       
                    driver.Navigate().Back();
                }
                catch (Exception)
                {
                    // If the click throws an exception, consider it not clickable
                    isClickable = false;
                }
            }

            return isClickable;
        }



        public virtual bool? CheckPaymentCalculatorOnVDPWidget()
        {
            By MIWidget = By.CssSelector(VDPLocators.CommonLocators.PaymentCalcWidget.GetAttribute<DescriptionAttribute>().Description);           
            return IsElementVisible(MIWidget);
        }

        public void EnterMessageOnEmailLeadForm(EmailLeadForm emailLeadForm, string message)
        {
            By messageFieldLocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadMessageField, emailLeadForm));
            ScrollTo(FindElement(messageFieldLocator));
            EnterText(messageFieldLocator, message);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(messageFieldLocator));
            }
        }

        public void EnterMyOfferOnEmailLeadForm(string message)
        {
            By messageFieldLocator = By.CssSelector(VDPLocators.CommonLocators.MyOfferField.GetAttribute<DescriptionAttribute>().Description);
            ScrollTo(FindElement(messageFieldLocator));
            EnterText(messageFieldLocator, message);
        }

        public void ClickSubmissionButtonOnLeadForm(EmailLeadForm emailLeadForm)
        {
            IWebElement leadSubmissionButton = FindElement(By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadSendButton, emailLeadForm)));
            ScrollTo(leadSubmissionButton);
            ClickElement(leadSubmissionButton);
            WaitForLeadFormProgress(60);
        }

        public void WaitForLeadFormProgress(int timeOut = 10)
        {
            WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormProgress.GetAttribute<DescriptionAttribute>().Description), timeOut);
        }

        public bool IsEmailLeadFeedbackMsgDisplayed(EmailLeadForm emailLeadForm)
        {
            By LeadFeedbackMsgLocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadFeedbackMsg, emailLeadForm));
            try
            {
                WaitForElementVisible(LeadFeedbackMsgLocator);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool IsEmailLeadFormAvailable()
        {
            return FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description)).Displayed;
        }

        public string GetEmailLeadFeedbackMessage1()
        {
            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadFeedbackMessage1.GetAttribute<DescriptionAttribute>().Description));
            return FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadFeedbackMessage1.GetAttribute<DescriptionAttribute>().Description)).Text.Trim();
        }

        public string GetEmailLeadFeedbackMessage2()
        {
            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadFeedbackMessage2.GetAttribute<DescriptionAttribute>().Description));
            return FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadFeedbackMessage2.GetAttribute<DescriptionAttribute>().Description)).Text.Trim();
        }

        public void ClickOnOkBtnOnEmailLeadFeedbackDialog()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadFeedbackOKButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public string GetVDPTitle()
        {
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.HeroCardTitle.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void WaitForModalToOpen(By locator, bool isStatusOpen = true)
        {
            if (!isStatusOpen)
            {
                WaitUntil(() => !FindElement(locator).GetAttribute("class").Contains("open"));
            }
            else
            {
                WaitUntil(() => FindElement(locator).GetAttribute("class").Contains("open"));
            }
        }

        public void WaitForLeadTabToOpen(EmailLeadForm.EmailLeads leadTab)
        {
            string tabName = string.Empty;
            switch (leadTab)
            {
                case EmailLeadForm.EmailLeads.BookAnAppointment:
                    {
                        tabName = (language.ToString() == "EN") ? "BOOK AN APPOINTMENT" : "PRENDRE RENDEZ-VOUS";
                        break;
                    }
                case EmailLeadForm.EmailLeads.GeneralInquiry:
                    {
                        tabName = (language.ToString() == "EN") ? "GENERAL INQUIRY" : "REQUÊTE GÉNÉRALE";
                        break;
                    }
                case EmailLeadForm.EmailLeads.MakeAnOffer:
                    {
                        tabName = (language.ToString() == "EN") ? "MAKE AN OFFER" : "FAIRE UNE OFFRE";
                        break;
                    }
            }

            WaitUntil(() => FindElements(By.CssSelector(VDPLocators.CommonLocators.LeadContainerTab.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.Equals(tabName)).GetAttribute("class").Contains("active"));
        }

        public bool IsVdpLeadModalOpen(bool isStatusOpen = true)
        {
            if (!isStatusOpen)
            {
                try
                {
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description), false);
                    return true;
                }
                catch (Exception) { return false; }
            }
            else
            {
                try
                {
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description));
                    return true;
                }
                catch (Exception) { return false; }
            }
        }

        /// <summary>
        /// This method enters date in the lead date field
        /// </summary>
        /// <param name="emailLeadForm">EmailLeadForm enum containing the lead form type</param>
        /// <param name="preferredDate">Preferred date string in 'M/d/yyyy' format</param>
        public void EnterPrefferedDateOnEmailLeadForm(EmailLeadForm emailLeadForm, string preferredDate)
        {
            IWebElement preferredDateElement = FindElement(By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadPreferredDate, emailLeadForm)));
            ScrollTo(preferredDateElement);
            ClearField(preferredDateElement);
            preferredDateElement.SendKeys(preferredDate);
        }

        /// <summary>
        /// This method selects Time of day combo element in the lead form
        /// </summary>
        /// <param name="emailLeadForm">EmailLeadForm enum containing the lead form type</param>
        /// <param name="timeOfDayValue">Combo element to select by value</param>
        public void EnterTimeOfDayOnEmailLeadForm(EmailLeadForm emailLeadForm, string timeOfDayValue)
        {
            By timeOfDaylocator = By.CssSelector(GetLeadAttribute(VDPLocators.CommonLocators.LeadTimeOfDayDropdown, emailLeadForm));
            ScrollTo(FindElement(timeOfDaylocator));
            SelectByValue(timeOfDaylocator, timeOfDayValue);
        }

        public void SelectPriceAlertOnEmailLeadForm()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertCheckbox.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            if (!IsCheckboxChecked(element))
            {
                ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertCheckbox.GetAttribute<DescriptionAttribute>().Description + " + span")));
            }
        }
        #endregion

        #region BuyersHub
        public void ClickOnLearnMoreBtnOnVdpFlyout()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.LearnMoreButton
                .GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
        }

        public void ClickOnLearnMoreBtnOnVdpFlyoutModal()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.LearnMoreButtonModal.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);
        }

        public string GetFlyoutTitle()
        {
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFlyoutTitle.GetAttribute<DescriptionAttribute>().Description)));
        }

        public string GetBuyersHubTitle()
        {
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.BuyersHubTitle.GetAttribute<DescriptionAttribute>().Description)));
        }
        #endregion

        #region TryBeforeYouBuy
        public virtual bool IsTbybButtonEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return FindElement(By.CssSelector(VDPLocators.CommonLocators.TryBeforeYouBuyButton.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("disabled");
            }
            return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.CommonLocators.TryBeforeYouBuyButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public virtual void ClickTbybButton()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.TryBeforeYouBuyButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);
        }

        public void ClickEmailDialogueCloseBtn()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.VdpLeadModalCloseButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickRequestTextModalCloseBtn()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.VdpTextModalCloseButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsVdpTextModalOpen(bool isStatusOpen = true)
        {
            if (!isStatusOpen)
            {
                try
                {
                    WaitForElementNotVisible(By.CssSelector(VDPLocators.XSLocators.VdpTextModal.GetAttribute<DescriptionAttribute>().Description));
                    return true;
                }
                catch (Exception) { return false; }
            }
            else
            {
                try
                {
                    WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.VdpTextModal.GetAttribute<DescriptionAttribute>().Description));
                    return true;
                }
                catch (Exception) { return false; }
            }
        }

        public void ClickContactSellerOption(ContactSellerButtons contactSellerButtons)
        {
            if (!FindElement(By.CssSelector(VDPLocators.CommonLocators.VdpFooterWidget.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("show-contact-options"))
            {
                ClickStickyButton(StickyButtons.ContactSeller);
                WaitForContactSellerOptionToDisplay();
            }

            switch (contactSellerButtons)
            {
                case ContactSellerButtons.Email:
                    WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerEmailButton.GetAttribute<DescriptionAttribute>().Description));
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.ContactSellerEmailButton.GetAttribute<DescriptionAttribute>().Description)));
                    break;
                case ContactSellerButtons.Text:
                    WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerTextButton.GetAttribute<DescriptionAttribute>().Description));
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.ContactSellerTextButton.GetAttribute<DescriptionAttribute>().Description)));
                    break;
            }
        }

        public bool IsContactSellerOptionsDisplayed(ContactSellerButtons contactSellerButtons, bool isDisplayed = true)
        {
            switch (contactSellerButtons)
            {
                case ContactSellerButtons.Email:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerEmailButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    try
                    {
                        WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerEmailButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    catch (Exception) { return false; }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerEmailButton.GetAttribute<DescriptionAttribute>().Description));
                case ContactSellerButtons.Text:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerTextButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    try
                    {
                        WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerTextButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    catch (Exception) { return false; }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerTextButton.GetAttribute<DescriptionAttribute>().Description));
                case ContactSellerButtons.Call:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerCallButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    try
                    {
                        WaitForElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerCallButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    catch (Exception) { return false; }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerCallButton.GetAttribute<DescriptionAttribute>().Description));
                default:
                    return false;
            }
        }

        public void WaitForContactSellerOptionToDisplay(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitUntil(() => !FindElement(By.CssSelector(VDPLocators.CommonLocators.VdpFooterWidget.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("show-contact-options"));
            }
            else
            {
                WaitUntil(() => FindElement(By.CssSelector(VDPLocators.CommonLocators.VdpFooterWidget.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("show-contact-options"));
            }
        }

        public void ClickStickyButton(StickyButtons stickyButtons)
        {
            switch (stickyButtons)
            {
                case StickyButtons.TryBeforeYouBuy:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.TbybStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForPageLoad(90);
                    break;
                case StickyButtons.EmailSeller:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    break;
                case StickyButtons.Text:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.TextStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    break;
                case StickyButtons.ContactSeller:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.ContactSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForContactSellerOptionToDisplay();
                    break;
                case StickyButtons.Email:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailPermanentStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    break;
            }
        }

        internal string VerifyPhoneNumberAssitedTextOnTheLeadForm()
        {
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.PhoneAssistiveText.GetAttribute<DescriptionAttribute>().Description))).Trim();
        }

        internal string GetEmailLeadTermsAndConditionMessage()
        {
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.TAndC.GetAttribute<DescriptionAttribute>().Description))).Trim();
            
        }

        internal bool? VerifyDealerDetailsOnTheLeadForm()
        {
            return (IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoHeader.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoHeaderDealerName.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.LeadFormDealerInfoHeaderDealerContact.GetAttribute<DescriptionAttribute>().Description)));

        }

        internal bool? VerifyLeadFormtabs()
        {
            throw new NotImplementedException();
        }

        public bool IsStickyButtonDisplayed(StickyButtons stickyButtons, bool isDisplayed = true)
        {
            switch (stickyButtons)
            {
                case StickyButtons.TryBeforeYouBuy:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyStickyButton.GetAttribute<DescriptionAttribute>().Description));
                case StickyButtons.EmailSeller:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.EmailSellerStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.EmailSellerStickyButton.GetAttribute<DescriptionAttribute>().Description));
                case StickyButtons.Text:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.TextStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.TextStickyButton.GetAttribute<DescriptionAttribute>().Description));
                case StickyButtons.Call:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.CallStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.CallStickyButton.GetAttribute<DescriptionAttribute>().Description));
                case StickyButtons.ContactSeller:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.ContactSellerStickyButton.GetAttribute<DescriptionAttribute>().Description));
                case StickyButtons.Email:
                    if (!isDisplayed)
                    {
                        return !IsElementVisible(By.CssSelector(VDPLocators.XSLocators.EmailPermanentStickyButton.GetAttribute<DescriptionAttribute>().Description));
                    }
                    return IsElementVisible(By.CssSelector(VDPLocators.XSLocators.EmailPermanentStickyButton.GetAttribute<DescriptionAttribute>().Description));
                default:
                    return false;
            }
        }
         

        public bool IsStickyButtonEnabled(StickyButtons stickyButtons, bool isEnabled = true)
        {
            switch (stickyButtons)
            {
                case StickyButtons.TryBeforeYouBuy:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                case StickyButtons.EmailSeller:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                case StickyButtons.Text:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.TextStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.TextStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                case StickyButtons.Call:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.CallStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.CallStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                case StickyButtons.ContactSeller:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.ContactSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.ContactSellerStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                case StickyButtons.Email:
                    if (!isEnabled)
                    {
                        return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailPermanentStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                    }
                    return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailPermanentStickyButton.GetAttribute<DescriptionAttribute>().Description)));
                default:
                    return false;
            }
        }

        public abstract void ClickTenDayMoneyBackGuaranteeText();
        #endregion

        #region ReserveIt
        public void ClickReserveVehicleButton(string reserveVehicleBtnLabel)
        {
            try
            {
                if (!GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description))).Contains(reserveVehicleBtnLabel))  //Reload page if reserve it button is not available (i.e. after payment cancellation)
                {
                    RefreshPage();
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
                }
            }
            catch (Exception)  //Sometimes Reserve Vehicle feature does not load on VDP
            {
                RefreshPage();
                WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
            }

            IWebElement reserveVehicleButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(reserveVehicleButton);
            ClickElement(reserveVehicleButton);
        }

        public void ClickOnlineReservationBadge()
        {
            IWebElement onlineReservationCSBadge = FindElement(By.CssSelector(VDPLocators.CommonLocators.OnlineReservationBadge.GetAttribute<DescriptionAttribute>().Description), 30);
            ScrollTo(onlineReservationCSBadge);
            ClickElement(onlineReservationCSBadge);
        }

        public void WaitForReserveVehicleModal(bool isModalOpen = true)
        {
            if (isModalOpen)
            {
                WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleModalOpen.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleModalOpen.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsReserveVehicleModalAvailable(bool isOpen = true)
        {
            if (!isOpen)
            {
                return !IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleModalOpen.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleModalOpen.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public void EnterBuyerInfo(ReserveVehicleForm reserveVehicleForm)
        {
            EnterText(By.CssSelector(VDPLocators.CommonLocators.BuyerName.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.Name);
            EnterText(By.CssSelector(VDPLocators.CommonLocators.BuyerPhone.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.Phone);
        }

        public void ClickBuyerInfoNameField()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.BuyerName.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickBuyerInfoPhoneField()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.BuyerPhone.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsBuyerInfoNameFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.NameErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsGalleryModalOpen()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.ModalContainer.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsBuyerInfoPhoneFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.BuyerPhoneErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickContinueToPaymentButton()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.ContinueToPaymentButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickBuyerInfoCancelButton()
        {
            ClickElement(FindElements(By.CssSelector(VDPLocators.CommonLocators.BuyerInfoCancelButton.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Displayed));
        }

        public void EnterDepositInfo(ReserveVehicleForm reserveVehicleForm)
        {
            EnterText(By.CssSelector(VDPLocators.CommonLocators.DepositEmail.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.Email);
            EnterText(By.CssSelector(VDPLocators.CommonLocators.DepositPostalCode.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.PostalCode);
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCCNumber.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterTextWithDelay(By.CssSelector(VDPLocators.CommonLocators.DepositCCNumber.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.CCNumber));
            EnterText(By.CssSelector(VDPLocators.CommonLocators.DepositCCName.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.CCName);
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCCExpiry.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.DepositCCExpiry.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.CCExpiry));
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCVV.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.DepositCVV.GetAttribute<DescriptionAttribute>().Description), reserveVehicleForm.CVV));
        }

        public void ClickDepositEmail()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositEmail.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsDepositEmailFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositEmailErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public abstract void ClickVideoPlayIconInGallery();

        public void ClickDepositPostalCode()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositPostalCode.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsDepositPostalCodeFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositPostalCodeErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsDepositCCNumberFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositCCNumberErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsDepositCCNameFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.NameErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsDepositCCExpiryFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositCCExpiryErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsDepositCVVFieldErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositCVVErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsDepositTermsCheckboxErrorDisplayed()
        {
            return IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.DepositTermsErrorMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsReserveVehicleButtonEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description)));
            }
            else
            {
                return IsElementEnabled(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description)));
            }
        }

        public void ClickDepositCCNumber()
        {
            try
            {
                InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCCNumber.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositCCNumber.GetAttribute<DescriptionAttribute>().Description))));
            }
            catch (System.Exception)
            {
                ClickElementJS(VDPLocators.CommonLocators.DepositCCNumber.GetAttribute<JsLocatorAttribute>().JsLocator);
            }
        }

        public void ClickDepositCCName()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositCCName.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickDepositCCExpiry()
        {
            try
            {
                InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCCExpiry.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositCCExpiry.GetAttribute<DescriptionAttribute>().Description))));
            }
            catch (System.Exception)
            {
                ClickElementJS(VDPLocators.CommonLocators.DepositCCExpiry.GetAttribute<JsLocatorAttribute>().JsLocator);
            }
        }

        public void ClickDepositCVV()
        {
            try
            {
                InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DepositCVV.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositCVV.GetAttribute<DescriptionAttribute>().Description))));
            }
            catch (System.Exception)
            {
                ClickElementJS(VDPLocators.CommonLocators.DepositCVV.GetAttribute<JsLocatorAttribute>().JsLocator);
            }
        }

        public abstract void CheckDepositTerms(bool toBeChecked = true);

        public void ClickDepositReserveVehcileButton()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.LoadElement.GetAttribute<DescriptionAttribute>().Description), 30);
        }

        public bool VerifyDepositSuccess()
        {
            return FindElements(By.CssSelector(VDPLocators.CommonLocators.DepositSuccessForm.GetAttribute<DescriptionAttribute>().Description)) != null ? true : false;
        }

        public void ClickDepositFinishButton()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DepositFinishButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickDepositBackButton()
        {
            ClickElement(FindElements(By.CssSelector(VDPLocators.CommonLocators.DepositBackButton.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Displayed));
        }

        public void ClickTermsConditionsLink()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.TermsConditionsLink.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsReserveItTCDisplayed(string reserveVehicleTCHeader)
        {
            return GetElementText(FindElements(By.CssSelector(VDPLocators.CommonLocators.ReserveItTCHeader.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault()).Equals(reserveVehicleTCHeader);
        }

        public void ClickBackButtonOnReserveItTCPage()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveItTCBackButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsDepositPageDisplayed()
        {
            return FindElements(By.CssSelector(VDPLocators.CommonLocators.DepositReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description)) != null ? true : false;
        }

        public bool IsBuyerInfoPageDisplayed()
        {
            return FindElements(By.CssSelector(VDPLocators.CommonLocators.ContinueToPaymentButton.GetAttribute<DescriptionAttribute>().Description)) != null ? true : false;
        }

        public bool IsJoinWaitListBuyerInfoPageVisible(bool isVisible = true)
        {
            if (!isVisible)
            {
                return !IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.JoinWaitListBuyerInfoForm.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.JoinWaitListBuyerInfoForm.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsJoinWaitListButtonDisplayed(string joinWaitListBtnLabel, bool isTbyb = false)
        {
            //Reserve It
            if (!isTbyb)
            {
                try
                {
                    RefreshPage();  //Join Wait List Success page does not sync if not refreshed
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);  //Sometimes ReserveIt widget does not load instantly
                    if (!GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description))).Contains(joinWaitListBtnLabel))
                    {
                        RefreshPage();
                        WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
                    }
                }
                catch (Exception)
                {
                    RefreshPage();
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
                }
                IWebElement reserveVehicleButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(reserveVehicleButton);

                WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
                return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description))).Equals(joinWaitListBtnLabel);
            }

            //Try Before You Buy
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);

            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description))).Equals(joinWaitListBtnLabel);
        }

        public void ClickJoinWaitListButton()
        {
            IWebElement joinWaitListButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(joinWaitListButton);
            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description), 30);
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.ReserveVehicleButton.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void EnterWaitListInfo(JoinWaitListForm joinWaitListForm)
        {
            EnterText(By.CssSelector(VDPLocators.CommonLocators.WaitListName.GetAttribute<DescriptionAttribute>().Description), joinWaitListForm.Name, 20);
            EnterText(By.CssSelector(VDPLocators.CommonLocators.WaitListEmail.GetAttribute<DescriptionAttribute>().Description), joinWaitListForm.Email);
            EnterText(By.CssSelector(VDPLocators.CommonLocators.WaitListPhone.GetAttribute<DescriptionAttribute>().Description), joinWaitListForm.Phone);
        }

        public void ClickWaitListSubmitButton()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.WaitListSubmitButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.LoadElement.GetAttribute<DescriptionAttribute>().Description), 30);
        }

        public bool IsJoinWaitListSucceed(string joinWaitListSuccessMessage)
        {
            if (FindElements(By.CssSelector(VDPLocators.CommonLocators.DepositSuccessForm.GetAttribute<DescriptionAttribute>().Description), 30) != null)
            {
                return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.JoinWaitListSuccessTitle.GetAttribute<DescriptionAttribute>().Description))).Contains(joinWaitListSuccessMessage);
            }
            return false;
        }

        public void ClickPrivacyPolicyLink()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.WaitListPrivacyLink.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsJoinWaitListPrivacyDisplayed(string privacyUrl)
        {
            SwitchTabOrWindow(() => driver.Url.Contains(privacyUrl));
            return IsInCurrentUrl(privacyUrl);
        }
        #endregion

        #region Spin360
        public bool IsSpinIconDisplayedOnVdp()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(VDPLocators.LargeLocators.MainImage.GetAttribute<DescriptionAttribute>().Description));
                return IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.Spin360Icon.GetAttribute<DescriptionAttribute>().Description));
            }
            catch (Exception)
            {
                RefreshPage();
                return IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.Spin360Icon.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        public bool IsSpinWidgetDisplayedOnVdp()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.SpinWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickSpinIconOnVdp()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.LargeLocators.Spin360Icon.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(element);
            WaitForSpinModalToOpen();
        }
        public bool IsSeeInsideButtonDisplayed()
        {
            WaitForElementVisible(By.CssSelector(VDPLocators.LargeLocators.SpinSeeInsideButton.GetAttribute<DescriptionAttribute>().Description), 5);
            return IsElementVisible(By.CssSelector(VDPLocators.LargeLocators.SpinSeeInsideButton.GetAttribute<DescriptionAttribute>().Description));
        }
        public string GetSeeInsideButtonText()
        {
            string script = "return window.getComputedStyle(document.querySelector('span[data-cy=\"change-view-action-text\"]'),'::before').getPropertyValue('content')";
            return GetValueByJS(script);
        }
        public void CloseSpinModal()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.LargeLocators.SpinCloseButton.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(element);
        }
        public void WaitForSpinModalToOpen(bool isModalOpen = true)
        {
            if (isModalOpen)
            {
                WaitForElementVisible(By.CssSelector(VDPLocators.LargeLocators.SpinModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementNotVisible(By.CssSelector(VDPLocators.LargeLocators.SpinModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        public bool IsSpinModalClosed()
        {
            try
            {
                WaitForSpinModalToOpen(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ClickWidgetSpinIcon()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.LargeLocators.SpinView360Button.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForSpinModalToOpen();
        }
        public string GetSpinWidgetTextDisplayedOnVdp()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.LargeLocators.SpinWidgetText.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return element.Text;
        }
        #endregion

        #region Payment2
        /// <summary>
        /// This method adds Payment configuration details on Payment calculator 
        /// </summary>
        /// <param name="paymentCalculator">paymentCalculator object accepts Payment configuration item values </param>
        public void FillPaymentConfiguration(PaymentCalculator paymentCalculator)
        {
            IWebElement paymentConfiguration = FindElement(By.CssSelector(VDPLocators.CommonLocators.PaymentConfigurationPnl.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(paymentConfiguration);

            if (paymentCalculator.PaymentConfiguration.PaymentFrequency != null)
                SelectByText(By.CssSelector(VDPLocators.CommonLocators.PaymentFrequencyLst.GetAttribute<DescriptionAttribute>().Description), paymentCalculator.PaymentConfiguration.PaymentFrequency);

            if (paymentCalculator.PaymentConfiguration.FinanceTerm != null)
                SelectByText(By.CssSelector(VDPLocators.CommonLocators.FinanceTermsLst.GetAttribute<DescriptionAttribute>().Description), paymentCalculator.PaymentConfiguration.FinanceTerm);

            if (paymentCalculator.PaymentConfiguration.Downpayment != null)
            {
                IWebElement downPaymentField = FindElement(By.CssSelector(VDPLocators.CommonLocators.DownPaymentTxt.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(downPaymentField);
                downPaymentField.Clear();
                downPaymentField.SendKeys(paymentCalculator.PaymentConfiguration.Downpayment);
                downPaymentField.SendKeys(Keys.Tab);
            }
            if (paymentCalculator.PaymentConfiguration.TradeIn != null)
            {
                IWebElement tradeInField = FindElement(By.CssSelector(VDPLocators.CommonLocators.TradeInTxt.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(tradeInField);
                tradeInField.Clear();
                tradeInField.SendKeys(paymentCalculator.PaymentConfiguration.TradeIn);
                tradeInField.SendKeys(Keys.Tab);
            }
            if (paymentCalculator.PaymentConfiguration.SalesTax)
            {
                IWebElement salesTax = FindElement(By.CssSelector(VDPLocators.CommonLocators.SalesTaxChk.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(salesTax);
                if (!IsCheckboxChecked(salesTax))
                    ClickElement(salesTax);
            }

        }
        /// <summary>
        /// This method gets Payment configuarion details
        /// </summary>
        /// <returns></returns>
        public PaymentConfiguration GetPaymentConfiguation()
        {
            var paymentConfiguration = new PaymentConfiguration
            {
                PaymentFrequency = new SelectElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.PaymentFrequencyLst.GetAttribute<DescriptionAttribute>().Description))).SelectedOption.Text.Trim(),
                FinanceTerm = new SelectElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.FinanceTermsLst.GetAttribute<DescriptionAttribute>().Description))).SelectedOption.Text.Trim(),
                Downpayment = FindElement(By.CssSelector(VDPLocators.CommonLocators.DownPaymentTxt.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("value"),
                TradeIn = FindElement(By.CssSelector(VDPLocators.CommonLocators.TradeInTxt.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("value"),
                SalesTax = FindElement(By.CssSelector(VDPLocators.CommonLocators.SalesTaxChk.GetAttribute<DescriptionAttribute>().Description)).Selected

            };
            return paymentConfiguration;
        }
        /// <summary>
        /// This method scrolls to pricing breakdown panel
        /// </summary>
        /// <param name="isExpand">Pass true to expand Pricing breadkwon panel</param>
        /// <param name="isModal">Pass true to apply on modal popup</param>
        public abstract void SetPricingBreakdown(bool isExpand, bool isModal);
        /// <summary>
        /// This method expand and collapse pricing breakdown clicking on chevronImage
        /// </summary>
        /// <param name="isExpand">Pass true to expand</param>
        /// <param name="isModal">Pass true to apply on modal popup</param>
        public void TogglePricingBreakdown(bool isExpand, bool isModal)
        {
            IWebElement chevronImage;
            if (isModal)
            {
                chevronImage = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalPricingBreakdownChevronImg.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                chevronImage = FindElement(By.CssSelector(VDPLocators.CommonLocators.PricingBreakdownChevronImg.GetAttribute<DescriptionAttribute>().Description));
            }
            bool isChevronUpArrow = chevronImage.GetAttribute("src").Contains("chevron-up-vdp.svg");
            if (isExpand && !isChevronUpArrow)
            {
                chevronImage.Click();
            }
            else if (!isExpand && isChevronUpArrow)
            {
                chevronImage.Click();
            }
            Wait(1);
        }
        /// <summary>
        /// This method gets pricing breadkdown details 
        /// </summary>
        /// <param name="isFinanaceFeeEnable">Pass true to get finance fee details</param>
        /// <param name="isDefaultValue">Pass true to get default downpayment and trade-in value</param>
        /// <returns></returns>
        public PricingBreakdown GetPricingBreakdown(bool isFinanaceFeeEnable, bool isDefaultValue)
        {

            var pricingBreakdown = new PricingBreakdown
            {
                DealerPriceLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerPriceLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                DealerPrice = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerPriceValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                MSRPLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.MsrpLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                MSRP = FindElement(By.CssSelector(VDPLocators.CommonLocators.MsrpValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                InterestChargeLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.InterestChargeLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                InterestCharge = FindElement(By.CssSelector(VDPLocators.CommonLocators.InterestChargeValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                SalesTaxesLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.SalesTaxesLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                TotalObligationLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.TotalObligationLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                TotalObligation = FindElement(By.CssSelector(VDPLocators.CommonLocators.TotalObligationValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                YourEstimateLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.YourEstimateLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                YourEstimate = FindElement(By.CssSelector(VDPLocators.CommonLocators.YourEstimateValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim()
            };
            if (isFinanaceFeeEnable)
            {
                pricingBreakdown.FinanceFeeLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.FinanceFeeLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
                pricingBreakdown.FinanceFee = FindElement(By.CssSelector(VDPLocators.CommonLocators.FinanceFeeValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim();
            }
            if (pricingBreakdown.SalesTaxesLabel != null)
            {
                pricingBreakdown.FederalLuxuryTaxLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.LuxuryTaxLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
                pricingBreakdown.FederalLuxuryTax = FindElement(By.CssSelector(VDPLocators.CommonLocators.LuxuryTaxValue.GetAttribute<DescriptionAttribute>().Description))?.Text;
            }
            if (!isDefaultValue)
            {
                pricingBreakdown.DownPaymentLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.DownPaymentLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
                pricingBreakdown.DownPayment = FindElement(By.CssSelector(VDPLocators.CommonLocators.DownPaymentValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim();
                pricingBreakdown.TradeInLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.TradeInLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
                pricingBreakdown.TradeIn = FindElement(By.CssSelector(VDPLocators.CommonLocators.TradeInValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim();

            }
            return pricingBreakdown;

        }
        /// <summary>
        /// This method cliks on Inquire now button
        /// </summary>
        public void ClickOnPayment2Inquire()
        {
            IWebElement inquireNowPnl = FindElement(By.CssSelector(VDPLocators.CommonLocators.InquireNowPnl.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(inquireNowPnl);
            IWebElement inquireNowBtn = FindElement(By.CssSelector(VDPLocators.CommonLocators.InquireNowBtn.GetAttribute<DescriptionAttribute>().Description));
            inquireNowBtn.Click();
            WaitUntilDialogDisplayed(By.CssSelector(VDPLocators.CommonLocators.ModalDialogOpen.GetAttribute<DescriptionAttribute>().Description), 5);
        }
        public abstract PaymentLeadForm GetPaymentModalLeadFormContent();
        /// <summary>
        /// This function submit Payment 2 Lead information 
        /// </summary>
        /// <param name="leadForm">Pass leadform information</param>
        public void SubmitPayment2Inquiry(EmailLeadForm leadForm)
        {
            if (leadForm != null)
            {
                if (leadForm.Name != null)
                {

                    IWebElement nameField = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalNameTxt.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(nameField);
                    nameField.Clear();
                    nameField.SendKeys(leadForm.Name);
                }
                if (leadForm.Email != null)
                {

                    IWebElement emailField = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalEmailTxt.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(emailField);
                    emailField.Clear();
                    emailField.SendKeys(leadForm.Email);
                }
                if (leadForm.PhoneNumber != null)
                {
                    IWebElement phoneNumberField = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalPhoneNumberTxt.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(phoneNumberField);
                    phoneNumberField.Clear();
                    phoneNumberField.SendKeys(leadForm.PhoneNumber);
                }
                if (leadForm.Message != null)
                {
                    IWebElement messageField = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalMessageTxt.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(messageField);
                    messageField.Clear();
                    messageField.SendKeys(leadForm.Message);
                }
                IWebElement submitBtn = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalSubmitBtn.GetAttribute<DescriptionAttribute>().Description));
                submitBtn.Click();
            }
        }
        /// <summary>
        /// This method gets ThankYou modal popup information
        /// </summary>
        /// <returns></returns>
        public PaymentThankYou GetPaymentModalThankYouContent()
        {
            WaitUntilDialogDisplayed(By.CssSelector(VDPLocators.CommonLocators.ModalDialogOpen.GetAttribute<DescriptionAttribute>().Description), 2);
            var paymentThankYou = new PaymentThankYou
            {
                Heading = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalThankYouHeading.GetAttribute<DescriptionAttribute>().Description))?.Text,
                Message = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalThankYouMsg.GetAttribute<DescriptionAttribute>().Description))?.Text,
                FinishButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFinishButton.GetAttribute<DescriptionAttribute>().Description))?.Text
            };
            return paymentThankYou;
        }
        /// <summary>
        /// This method clicks on finish button in ThankYou modal popup
        /// </summary>
        public void ClickOnPaymentModalThankYouFinishBtn()
        {
            var element = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFinishButton.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFinishButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitUntilDialogClosed(By.CssSelector(VDPLocators.CommonLocators.ModalDialogOpen.GetAttribute<DescriptionAttribute>().Description), 2);
        }
        /// <summary>
        /// This method gets payment tab name
        /// </summary>
        /// <returns></returns>
        public string GetPaymentTabName()
        {
            var element = FindElement(By.CssSelector(VDPLocators.CommonLocators.FinanceTab.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return element.Text;
        }
        public string GetFinanceNotNeededMsg()
        {
            return FindElement(By.CssSelector(VDPLocators.CommonLocators.FinanceNotNeeded.GetAttribute<DescriptionAttribute>().Description))?.Text;
        }
        /// <summary>
        /// This method gets inquireinfo, desclaimer text and Inquire now button text from bottom panel of calculator
        /// </summary>
        /// <returns></returns>
        public PaymentBottomPanel GetPaymentBottomPanel()
        {
            var paymentBottomPanel = new PaymentBottomPanel
            {
                InquireInfo = FindElement(By.CssSelector(VDPLocators.CommonLocators.InquireInfo.GetAttribute<DescriptionAttribute>().Description))?.Text,
                InquireNow = FindElement(By.CssSelector(VDPLocators.CommonLocators.InquireNowBtn.GetAttribute<DescriptionAttribute>().Description))?.Text,
                Disclaimer = FindElement(By.CssSelector(VDPLocators.CommonLocators.Disclaimer.GetAttribute<DescriptionAttribute>().Description))?.Text,
            };
            return paymentBottomPanel;
        }
        #endregion

        #region VdpPriceAlert
        public bool GetPriceAlertBtnStatus(bool isSubscribed = true)
        {
            if (!isSubscribed)
            {
                try
                {
                    WaitUntil(() => !FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("saved"), 30);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    WaitUntil(() => FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("saved"), 30);  //Browserstack is slow sometimes
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void ClickPriceAlertBtn()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForPriceAlertModalDisplayed();
        }

        public void WaitForPriceAlertModalDisplayed()
        {
            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.PriceAlertModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public void WaitForPriceAlertSuccessModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.PriceAlertSuccessModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.PriceAlertSuccessModal.GetAttribute<DescriptionAttribute>().Description), 30);
            }
        }

        public void EnterEmailPriceAlert(string email)
        {
            EnterText(By.CssSelector(VDPLocators.CommonLocators.PriceAlertEmailTxt.GetAttribute<DescriptionAttribute>().Description), email);
        }

        public void ClickPriceAlertSubscribeBtn()
        {
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertEmailTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertSubscribeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPriceAlertSuccessModalStatus();
        }

        public bool IsPriceAlertSuccessDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.PriceAlertSuccessHeader.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClosePriceAlertSuccessModal()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertSuccessModalCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPriceAlertSuccessModalStatus(false);
        }

        public bool IsRedirectedToCorrectVdpFromMyGarage(string adId)
        {
            string urlWithoutQuery = new Uri(driver.Url).GetLeftPart(UriPartial.Path);
            return urlWithoutQuery.ToUpperInvariant().Contains(adId.ToUpperInvariant());
        }

        public void UnsubscribeVdpPriceAlert()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.PriceAlertBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
        }
        #endregion

        #region VdpSaveVehicle
        public void ClickSaveVehicleBtn()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.SaveVehicleBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
        }

        public string GetToasterMsg()
        {
            try
            {
                By toasterMsgLocator = By.CssSelector(VDPLocators.CommonLocators.ToasterMsg.GetAttribute<DescriptionAttribute>().Description);
                WaitForElementVisible(toasterMsgLocator);
                string toasterMsg = GetElementText(FindElement(toasterMsgLocator));
                WaitForElementNotVisible(toasterMsgLocator);
                return toasterMsg;
            }
            catch (Exception) { return string.Empty; }
        }

        public bool GetSaveVehicleBtnStatus(bool isSaved = true)
        {
            if (!isSaved)
            {
                try
                {
                    WaitUntil(() => !FindElement(By.CssSelector(VDPLocators.CommonLocators.SaveVehicleBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("saved"));
                    return true;
                }
                catch (Exception) { return false; }
            }
            else
            {
                try
                {
                    WaitUntil(() => FindElement(By.CssSelector(VDPLocators.CommonLocators.SaveVehicleBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("saved"));
                    return true;
                }
                catch (Exception) { return false; }
            }
        }
        #endregion

        #region PreQualification
        public void ClickPreQualGetStartedBtn()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.PreQualGetStartedButton
               .GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);

        }
        public bool IsConfirmatoryTitleDisplayed()
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PreQualConfirmatoryTitle.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.PreQualConfirmatoryTitle.GetAttribute<IframeLocatorAttribute>().IframeLocator)));
            return true;
        }
        public bool IsPersonalInfoDisplayed()
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => IsElementAvailable(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator)));
            return true;
        }
        public void FillPersonalInfo(PersonalInformation personalInformation)
        {

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoFirstName.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => EnterText(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoFirstName.GetAttribute<DescriptionAttribute>().Description), personalInformation.FirstName, 30), 30);

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoLastName.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => EnterText(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoLastName.GetAttribute<DescriptionAttribute>().Description), personalInformation.LastName));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoEmail.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoEmail.GetAttribute<DescriptionAttribute>().Description), personalInformation.Email));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoPhoneNumber.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.PersonalInfoPhoneNumber.GetAttribute<DescriptionAttribute>().Description), personalInformation.PhoneNumber));
        }
        public void ClickPreQualNextBtn()
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PreQualNextButton.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.PreQualNextButton.GetAttribute<DescriptionAttribute>().Description))));

        }
        public void FillAppSummary()
        {

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.AppSummaryCkBox.GetAttribute<IframeLocatorAttribute>().IframeLocator),

                              () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.AppSummaryCkBox.GetAttribute<DescriptionAttribute>().Description))));
        }
        public void FillAddressInfo(AddressInformation addressInfo)
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.AddressLine1.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => EnterText(By.CssSelector(VDPLocators.CommonLocators.AddressLine1.GetAttribute<DescriptionAttribute>().Description), addressInfo.AddressLine1));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.AddressLine2.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                              () => EnterText(By.CssSelector(VDPLocators.CommonLocators.AddressLine2.GetAttribute<DescriptionAttribute>().Description), addressInfo.AddressLine2));


            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.City.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.City.GetAttribute<DescriptionAttribute>().Description), addressInfo.City));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.City.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.City.GetAttribute<DescriptionAttribute>().Description), addressInfo.City));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.Province.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.Province.GetAttribute<DescriptionAttribute>().Description))));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.ProvinceOption.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                           () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.ProvinceOption.GetAttribute<DescriptionAttribute>().Description))));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.PostalCode.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.PostalCode.GetAttribute<DescriptionAttribute>().Description), addressInfo.PostalCode));

        }
        public void SelectMonth()
        {

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.Month.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                           () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.Month.GetAttribute<DescriptionAttribute>().Description))));
        }

        public void SelectEmploymentStatus()
        {

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.EmploymentStatus.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                           () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmploymentStatus.GetAttribute<DescriptionAttribute>().Description))));
        }

        public void FillIncomeInfo()
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.GrossIncome.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.GrossIncome.GetAttribute<DescriptionAttribute>().Description), "3000"));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.IncomeFrequency.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                         () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.IncomeFrequency.GetAttribute<DescriptionAttribute>().Description))));
        }

        public void FillExpenses(Expenses expenses)
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.HousingMonthlyPayments.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.HousingMonthlyPayments.GetAttribute<DescriptionAttribute>().Description), expenses.HousingPayments));
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.VehicleMonthlyPayments.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                             () => EnterText(By.CssSelector(VDPLocators.CommonLocators.VehicleMonthlyPayments.GetAttribute<DescriptionAttribute>().Description), expenses.VehiclePayments));
        }
        public void FillCreditScoreInfo()
        {
            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.IncomeFrequency.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                        () => WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.CreditScore.GetAttribute<DescriptionAttribute>().Description)));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.IncomeFrequency.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                         () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.CreditScore.GetAttribute<DescriptionAttribute>().Description))));

            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DateofBirthBtn.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                         () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DateofBirthBtn.GetAttribute<DescriptionAttribute>().Description))));


            InteractOnIframe(By.CssSelector(VDPLocators.CommonLocators.DOBDate.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                      () => ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.DOBDate.GetAttribute<DescriptionAttribute>().Description))));

        }
        #endregion

        public abstract string VerifyYoutubeVideoIsPlaying();

        public string VerifyCorrectYoutubeIDLinkIsDisplayedOnVDP()
        {
            return FindElement(By.CssSelector(VDPLocators.CommonLocators.YoutubeIframe.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("src");
        }

        public abstract bool IsViewMapDisplayed();

        public abstract void ClickOnViewMapButton();

        public string GetDealerAddressOnViewMap()
        {
            WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description));
            return GetElementText(FindElement(By.CssSelector(VDPLocators.CommonLocators.ViewMapDealerAddress.GetAttribute<DescriptionAttribute>().Description)));
        }

        #region DealerInfo
        public bool IsDealerInfoDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoWidget.GetAttribute<DescriptionAttribute>().Description));
        }
        public string GetDealerInfoWidgetHeader()
        {
            WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoHeader.GetAttribute<DescriptionAttribute>().Description));
            IWebElement dealerInfoWidgetHeader =  FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerInfoHeader.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(dealerInfoWidgetHeader);
            return GetElementText(dealerInfoWidgetHeader);
        }
        public bool IsDealerInfoLogoDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoLogo.GetAttribute<DescriptionAttribute>().Description));
        }
        public string GetDealerInfoName()
        {
            By dealerInfoNameLocator = By.CssSelector(VDPLocators.CommonLocators.DealerInfoName.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(dealerInfoNameLocator);
            string dealerFullNameString = GetElementText(FindElement(dealerInfoNameLocator));
            return GetSubstringBeforeCharacter(dealerFullNameString, '\r');
        }
        public virtual bool IsDealerInfoShowmapButtonDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoShowMapBtn.GetAttribute<DescriptionAttribute>().Description));
        }
        public void CloseViewMap()
        {
            By viewMapCloseBtnLocator = By.CssSelector(VDPLocators.CommonLocators.ViewMapModalCloseButton.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(viewMapCloseBtnLocator);
            ClickElement(FindElement(viewMapCloseBtnLocator));
            WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description),false);
        }
        public virtual void ClickOnShowmapButton()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerInfoShowMapBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description));
        }
        public virtual bool IsDealerInfoViewAllInventoryButtonDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoViewAllInventoryBtn.GetAttribute<DescriptionAttribute>().Description));
        }
        public virtual void ClickOnViewAllInventoryButton()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerInfoViewAllInventoryBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
        }
        public bool IsDealershipPageMatch(string dealershipURLRoute)
        {
            WaitForPageLoad(60);
            return IsInCurrentUrl(dealershipURLRoute);
        }
        public bool IsDealerInfoVisitWebsiteLinkClickable()
        {
            IWebElement websiteLink = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerInfoVisitWebsiteLink.GetAttribute<DescriptionAttribute>().Description));
            return websiteLink.Enabled && websiteLink.Displayed;
        }
        public void ClickOnDealerInfoVisitWebsiteLink()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealerInfoVisitWebsiteLink.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForPageLoad(60);
        }
        public bool IsDealerInfoVisitWebsiteLinkUrlMatch(string visitWebsiteUrl)
        {
            SwitchTabOrWindow(() => driver.Url.Contains(visitWebsiteUrl));
            bool isUrlSame = IsInCurrentUrl(visitWebsiteUrl);
            SwitchToBaseWindow();
            return isUrlSame;
        }
        public bool IsDealerInfoSeeReviewsLinkClickable()
        {
            IWebElement seeReviewLink = FindElement(By.CssSelector(VDPLocators.CommonLocators.DealeInfoSeeReviewLink.GetAttribute<DescriptionAttribute>().Description));
            return seeReviewLink.Enabled && seeReviewLink.Displayed;
        }
        public bool IsDealerInfoGoogleReviewDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealeInfoGoogleRating.GetAttribute<DescriptionAttribute>().Description));
        }
        public int GetDealerInfoUSPItemsCount()
        {
            var uspItems = FindElements(By.CssSelector(VDPLocators.CommonLocators.DealeInfoUSPItems.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(uspItems.FirstOrDefault());
            return uspItems.Count;
        }
        public virtual bool IsDealerInfoAwardWinnerLogoDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DealerInfoAwardWinnerLogo.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsBrandingVideoImageThumbDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoThumb.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsOTLEventImageThumbDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.OltEventImgThumb.GetAttribute<DescriptionAttribute>().Description));
        }
        public bool IsDefaultImageThumbDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.DefaultImageThumb.GetAttribute<DescriptionAttribute>().Description));
        }
        public void CloseBrandingVideo()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForBradingVideoToOpen(false);
        }
        public bool IsBandingVideoModalDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoModal.GetAttribute<DescriptionAttribute>().Description));
        }
        public void WaitForBradingVideoToOpen(bool isModalOpen = true)
        {
            if (isModalOpen)
            {
                WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementNotVisible(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        public void ClickOnBrandingVideo()
        {
            IWebElement brandingVideoThumbElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.BrandingVideoThumb.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(brandingVideoThumbElement);
            try
            {
                ClickElement(brandingVideoThumbElement);
            }
            catch (Exception)
            {
                ClickElementJS(VDPLocators.CommonLocators.BrandingVideoThumb.GetAttribute<DescriptionAttribute>().Description);
            }
            WaitForBradingVideoToOpen();
        }
        #endregion
    }

    public class ReserveVehicleForm : Page
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string CCNumber { get; set; }
        public string CCName { get; set; }
        public string CCExpiry { get; set; }
        public string CVV { get; set; }
    }

    public class JoinWaitListForm : Page
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class EmailLeadForm : Page
    {
        public EmailLeads LeadForm { get; set; }
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public bool TradeIn { get; set; }
        public bool GetPriceAlert { get; set; }
        public string MyOffer { get; set; }
        public string PreferredDate { get; set; }
        public string TimeOfDay { get; set; }
        public enum EmailLeads
        {
            BookAnAppointment,
            GeneralInquiry,
            MakeAnOffer,
            ViewMapLead,
            GalleryLead,
            CSBadgeBookAnAppointment,
            CSBadgeHomeTestDrive,
            CSBadgeDelivery
        }

        public enum DealerType
        {
            Traditional,
            Hybrid,
            Virtual            
        }

    }

    public class PaymentCalculator
    {
        public Type PaymentType { get; set; }
        public PaymentConfiguration PaymentConfiguration { get; set; }
        public enum Type
        {
            Finance,
            Cash
        }
    }

    public class PricingBreakdown
    {
        public string DealerPriceLabel { get; set; }
        public string DealerPrice { get; set; }
        public string MSRPLabel { get; set; }
        public string MSRP { get; set; }
        public string FinanceFeeLabel { get; set; }
        public string FinanceFee { get; set; }
        public string InterestChargeLabel { get; set; }
        public string InterestCharge { get; set; }
        public string SalesTaxesLabel { get; set; }
        public string SalesTaxes { get; set; }
        public string DownPaymentLabel { get; set; }
        public string DownPayment { get; set; }
        public string TradeInLabel { get; set; }
        public string TradeIn { get; set; }
        public string TotalObligationLabel { get; set; }
        public string TotalObligation { get; set; }
        public string YourEstimateLabel { get; set; }
        public string YourEstimate { get; set; }
        public string YourEstimateFrequency { get; set; }
        public string FinanceTerm { get; set; }
        public string FederalLuxuryTaxLabel { get; set; }
        public string FederalLuxuryTax { get; set; }

    }

    public class PaymentConfiguration
    {
        public string PaymentFrequency { get; set; }
        public string FinanceTerm { get; set; }
        public string Downpayment { get; set; }
        public string TradeIn { get; set; }
        public bool SalesTax { get; set; }
    }

    public class PaymentLeadForm
    {
        public PricingBreakdown PricingBreakdown { get; set; }
        public string SummaryDisclaimer { get; set; }
        public string GetPriceAlert { get; set; }
        public string FormDisclaimer { get; set; }
        public string NameLabel { get; set; }
        public string EmailLabel { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string SubmitButton { get; set; }
    }

    public class PaymentThankYou
    {
        public string Heading { get; set; }
        public string Message { get; set; }
        public string FinishButton { get; set; }
    }

    public class PaymentBottomPanel
    {
        public string InquireInfo { get; set; }
        public string InquireNow { get; set; }
        public string Disclaimer { get; set; }
    }

    public class Gallery
    {
        public string VideoPlayIcon { get; set; }
        public string MoviePlayer { get; set; }
        public string YoutubeLink { get; set; }
        public string GalleryCounter { get; set; }
        public string ModalContainer { get; set; }
        public string GalleryPhotoStripe { get; set; }
        public string NextButton { get; set; }
        public string PreviousButton { get; set; }
        public string PlayPauseButton { get; set; }
        public string CloseGalleryModalButton { get; set; }
    }

    public enum StickyButtons
    {
        TryBeforeYouBuy,
        EmailSeller,
        Text,
        Call,
        ContactSeller,
        Email
    }

    public enum ContactSellerButtons
    {
        Email,
        Text,
        Call
    }

    public class PersonalInformation
    {
        public string Dob;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class AddressInformation
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

    }

    public class Expenses
    {
        public string HousingPayments { get; set; }
        public string VehiclePayments { get; set; }

    }
}