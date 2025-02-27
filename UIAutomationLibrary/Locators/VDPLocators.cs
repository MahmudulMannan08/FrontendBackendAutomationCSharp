using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class VDPLocators
    {
        public enum LargeLocators
        {
            #region Leads
            [Description("#mainPhoto")]
            MainImage,
            [Description("#reserve-vehicle-modal div.modal-footer a")]
            CancelReserveButton,
            [Description("#emailLeadContainer button.viewmapButton")]
            ViewMapButton,
            [Description("#emailLeadContainer button.visitWebsiteButton.ng-star-inserted")]
            VisitWebsite,
            [Description("div.virtual-dealerinfo #visitDealerWebsite")]
            VisitDealerWebsite,
            #endregion

            #region ReserveIt
            [Description("#reserve-charge-form vdp-checkbox")]
            DepositTerms,
            #endregion

            #region Spin360
            [Description(".spin-icon-container")]
            Spin360Icon,
            [Description(".spin-container")]
            SpinWidget,
            [Description("div[data-cy='change-view-wrapper']")]
            SpinSeeInsideButton,
            [Description("#close-button-spin-modal")]
            SpinCloseButton,
            [Description(".spin360Container-md")]
            SpinModal,
            [Description(".spin-btn-getstarted")]
            SpinView360Button,
            [Description(".spin-container .spin-text h4")]
            SpinWidgetText,
            #endregion

            #region DealerInfo
            [Description("#vdpDealerInfoWidget #dealerMapToggleTitleAreaBtn:first-of-type button")]
            DealerInfoShowMapBtn,
            [Description("#vdpDealerInfoWidget #dealerInventoryInfoBtn:nth-of-type(2) button")]
            DealerInfoViewAllInventoryBtn,
            [Description("#vdpDealerInfoWidget .best-price-heading-bottom")]
            DealerInfoAwardWinnerLogo
            #endregion
        }

        public enum SmallLocators
        {
            #region Leads
            [Description("#mainPhoto")]
            MainImage,
            [Description(".viewmapButton")]
            ViewMapButton,
            #endregion

            #region ReserveIt
            [Description("#reserve-charge-form vdp-checkbox")]
            DepositTerms,
            #endregion

            #region VdpPriceAlert
            [Description("//div[text()='Price Alerts On' or text()='Alertes de prix activées']/preceding-sibling::div[@class='price-alert-icon']")]
            PriceAlertOn,
            [Description("div[class^='price-alert-icon-container']")]
            PriceAlertOnSM
            #endregion
        }

        public enum XSLocators
        {
            #region Leads
            [Description("#mainPhotoXS")]
            MainImage,
            [Description("a.email-button")]
            EmailButton,
            [Description("#vdp-lead-modal #dealerMapToggleTitleArea")]
            ViewMapButton,
            #endregion

            #region TryBeforeYouBuy
            [Description("vdp-tbyb-contact-dealership button.btn-tbyb")]
            TryBeforeYouBuyButton,
            [Description("#vdp-widgets-footer .btn-tbyb")]
            TryBeforeYouBuyStickyButton,
            [Description("vdp-tbyb-lead-form-contact-dealership #tbybBtn")]
            BuyOnlineButton,
            [Description("#vdp-widgets-footer .email-seller")]
            EmailSellerStickyButton,
            [Description("#vdp-widgets-footer .btn-tbyb")]
            TbybStickyButton,
            [Description("#vdp-widgets-footer .chat-cta")]
            TextStickyButton,
            [Description("#vdp-widgets-footer .call-button")]
            CallStickyButton,
            [Description("#vdp-widgets-footer #contact-option")]
            ContactSellerStickyButton,
            [Description("#tbyb-contact-section .email-button")]
            ContactSellerEmailButton,
            [Description("#tbyb-contact-section .chat-cta")]
            ContactSellerTextButton,
            [Description("#tbyb-contact-section .call-button")]
            ContactSellerCallButton,
            [Description("#vdp-text-connect-modal")]
            VdpTextModal,
            [Description("#vdp-text-connect-modal .title")]
            VdpTextModalTitle,
            [Description("#vdp-lead-modal button.modal__close")]
            VdpLeadModalCloseButton,
            [Description("#vdp-text-connect-modal button.modal__close")]
            VdpTextModalCloseButton,
            [Description("#vdp-widgets-footer .email-button")]
            EmailPermanentStickyButton,
            #endregion

            #region ReserveIt
            [Description("#reserve-charge-form vdp-toggle-switch")]
            DepositTerms,
            #endregion

            #region Gallery
            [Description("//*[@id='pagination']/following-sibling::a[contains(@class,'gallery-arrows gallery-next')]")]
            Nextarrow,
            [Description(".gallery-video-icon")]
            TapToViewIcon,
            #endregion

            #region VdpPriceAlert
            [Description("//div[text()='Price Alerts On' or text()='Alertes de prix activées']/preceding-sibling::div[@class='price-alert-icon']")]
            PriceAlertOn,
            [Description("div[class^='price-alert-icon-container']")]
            PriceAlertOnXS,
            #endregion

            [Description(".dealer-header-action-button-bottom  button#dealerMapToggleTitleAreaBtn")]
            DealerMapViewMapButton
        }

        public enum CommonLocators
        {
            #region Leads
            [Description("#emailLeadContainer")]
            EmailLeadContainer,
            [Description("#emailLeadContainer li span")]
            LeadContainerTab,   
            [Description("#dealerInfoHeader")]
            LeadFormDealerInfoHeader,
            [Description("vdp-lead-tab-group div[class*='terms-and-conditions']")]
            TAndC,
            [Description("#dealerInfoHeader  div.dealerInfo-name")]
            LeadFormDealerInfoHeaderDealerName,
            [Description("#dealerInfoHeader div.dealerInfo-phone")]
            LeadFormDealerInfoHeaderDealerContact,
            [Description("#emailLeadContainer > div.non-virtual-dealerinfo")]
            LeadFormDealerInfoFooterNonVirtual,            
            [DescriptionGeneralLead("#leadName"), DescriptionViewMapLead("#map-modal input[formcontrolname='name']")]
            LeadNameField,
            [DescriptionGeneralLead("#leadEmail"), DescriptionViewMapLead("#map-modal input[formcontrolname='email']"), DescriptionGalleryLead("#gallery-modal #email")]
            LeadEmailField,
            [DescriptionGeneralLead("#leadPhone"), DescriptionViewMapLead("#lead-phone")]
            LeadPhoneNumberField,
            [DescriptionGeneralLead("#leadMessage"), DescriptionViewMapLead("#map-modal textarea[formcontrolname='message']"), DescriptionGalleryLead("#gallery-modal textarea[formcontrolname='message']")]
            LeadMessageField,
            [DescriptionGeneralLead("input#leadPreferredDate"), DescriptionViewMapLead("#map-modal input[formcontrolname='appointmentDate']")]
            LeadPreferredDate,
            [DescriptionGeneralLead("#leadTimeOfDaySelect"), DescriptionViewMapLead("#time-of-day-select")]
            LeadTimeOfDayDropdown,
            [DescriptionGeneralLead("#leadSendButton"), DescriptionViewMapLead("#map-modal .lead-form-cta"), DescriptionGalleryLead("#gallery-modal button[type='submit']")]
            LeadSendButton,
            [DescriptionGeneralLead(".lead-feedback"), DescriptionViewMapLead(".confirmation-message"), DescriptionGalleryLead(".gallery-lead-confirmation")]
            LeadFeedbackMsg,
            [Description("#map-modal textarea[formcontrolname='message']")]
            ViewMapLeadMessageField,
            [Description("#gallery-modal textarea[formcontrolname='message']")]
            GalleryLeadMessageField,
            [Description("input[formcontrolname='offerAmount'].form-control")]
            MyOfferField,
            [Description("vdp-lead-form div.save-search input[type='checkbox']")]
            PriceAlertCheckbox,
            [Description("#leadThankYouHeader")]
            EmailLeadFeedbackMessage1,
            [Description("#leadThankYouLabel")]
            EmailLeadFeedbackMessage2,
            [Description("#leadThanksOkButton")]
            EmailLeadFeedbackOKButton,
            [Description("#bookAnAppointmentBadge")]
            BookAnAppointmentCSBadge,
            [Description("#homeTestDriveBadge")]
            HomeTestDriveCSBadge,
            [Description("#deliveryBadge")]
            DeliveryCSBadge,
            [Description("#vdp-lead-modal")]
            VdpLeadModal,
            [Description("#map-modal")]
            ViewMapModal,
            [Description("#gallery-modal")]
            GalleryModal,
            [Description(".vdp-double-ring")]
            LeadFormProgress,
            [Description("#map-modal #vdpMapClose")]
            ViewMapModalCloseButton,
            [Description("#emailLeadContainer div.virtual-dealerinfo")]
            LeadFormDealerInfoFooterVirtual,
            [Description("#visitDealerWebsite")]
            LeadFormDealerWebsite,
            [Description("#emailLeadContainer div.lead-phone")]
            PhoneAssistiveText,
            [Description("#emailLeadContainer  label[for=\"phone\"]")]
            PhoneNumberLabel,
            [Description("#emailLeadContainer  #leadMessage")]
            LeadMessage,
            [Description("#emailLeadContainer #dealerInfoHeader > div")]
            DealerInforLeadForm,
            #endregion

            #region TryBeforeYouBuy
            [Description("vdp-tbyb-lead-form-contact-dealership button.btn-tbyb")]
            TryBeforeYouBuyButton,
            [Description("vdp-tbyb-lead-form-contact-dealership #tbybBtn")]
            BuyOnlineButton,
            [Description("#vdp-widgets-footer")]
            VdpFooterWidget,
            [Description(".money-back")]
            TenDayMoneyBackGuarantee,
            #endregion

            #region ReserveIt
            [Description(" vdp-reserve-it div")]
            ReserveItVDPWidget,
            [Description("vdp-reserved-widget div.card-cta span")]
            ReserveVehicleButton,
            [Description("#onlineReservationBadge")]
            OnlineReservationBadge,
            [Description("app-reserve-vehicle-modal .open")]
            ReserveVehicleModalOpen,
            [Description("#reserve-buyer-info-form #buyerName")]
            BuyerName,
            [Description("#nameRequired")]
            NameErrorMsg,
            [Description("#reserve-buyer-info-form #phone")]
            BuyerPhone,
            [Description("#phoneRequired")]
            BuyerPhoneErrorMsg,
            [Description("#reserve-vehicle-modal div.modal-footer button.buyer-info-btn")]
            ContinueToPaymentButton,
            [Description("#reserve-vehicle-modal div.modal-footer a")]
            BuyerInfoCancelButton,
            [Description("#reserve-charge-form #email")]
            DepositEmail,
            [Description("#emailRequired")]
            DepositEmailErrorMsg,
            [Description("#reserve-charge-form #postalCode")]
            DepositPostalCode,
            [Description("#postalCodeRequired")]
            DepositPostalCodeErrorMsg,
            [Description("input[name='cardnumber']"), IframeLocator("#cardNumberElement iframe"), JsLocator("#cardNumberElement input")]
            DepositCCNumber,
            [Description("#cardNumberRequired")]
            DepositCCNumberErrorMsg,
            [Description("#reserve-charge-form #name")]
            DepositCCName,
            [Description("input[name='exp-date']"), IframeLocator("#cardExpElement iframe"), JsLocator("#cardExpElement input")]
            DepositCCExpiry,
            [Description("#cardExpiryRequired")]
            DepositCCExpiryErrorMsg,
            [Description("input[name='cvc']"), IframeLocator("#cardCvcElement iframe"), JsLocator("#cardCvcElement input")]
            DepositCVV,
            [Description("#cardCvcRequired")]
            DepositCVVErrorMsg,
            [Description("#termsRequired")]
            DepositTermsErrorMsg,
            [Description("#reserve-vehicle-modal div.modal-footer button.charge-form-btn")]
            DepositReserveVehicleButton,
            [Description("#reserve-vehicle-modal div.modal-footer a.charge-form-back-btn")]
            DepositBackButton,
            [Description("app-reserve-vehicle-modal div.vdp-double-ring-progress")]
            LoadElement,
            [Description("#reserve-vehicle-modal success")]
            DepositSuccessForm,
            [Description("#reserve-vehicle-modal div.modal-footer button.success-msg-btn")]
            DepositFinishButton,
            [Description("#reserve-vehicle-modal div.row.terms a")]
            TermsConditionsLink,
            [Description("#reserve-vehicle-modal div.modal-body p.page-title")]
            ReserveItTCHeader,
            [Description("#reserve-vehicle-modal div.terms-footer button")]
            ReserveItTCBackButton,
            [Description("#wait-list-buyer-info-form #name")]
            WaitListName,
            [Description("#wait-list-buyer-info-form #email")]
            WaitListEmail,
            [Description("#wait-list-buyer-info-form #phone")]
            WaitListPhone,
            [Description("wait-list-form div.modal-footer button.wait-list-btn")]
            WaitListSubmitButton,
            [Description("#wait-list-buyer-info-form a")]
            WaitListPrivacyLink,
            [Description("#reserve-vehicle-modal success div.page-title")]
            JoinWaitListSuccessTitle,
            [Description("#wait-list-buyer-info-form")]
            JoinWaitListBuyerInfoForm,
            #endregion

            #region Payment2VDP
            [Description(".modal.open")]
            ModalDialogOpen,
            [Description("#divFinance .payment-configuration")]
            PaymentConfigurationPnl,
            [Description("#divFinance #paymentFrequencySelect")]
            PaymentFrequencyLst,
            [Description("#divFinance #financeTerms")]
            FinanceTermsLst,
            [Description("#divFinance .cash-and-tradein input[formcontrolname='downPayment']")]
            DownPaymentTxt,
            [Description("#divFinance .cash-and-tradein input[formcontrolname='tradeIn']")]
            TradeInTxt,
            [Description("#divFinance #includeSalesTax")]
            SalesTaxChk,
            [Description("#divFinance .inquire-better-rate .info")]
            InquireInfo,
            [Description("#divFinance .disclaimer")]
            Disclaimer,
            [Description(".tabs ul:first-child .active .nav-link span:first-child")]
            FinanceTab,

            [Description("#divFinance .pricing-breakdown")]
            PricingBreakdownPnl,
            [Description("#divFinance div.pricing-breakdown span img")]
            PricingBreakdownChevronImg,
            [Description("#divFinance #dealerPriceLabel")]
            DealerPriceLbl,
            [Description("#divFinance #dealerPriceValue")]
            DealerPriceValueLbl,
            [Description("#divFinance #msrpLabel")]
            MsrpLbl,
            [Description("#divFinance #msrpValue")]
            MsrpValueLbl,
            [Description("#divFinance #financeFeeLabel")]
            FinanceFeeLbl,
            [Description("#divFinance #financeFeeValue")]
            FinanceFeeValueLbl,
            [Description("#divFinance #interestChargeLabel")]
            InterestChargeLbl,
            [Description("#divFinance #interestChargeValue")]
            InterestChargeValueLbl,
            [Description("#divFinance #salesTaxLabel")]
            SalesTaxesLbl,
            [Description("#divFinance #salesTaxValue")]
            SalesTaxesValueLbl,
            [Description("#divFinance #luxuryTaxLabel")]
            LuxuryTaxLbl,
            [Description("#divFinance #luxuryTaxValue")]
            LuxuryTaxValue,
            [Description("#divFinance .pricing-breakdown #downPaymentLabel")]
            DownPaymentLbl,
            [Description("#divFinance .pricing-breakdown #downPaymentValue")]
            DownPaymentValueLbl,
            [Description("#divFinance .pricing-breakdown #tradeInLabel")]
            TradeInLbl,
            [Description("#divFinance .pricing-breakdown #tradeInValue")]
            TradeInValueLbl,
            [Description("#divFinance #totalLabel")]
            TotalObligationLbl,
            [Description("#divFinance #totalValue")]
            TotalObligationValueLbl,
            [Description("#divFinance #PaymentEstimateLabel")]
            YourEstimateLbl,
            [Description("#divFinance #estimateAmountvalue")]
            YourEstimateValueLbl,
            [Description("#divFinance .financing-not-needed")]
            FinanceNotNeeded,
            [Description("#divFinance .inquire-better-rate")]
            InquireNowPnl,
            [Description("#divFinance #inquireRate")]
            InquireNowBtn,
            #endregion

            #region Payment2Modal
            [Description(".iconCloseModal")]
            ModalCloseBtn,
            [Description("#mpDealerPriceLabel")]
            ModalDealerPriceLbl,
            [Description("#summaryBox  div.card-header span img")]
            ModalPricingBreakdownChevronImg,
            [Description("#mpDealerPriceValue")]
            ModalDealerPriceValueLbl,
            [Description("#mpMsrpLabel")]
            ModalMSRPLbl,
            [Description("#mpMsrpValue")]
            ModalMSRPValueLbl,

            [Description("#mpInterestChargeLabel")]
            ModalInterestChargeLbl,
            [Description("#mpInterestChargeValue")]
            ModalInterestChargeValueLbl,
            [Description("#mpDownPaymentLabel")]
            ModalDownPaymentLbl,
            [Description("#mpDownPaymentValue")]
            ModalDownPaymentValueLbl,
            [Description("#mpTradeInLabel")]
            ModalTradeInLbl,
            [Description("#mpTradeInValue")]
            ModalTradeInValueLbl,
            [Description("#mpLuxuryTaxLabel")]
            ModalLuxuryTaxLbl,
            [Description("#mpLuxuryTaxValue")]
            ModalLuxuryTaxValueLbl,
            [Description("#mpTotalObligationLabel")]
            ModalTotalObligationLbl,
            [Description("#mpTotalObligationValue")]
            ModalTotalObligationValueLbl,
            [Description(".estimation-price")]
            ModalYourEstimate,
            [Description(".payment-freq")]
            ModalYourEstimateFequencyLbl,
            [Description(".finance-term")]
            ModalFinanceTermLbl,
            [Description("//*[@id='paymentsModalFuji']//payments-fuzi-summary/div/div[3]")]
            ModalSummaryDisclaimerX,
            [Description("#leadForm .summary-disclaimer")]
            ModalSummaryDisclaimer,

            [Description("#leadForm #lead-nameLabel")]
            ModalNameLbl,
            [Description("#leadForm #lead-name")]
            ModalNameTxt,
            [Description("#leadForm #lead-emailLabel")]
            ModalEmailLbl,
            [Description("#leadForm #lead-email")]
            ModalEmailTxt,
            [Description("#leadForm #lead-phoneLabel")]
            ModalPhoneNumberLbl,
            [Description("#leadForm #lead-phone")]
            ModalPhoneNumberTxt,
            [Description("#leadForm #lead-message")]
            ModalMessageTxt,
            [Description("#leadForm .save-search-content")]
            ModalGetPriceAlert,
            [Description("#leadForm #sendLead")]
            ModalSubmitBtn,
            [Description("#leadForm .form-disclaimer p")]
            ModalFormDisclaimer,


            [Description("#thanksbox .thanks-box-heading")]
            ModalThankYouHeading,
            [Description(".thanks-box-msg span")]
            ModalThankYouMsg,
            [Description("#finishButton")]
            ModalFinishButton,
            #endregion

            #region Gallery
            [Description(".gallery-thumbnails-Video")]
            VideoPlayIcon,
            [Description("#movie_player")]
            MoviePlayer,
            [Description(".ytp-youtube-button ytp-button yt-uix-sessionlink")]
            YoutubeLink,
            [Description(".gallery - counter - badge")]
            GalleryCounter,
            [Description(".modal - outer - wrapper - md")]
            ModalContainer,
            [Description(".gallery-carousel-wrapper-md")]
            GalleryPhotoStripe,
            [Description(".next-md")]
            NextButton,
            [Description(".previous")]
            PreviousButton,
            [Description(".ytp-play-button ytp-button")]
            PlayPauseButton,
            [Description(".modal__close gallery-modal-close")]
            CloseGalleryModalButton,
            [Description("iframe[id^='youtubeIFrameId_']")]
            YoutubeIframe,
            #endregion

            #region VdpPriceAlert
            [Description("#heroWrapperDescription #priceAlertContainer")]
            PriceAlertBtn,
            [Description("#vdpPriceDropAlert")]
            PriceAlertModal,
            [Description("#heroWrapperDescription .price-modal-overlay")]
            PriceAlertSuccessModal,
            [Description("#vdpPriceDropAlert #txtEmail")]
            PriceAlertEmailTxt,
            [Description("#vdpPriceDropAlert #btnSubscribe")]
            PriceAlertSubscribeBtn,
            [Description("#heroWrapperDescription .price-modal-overlay .price-modal-body-heading")]
            PriceAlertSuccessHeader,
            [Description("#heroWrapperDescription .price-modal-overlay .close-button")]
            PriceAlertSuccessModalCloseBtn,

            [Description("#heroWrapperDescription .price-alert-icon")]
            PriceAlertBellIcon,
            [Description("#heroWrapperDescription div[class^='price-alert-label']")]
            PriceAlertLabel,
            [Description("//div[text()='Get Price Alerts' or text()='Obtenir alertes de prix']")]
            GetPriceAlert,
            [Description("h1.price-drop-heading")]
            SubscribeToPriceDropALert,
            [Description("div#dvPriceDropAlert")]
            PriceAlertOverlay,
            [Description("img.email-icon")]
            EmailIconOnPriceAlertOverlay,
            [Description(".close-button")]
            ClosePriceAlertOverlayButton,
            [Description("//span[text()='Unsubscribed' or text()='Désabonné']")]
            UnsubscribedMessageBar,
            [Description("//span[text()='Subscribed' or text()='Abonné']")]
            SubscribedMessageBar,
            [Description("p.price-modal-body-text")]
            PriceAlertInboxConfirmation,
            #endregion

            #region VdpSaveVehicle
            [Description("#heroWrapperDescription #saveVehicleContainer")]
            SaveVehicleBtn,
            [Description("simple-snack-bar span")]
            ToasterMsg,
            #endregion

            #region PreQualification
            [Description(".prequal-button button")]
            PreQualGetStartedButton,
            [Description("input[formcontrolname='firstName']"), IframeLocator("#inovatecFrame")]
            PersonalInfoFirstName,
            [Description("input[formcontrolname='lastName']"), IframeLocator("#inovatecFrame")]
            PersonalInfoLastName,
            [Description("input[formcontrolname='email']"), IframeLocator("#inovatecFrame")]
            PersonalInfoEmail,
            [Description("input[formcontrolname='phoneNumber']"), IframeLocator("#inovatecFrame")]
            PersonalInfoPhoneNumber,
            [Description("input[type='button']"), IframeLocator("#inovatecFrame")]
            PreQualNextButton,
            [Description("input[formcontrolname='addressLine1']"), IframeLocator("#inovatecFrame")]
            AddressLine1,
            [Description("input[formcontrolname='addressLine2']"), IframeLocator("#inovatecFrame")]
            AddressLine2,
            [Description("input[formcontrolname='city']"), IframeLocator("#inovatecFrame")]
            City,
            [Description("#mat-select-0"), IframeLocator("#inovatecFrame")]
            Province,
            [Description("#mat-option-0"), IframeLocator("#inovatecFrame")]
            ProvinceOption,
            [Description("input[formcontrolname='postalCode'"), IframeLocator("#inovatecFrame")]
            PostalCode,
            [Description("div[class*='col-6 col-lg-3 select-buttons ng-star-inserted']"), IframeLocator("#inovatecFrame")]
            Month,
            [Description("div[class*='col-12 col-md-6 select-buttons ng-star-inserted']"), IframeLocator("#inovatecFrame")]
            EmploymentStatus,
            [Description("input[formcontrolname='grossIncome']"), IframeLocator("#inovatecFrame")]
            GrossIncome,
            [Description("div[class*='col-6 col-lg-3 select-buttons ng-star-inserted']"), IframeLocator("#inovatecFrame")]
            IncomeFrequency,
            [Description("input[formcontrolname='housingMonthlyPayments']"), IframeLocator("#inovatecFrame")]
            HousingMonthlyPayments,
            [Description("input[formcontrolname='vehicleMonthlyPayments']"), IframeLocator("#inovatecFrame")]
            VehicleMonthlyPayments,
            [Description("div[class='col-12 col-md-6 select-buttons ng-star-inserted']"), IframeLocator("#inovatecFrame")]
            CreditScore,
            [Description("div.mat-form-field-suffix.ng-tns-c67-13.ng-star-inserted"), IframeLocator("#inovatecFrame")]
            DateofBirthBtn,
            [Description("div[class='mat-calendar-body-cell-content mat-focus-indicator']"), IframeLocator("#inovatecFrame")]
            DOBDate,
            [Description("#mat-checkbox-1"), IframeLocator("#inovatecFrame")]
            AppSummaryCkBox,
            [Description(".title-section_heading h1"), IframeLocator("#inovatecFrame")]
            PreQualConfirmatoryTitle,
            [Description("application-description .description"), IframeLocator("#inovatecFrame")]
            PersonalInfoTxt,
            #endregion
          
            #region DealerInfo
            [Description("#vdpDealerInfoWidget")]
            DealerInfoWidget,
            [Description("#vdpDealerInfoWidget .dealer-header-title")]
            DealerInfoHeader,
            [Description("#vdpDealerInfoWidget #dealerLogoImg")]
            DealerInfoLogo,
            [Description("#vdpDealerInfoWidget .dealer-name")]
            DealerInfoName,
            [Description("#vdpDealerInfoWidget #websiteTextLink")]
            DealerInfoVisitWebsiteLink,
            [Description("#vdpDealerInfoWidget .see-reviews")]
            DealeInfoSeeReviewLink,
            [Description("#vdpDealerInfoWidget #dealerGoogleRating")]
            DealeInfoGoogleRating,
            [Description("#vdpDealerInfoWidget .bullet-container-item")]
            DealeInfoUSPItems,
            [Description("#vdpDealerInfoWidget .bullet-priceaward-container")]
            DealeInfoUSPContainer,
            [Description("#vdpDealerInfoWidget .small-screen-best-dealer-container")]
            DealerInfoAwardWinnerLogo,
            [Description("#vdpDealerInfoWidget #brandVideoThumbImg")] 
            BrandingVideoThumb,
            [Description("#vdpDealerInfoWidget #defaultImg")]
            DefaultImageThumb,
            [Description("#vdpDealerInfoWidget #oltEventImg")]
            OltEventImgThumb,
            [Description("#dealer-info-video-model")]
            BrandingVideoModal,
            [Description("#video-model-close")]
            BrandingVideoCloseBtn,
            [Description(".dealer-header-action-button-bottom #dealerMapToggleTitleAreaBottomBtn button")]
            DealerInfoShowMapBtn,
            [Description(".dealer-header-action-button-bottom #dealerInventoryInfoBottomBtn button")]
            DealerInfoViewAllInventoryBtn,
            #endregion

            [Description("#heroWrapperDescription #heroTitleWrapper .hero-title")]
            HeroCardTitle,
            [Description("#btnLearnMore")]
            LearnMoreButton,
            [Description("#lnkLearnMore")]
            LearnMoreButtonModal,
            [Description(".overview-title")]
            ModalFlyoutTitle,
            [Description(".banner-title"), DescriptionIOS(".overview-title")]
            BuyersHubTitle,
            [Description(".dealer-contact-info .dealer-address p")]
            ViewMapDealerAddress,

            #region Dealer Info - MotoInsgight - PaymentCalculator
            [Description("#vdpDrLitePaymentCalculator")]
            PaymentCalcWidget,
            [Description("#vdpDrLitePaymentCalculator div.dr-lite-footer > a")]
            PaymentCalcWidgetButton,
            [Description("vdp-drlite-calculator-blur #vdpDrLitePaymentCalculator")]
            PaymentCalcWidgetNewXS,
            [Description("#paymentCalculatorCtaButton > button")]
            PaymentCalcWidgetCTA,
            [Description("#digitalRetailBadge a")]
            PaymentCalcPill,
            [Description("#calulateBtnWrapper > a")]
            PaymentcalcLeadFormCTA,
            [Description("#calculatePaymentsLink")]
            PaymenctCalcCTAXSHero,
            [Description("vdp-ico-widget")]
            ICOWidget
            #endregion                      
        }
    }

    internal class IframeLocatorAttribute : Attribute
    {
        public string IframeLocator { get; set; }

        public IframeLocatorAttribute(string value)
        {
            IframeLocator = value;
        }
    }

    internal class JsLocatorAttribute : Attribute
    {
        public string JsLocator { get; set; }

        public JsLocatorAttribute(string value)
        {
            JsLocator = value;
        }
    }

    internal class DescriptionIOSAttribute : Attribute
    {
        public string DescriptionIOS { get; set; }
        public DescriptionIOSAttribute(string value)
        {
            DescriptionIOS = value;
        }
    }

    internal class DescriptionGeneralLeadAttribute : Attribute
    {
        public string DescriptionGeneralLead { get; set; }

        public DescriptionGeneralLeadAttribute(string value)
        {
            DescriptionGeneralLead = value;
        }
    }

    internal class DescriptionViewMapLeadAttribute : Attribute
    {
        public string DescriptionViewMapLead { get; set; }

        public DescriptionViewMapLeadAttribute(string value)
        {
            DescriptionViewMapLead = value;
        }
    }

    internal class DescriptionGalleryLeadAttribute : Attribute
    {
        public string DescriptionGalleryLead { get; set; }

        public DescriptionGalleryLeadAttribute(string value)
        {
            DescriptionGalleryLead = value;
        }
    }
}