using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;

namespace MarketPlaceWeb.Pages.VDP
{
    public class VDPMain : Page
    {
        VDPAbstract vDPPage;

        public VDPMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    vDPPage = new VDPLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    vDPPage = new VDPXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    vDPPage = new VDPSmall(driver, viewport, language);
                    break;
            }
        }

        public void GoToVDP(string baseURL, string adId, string shortUrlEndpoint = "go/5-")
        {
            url = new Uri(baseURL + shortUrlEndpoint + adId);
            Open();
            if (viewport == Viewport.XS)
            {
                WaitForSplashPage();
            }
            string urlWithoutQuery = new Uri(driver.Url).GetLeftPart(UriPartial.Path);
            if (!urlWithoutQuery.ToUpperInvariant().Contains(adId.ToUpperInvariant()))
            {
                throw new Exception("AD " + baseURL + shortUrlEndpoint + adId + " is not available");
            }
        }

        #region TryBeforeYouBuy
        public bool IsTbybButtonEnabled(bool isEnabled = true)
        {
            return vDPPage.IsTbybButtonEnabled(isEnabled);
        }

        public void ClickTbybButton()
        {
            vDPPage.ClickTbybButton();
        }

        public void ClickEmailDialogueCloseBtn()
        {
            vDPPage.ClickEmailDialogueCloseBtn();
        }

        public void ClickRequestTextModalCloseBtn()
        {
            vDPPage.ClickRequestTextModalCloseBtn();
        }

        public bool IsVdpLeadModalOpen(bool isStatusOpen = true)
        {
            return vDPPage.IsVdpLeadModalOpen(isStatusOpen);
        }

        public bool IsVdpTextModalOpen(bool isStatusOpen = true)
        {
            return vDPPage.IsVdpTextModalOpen(isStatusOpen);
        }

        public bool IsContactSellerOptionsDisplayed(ContactSellerButtons contactSellerButtons, bool isDisplayed = true)
        {
            return vDPPage.IsContactSellerOptionsDisplayed(contactSellerButtons, isDisplayed);
        }

        public void ClickContactSellerOption(ContactSellerButtons contactSellerButtons)
        {
            vDPPage.ClickContactSellerOption(contactSellerButtons);
        }
          

        public void ClickStickyButton(StickyButtons stickyButtons)
        {
            vDPPage.ClickStickyButton(stickyButtons);
        }

        public bool IsStickyButtonDisplayed(StickyButtons stickyButtons, bool isEnabled = true)
        {
            return vDPPage.IsStickyButtonDisplayed(stickyButtons, isEnabled);
        }

        public bool? VerifyMIQueryParams(Viewport viewport, Uri testURL) => vDPPage.VerifyMIQueryParams(viewport, testURL);        

        public bool? VerifyICOWidgetrIsNotDisplayed() => vDPPage.VerifyICOWidgetrIsNotDisplayed();      
        
        public bool? VerifyReserveItWidgetRankingWhenDealBuilderIsEnabled() => vDPPage.VerifyReserveItWidgetRankingWhenDealBuilderIsEnabled(); 
        

        public bool IsStickyButtonEnabled(StickyButtons stickyButtons, bool isEnabled = true)
        {
            return vDPPage.IsStickyButtonEnabled(stickyButtons, isEnabled);
        }
        #endregion

        #region ReserveIt
        public void ClickReserveVehicleButton(string reserveVehicleBtnLabel)
        {
            vDPPage.ClickReserveVehicleButton(reserveVehicleBtnLabel);
            vDPPage.WaitForReserveVehicleModal();
        }

        public void ClickOnlineReservationBadge()
        {
            vDPPage.ClickOnlineReservationBadge();
            vDPPage.WaitForReserveVehicleModal();
        }

        public void EnterBuyerInfo(ReserveVehicleForm reserveVehicleForm)
        {
            vDPPage.EnterBuyerInfo(reserveVehicleForm);
        }

        public bool? CheckPaymentCalculatorOnVDPWidget() => vDPPage.CheckPaymentCalculatorOnVDPWidget();        

        public bool? CheckPaymentCalculatorOnVDPLeadForms() => vDPPage.CheckPaymentCalculatorOnVDPLeadForms();
      

        public bool? CheckPaymentCalculatorOnVDPPills() => vDPPage.CheckPaymentCalculatorOnVDPPills();


        public void ClickBuyerInfoNameField()
        {
            vDPPage.ClickBuyerInfoNameField();
        }

        public void ClickBuyerInfoPhoneField()
        {
            vDPPage.ClickBuyerInfoPhoneField();
        }

        public bool IsBuyerInfoNameFieldErrorDisplayed()
        {
            return vDPPage.IsBuyerInfoNameFieldErrorDisplayed();
        }

        public bool IsBuyerInfoPhoneFieldErrorDisplayed()
        {
            return vDPPage.IsBuyerInfoPhoneFieldErrorDisplayed();
        }

        public bool IsDepositEmailFieldErrorDisplayed()
        {
            return vDPPage.IsDepositEmailFieldErrorDisplayed();
        }

        public bool IsDepositPostalCodeFieldErrorDisplayed()
        {
            return vDPPage.IsDepositPostalCodeFieldErrorDisplayed();
        }

        public bool IsDepositCCNumberFieldErrorDisplayed()
        {
            return vDPPage.IsDepositCCNumberFieldErrorDisplayed();
        }

        public bool IsDepositCCNameFieldErrorDisplayed()
        {
            return vDPPage.IsDepositCCNameFieldErrorDisplayed();
        }

        public bool IsDepositCCExpiryFieldErrorDisplayed()
        {
            return vDPPage.IsDepositCCExpiryFieldErrorDisplayed();
        }

        public bool IsDepositCVVFieldErrorDisplayed()
        {
            return vDPPage.IsDepositCVVFieldErrorDisplayed();
        }

        public bool IsDepositTermsCheckboxErrorDisplayed()
        {
            return vDPPage.IsDepositTermsCheckboxErrorDisplayed();
        }

        public bool IsReserveVehicleButtonEnabled(bool isEnabled = true)
        {
            return vDPPage.IsReserveVehicleButtonEnabled(isEnabled);
        }

        public void ClickContinueToPaymentButton()
        {
            vDPPage.ClickContinueToPaymentButton();
        }

        public void ClickBuyerInfoCancelButton()
        {
            vDPPage.ClickBuyerInfoCancelButton();
            vDPPage.WaitForReserveVehicleModal(false);
        }

        public bool IsReserveVehicleModalAvailable(bool isOpen = true)
        {
            return vDPPage.IsReserveVehicleModalAvailable(isOpen);
        }

        public void EnterDepositInfo(ReserveVehicleForm reserveVehicleForm)
        {
            vDPPage.EnterDepositInfo(reserveVehicleForm);
        }

        public void ClickDepositEmail()
        {
            vDPPage.ClickDepositEmail();
        }

        public void ClickDepositPostalCode()
        {
            vDPPage.ClickDepositPostalCode();
        }

        public void ClickDepositCCNumber()
        {
            vDPPage.ClickDepositCCNumber();
        }

        public void ClickDepositCCName()
        {
            vDPPage.ClickDepositCCName();
        }

        public void ClickDepositCCExpiry()
        {
            vDPPage.ClickDepositCCExpiry();
        }

        public void ClickDepositCVV()
        {
            vDPPage.ClickDepositCVV();
        }

        public void CheckDepositTerms(bool toBeChecked = true)
        {
            vDPPage.CheckDepositTerms(toBeChecked);
        }

        public void ClickDepositReserveVehcileButton()
        {
            vDPPage.ClickDepositReserveVehcileButton();
        }

        public void ClickDepositBackButton()
        {
            vDPPage.ClickDepositBackButton();
        }

        public bool VerifyDepositSuccess()
        {
            return vDPPage.VerifyDepositSuccess();
        }

        public void ClickDepositFinishButton()
        {
            vDPPage.ClickDepositFinishButton();
            vDPPage.WaitForReserveVehicleModal(false);
            WaitForPageLoad(90);
        }

        public void ClickTermsConditionsLink()
        {
            vDPPage.ClickTermsConditionsLink();
        }

        public bool IsReserveItTCDisplayed(string reserveVehicleTCHeader)
        {
            return vDPPage.IsReserveItTCDisplayed(reserveVehicleTCHeader);
        }

        public void ClickBackButtonOnReserveItTCPage()
        {
            vDPPage.ClickBackButtonOnReserveItTCPage();
        }

        public bool IsDepositPageDisplayed()
        {
            return vDPPage.IsDepositPageDisplayed();
        }

        public bool IsBuyerInfoPageDisplayed()
        {
            return vDPPage.IsBuyerInfoPageDisplayed();
        }

        public bool IsJoinWaitListBuyerInfoPageVisible(bool isVisible = true)
        {
            return vDPPage.IsJoinWaitListBuyerInfoPageVisible(isVisible);
        }

        public bool IsJoinWaitListButtonDisplayed(string joinWaitListBtnLabel, bool isTbyb = false)
        {
            return vDPPage.IsJoinWaitListButtonDisplayed(joinWaitListBtnLabel, isTbyb);
        }

        public void ClickJoinWaitListButton()
        {
            vDPPage.ClickJoinWaitListButton();
            vDPPage.WaitForReserveVehicleModal();
        }

        public void EnterWaitListInfo(JoinWaitListForm joinWaitListForm)
        {
            vDPPage.EnterWaitListInfo(joinWaitListForm);
        }

        public void ClickWaitListSubmitButton()
        {
            vDPPage.ClickWaitListSubmitButton();
        }

        public bool IsJoinWaitListSucceed(string joinWaitListSuccessMessage)
        {
            return vDPPage.IsJoinWaitListSucceed(joinWaitListSuccessMessage);
        }

        public void ClickJoinWaitListFinishButton()
        {
            vDPPage.ClickDepositFinishButton();
            vDPPage.WaitForReserveVehicleModal(false);
            WaitForPageLoad(30);
        }

        public void ClickPrivacyPolicyLink()
        {
            vDPPage.ClickPrivacyPolicyLink();
        }

        public bool IsJoinWaitListPrivacyDisplayed(string privacyUrl)
        {
            return vDPPage.IsJoinWaitListPrivacyDisplayed(privacyUrl);
        }
        #endregion

        #region Leads
        public void SubmitLeadForm(EmailLeadForm emailLeadForm, bool IsPriceAlertSubscribed = false)
        {
            vDPPage.SendEmailLeadAbs(emailLeadForm, IsPriceAlertSubscribed);
        }

        public bool? CheckLeadForm(EmailLeadForm.DealerType dealerType)
        {
            return vDPPage.CheckLeadFormAbs(dealerType);            
        }
        

        public bool IsEmailLeadFeedbackMsgDisplayed(EmailLeadForm emailLeadForm)
        {
            return vDPPage.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm);
        }

        public string GetEmailLeadFeedbackMessage1()
        {
            return vDPPage.GetEmailLeadFeedbackMessage1();
        }

        public string GetEmailLeadFeedbackMessage2()
        {
            return vDPPage.GetEmailLeadFeedbackMessage2();

        }

        public void ClickOnOkBtnOnEmailLeadFeedbackDialog()
        {
            vDPPage.ClickOnOkBtnOnEmailLeadFeedbackDialog();
        }

        public bool IsEmailLeadFormAvailable()
        {
            return vDPPage.IsEmailLeadFormAvailable();
        }
        #endregion

        #region BuyersHub
        public void ClickOnLearnMoreBtnOnVdpFlyout()
        {
            vDPPage.ClickOnLearnMoreBtnOnVdpFlyout();
        }

        public void ClickOnLearnMoreBtnOnVdpFlyoutModal()
        {
            vDPPage.ClickOnLearnMoreBtnOnVdpFlyoutModal();
        }

        public string GetFlyoutTitle()
        {
            return vDPPage.GetFlyoutTitle();
        }

        public string GetBuyersHubTitle()
        {
            return vDPPage.GetBuyersHubTitle();
        }

        public void ClickTenDayMoneyBackGuaranteeText()
        {
            vDPPage.ClickTenDayMoneyBackGuaranteeText();
        }
        #endregion

        #region Spin360
        public bool IsSpinIconDisplayedOnVdp()
        {
            return vDPPage.IsSpinIconDisplayedOnVdp();
        }
        public bool IsSpinWidgetDisplayedOnVdp()
        {
            return vDPPage.IsSpinWidgetDisplayedOnVdp();
        }
        public void ClickSpinIconOnVdp()
        {
            vDPPage.ClickSpinIconOnVdp();
        }
        public bool IsSeeInsideButtonDisplayed()
        {
            return vDPPage.IsSeeInsideButtonDisplayed();
        }
        public string GetSeeInsideButtonText()
        {
            return vDPPage.GetSeeInsideButtonText();
        }
        public void CloseSpinModal()
        {
            vDPPage.CloseSpinModal();
        }
        public bool IsSpinModalClosed()
        {
            return vDPPage.IsSpinModalClosed();
        }
        public void ClickWidgetSpinIcon()
        {
            vDPPage.ClickWidgetSpinIcon();
        }
        public string GetSpinWidgetTextDisplayedOnVdp()
        {
            return vDPPage.GetSpinWidgetTextDisplayedOnVdp();
        }
        #endregion

        #region Payment2
        public void FillPaymentConfiguration(PaymentCalculator paymentCalculator)
        {
            vDPPage.FillPaymentConfiguration(paymentCalculator);
        }
        public void SetPricingBreakdown(bool isExpand, bool isModal = false)
        {
            vDPPage.SetPricingBreakdown(isExpand, isModal);
        }
        public PricingBreakdown GetPricingBreakdown(bool isFinanaceFeeEnable = false, bool isDefaultValue = true)
        {
            return vDPPage.GetPricingBreakdown(isFinanaceFeeEnable, isDefaultValue);
        }
        public PaymentConfiguration GetPaymentConfiguation()
        {
            return vDPPage.GetPaymentConfiguation();
        }
        public void ClickOnPayment2Inquire()
        {
            vDPPage.ClickOnPayment2Inquire();
        }
        public PaymentLeadForm GetPaymentModalLeadFormContent()
        {
            return vDPPage.GetPaymentModalLeadFormContent();
        }
        public void SubmitPayment2Inquiry(EmailLeadForm emailLeadForm)
        {
            vDPPage.SubmitPayment2Inquiry(emailLeadForm);
        }
        public PaymentThankYou GetPaymentModalThankYouContent()
        {
            return vDPPage.GetPaymentModalThankYouContent();
        }
        public void ClickOnPaymentModalThankYouFinishBtn()
        {
            vDPPage.ClickOnPaymentModalThankYouFinishBtn();
        }
        public string GetPaymentTabName()
        {
            return vDPPage.GetPaymentTabName();
        }
        public string GetFinanceNotNeededMsg()
        {
            return vDPPage.GetFinanceNotNeededMsg();
        }
        public PaymentBottomPanel GetPaymentBottomPanel()
        {
            return vDPPage.GetPaymentBottomPanel();
        }
        #endregion

        #region Gallery
        internal bool IsGalleryModalOpen()
        {
            return vDPPage.IsGalleryModalOpen();
        }

        public string VerifyYoutubeVideoIsPlaying()
        {
            return vDPPage.VerifyYoutubeVideoIsPlaying();
        }

        public string VerifyCorrectYoutubeIDLinkIsDisplayedOnVDP()
        {
            return vDPPage.VerifyCorrectYoutubeIDLinkIsDisplayedOnVDP();
        }

        public void ClickVideoPlayIconInGallery()
        {
            vDPPage.ClickVideoPlayIconInGallery();
        }
        #endregion

        #region PreQualification
        public void ClickPreQualGetStartedBtn()
        {
            vDPPage.ClickPreQualGetStartedBtn();
        }

        public void FillPersonalInfo(PersonalInformation PersonalInformation)
        {
            vDPPage.FillPersonalInfo(PersonalInformation);
        }

        public bool IsConfirmatoryTitleDisplayed()
        {
            return vDPPage.IsConfirmatoryTitleDisplayed();
        }

        public bool IsPersonalInfoDisplayed()
        {
            return vDPPage.IsPersonalInfoDisplayed();
        }

        public void ClickPreQualNextBtn()
        {
            vDPPage.ClickPreQualNextBtn();
        }

        public void FillAppSummary()
        {
            vDPPage.FillAppSummary();
        }

        public void FillAddressInfo(AddressInformation addressInfo)
        {
            vDPPage.FillAddressInfo(addressInfo);
        }

        public void SelectMonth()
        {
            vDPPage.SelectMonth();
        }

        public void SelectEmploymentStatus()
        {
            vDPPage.SelectEmploymentStatus();
        }

        public void FillIncomeInfo()
        {
            vDPPage.FillIncomeInfo();
        }

        public void FillExpenses(Expenses expenses)
        {
            vDPPage.FillExpenses(expenses);
        }

        public void FillCreditScoreInfo()
        {
            vDPPage.FillCreditScoreInfo();
        }

        #endregion

        #region VdpPriceAlert
        public bool GetPriceAlertBtnStatus(bool isSubscribed = true)
        {
            return vDPPage.GetPriceAlertBtnStatus(isSubscribed);
        }

        public void ClickPriceAlertBtn()
        {
            vDPPage.ClickPriceAlertBtn();
        }

        public void EnterEmailPriceAlert(string email)
        {
            vDPPage.EnterEmailPriceAlert(email);
        }

        public void ClickPriceAlertSubscribeBtn()
        {
            vDPPage.ClickPriceAlertSubscribeBtn();
        }

        public bool IsPriceAlertSuccessDisplayed()
        {
            return vDPPage.IsPriceAlertSuccessDisplayed();
        }

        public void ClosePriceAlertSuccessModal()
        {
            vDPPage.ClosePriceAlertSuccessModal();
        }

        public bool IsRedirectedToCorrectVdpFromMyGarage(string adId)
        {
            return vDPPage.IsRedirectedToCorrectVdpFromMyGarage(adId);
        }

        public void UnsubscribeVdpPriceAlert()
        {
            vDPPage.UnsubscribeVdpPriceAlert();
        }
        #endregion

        #region VdpSaveVehicle
        public void ClickSaveVehicleBtn() => vDPPage.ClickSaveVehicleBtn();

        public string GetToasterMsg() => vDPPage.GetToasterMsg();

        public bool GetSaveVehicleBtnStatus(bool isSaved = true) => vDPPage.GetSaveVehicleBtnStatus(isSaved);
        #endregion

        #region DealerInfo
        public bool IsDealerInfoDisplayed()
        {
            return vDPPage.IsDealerInfoDisplayed();
        }
        public string GetDealerInfoWidgetHeader()
        {
            return vDPPage.GetDealerInfoWidgetHeader();
        }
        public bool IsDealerInfoLogoDisplayed()
        {
            return vDPPage.IsDealerInfoLogoDisplayed();
        }
        public string GetDealerInfoName()
        {
            return vDPPage.GetDealerInfoName();
        }
        public bool IsDealerInfoShowmapButtonDisplayed()
        {
            return vDPPage.IsDealerInfoShowmapButtonDisplayed();
        }
        public string GetDealerAddressOnViewMap()
        {
            return vDPPage.GetDealerAddressOnViewMap();
        }
        public void CloseViewMap()
        {
            vDPPage.CloseViewMap();
        }
        public void ClickOnShowmapButton()
        {
            vDPPage.ClickOnShowmapButton();
        }
        public bool IsDealerInfoViewAllInventoryButtonDisplayed()
        {
            return vDPPage.IsDealerInfoViewAllInventoryButtonDisplayed();
        }
        public void ClickOnViewAllInventoryButton()
        {
            vDPPage.ClickOnViewAllInventoryButton();
        }
        public bool IsDealershipPageMatch(string dealershipURLRoute)
        {
            return vDPPage.IsDealershipPageMatch(dealershipURLRoute);
        }
        public bool IsDealerInfoVisitWebsiteLinkClickable()
        {
            return vDPPage.IsDealerInfoVisitWebsiteLinkClickable();
        }
        public void ClickOnDealerInfoVisitWebsiteLink()
        {
            vDPPage.ClickOnDealerInfoVisitWebsiteLink();
        }
        public bool IsDealerInfoVisitWebsiteLinkUrlMatch(string visitWebsiteUrl)
        {
            return vDPPage.IsDealerInfoVisitWebsiteLinkUrlMatch(visitWebsiteUrl);
        }
        public bool IsDealerInfoSeeReviewsLinkClickable()
        {
            return vDPPage.IsDealerInfoSeeReviewsLinkClickable();
        }
        public bool IsDealerInfoGoogleReviewDisplayed()
        {
            return vDPPage.IsDealerInfoGoogleReviewDisplayed();
        }
        public int GetDealerInfoUSPItemsCount()
        {
            return vDPPage.GetDealerInfoUSPItemsCount();
        }
        public bool IsDealerInfoAwardWinnerLogoDisplayed()
        {
            return vDPPage.IsDealerInfoAwardWinnerLogoDisplayed();
        }
        public bool IsBrandingVideoImageThumbDisplayed()
        {
            return vDPPage.IsBrandingVideoImageThumbDisplayed();
        }
        public bool IsOTLEventImageThumbDisplayed()
        {
            return vDPPage.IsOTLEventImageThumbDisplayed();
        }
        public bool IsDefaultImageThumbDisplayed()
        {
            return vDPPage.IsDefaultImageThumbDisplayed();
        }
        public void ClickOnBrandingVideo()
        {
            vDPPage.ClickOnBrandingVideo();
        }
        public bool IsBandingVideoModalDisplayed()
        {
            return vDPPage.IsBandingVideoModalDisplayed();
        }
        public void CloseBrandingVideo()
        {
            vDPPage.CloseBrandingVideo();
        }
        #endregion

        public string GetVDPTitle()
        {
            return vDPPage.GetVDPTitle();
        }

        public bool IsViewMapDisplayed()
        {
            return vDPPage.IsViewMapDisplayed();
        }

        public void ClickOnViewMapButton()
        {
            vDPPage.ClickOnViewMapButton();
        }

        public bool? VerifyDealerDetailsOnTheLeadForm() => vDPPage.VerifyDealerDetailsOnTheLeadForm();        
       

        public string GetEmailLeadTermsAndConditionMessage() => vDPPage.GetEmailLeadTermsAndConditionMessage();

        public bool? VerifyLeadFormtabs() => vDPPage.VerifyLeadFormtabs();

        public string VerifyPhoneNumberAssitedTextOnTheLeadForm() => vDPPage.VerifyPhoneNumberAssitedTextOnTheLeadForm();        
    }
}