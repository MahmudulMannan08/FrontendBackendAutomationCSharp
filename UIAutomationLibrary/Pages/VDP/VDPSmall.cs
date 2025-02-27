using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MarketPlaceWeb.Pages.VDP
{
    public class VDPSmall : VDPAbstract
    {
        public VDPSmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region Leads
        public override void OpenLeadAbs(EmailLeadForm.EmailLeads emailLeads)
        {
            string tabName = string.Empty;
            switch (emailLeads)
            {
                case EmailLeadForm.EmailLeads.BookAnAppointment:
                    tabName = (language.ToString() == "EN") ? "BOOK AN APPOINTMENT" : "PRENDRE RENDEZ-VOUS";
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description)));                    
                    break;
                case EmailLeadForm.EmailLeads.GeneralInquiry:
                    tabName = (language.ToString() == "EN") ? "GENERAL INQUIRY" : "REQUÊTE GÉNÉRALE";
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description)));                   
                    break;
                case EmailLeadForm.EmailLeads.MakeAnOffer:
                    tabName = (language.ToString() == "EN") ? "MAKE AN OFFER" : "FAIRE UNE OFFRE";
                    WaitForElementVisible(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.EmailLeadContainer.GetAttribute<DescriptionAttribute>().Description)));                    
                    break;
                case EmailLeadForm.EmailLeads.ViewMapLead:
                    ClickElement(FindElement(By.CssSelector(VDPLocators.LargeLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description));
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeBookAnAppointment:
                    IWebElement bookAnAppointCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.BookAnAppointmentCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(bookAnAppointCSBadgeElement);
                    ClickElement(bookAnAppointCSBadgeElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.BookAnAppointment);
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeHomeTestDrive:
                    IWebElement homeTestDriveCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.HomeTestDriveCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(homeTestDriveCSBadgeElement);
                    ClickElement(homeTestDriveCSBadgeElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.BookAnAppointment);
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeDelivery:
                    IWebElement deliveryCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.DeliveryCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(deliveryCSBadgeElement);
                    ClickElement(deliveryCSBadgeElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.GeneralInquiry);
                    break;
            }
        }
        #endregion

        #region ReserveIt
        public override void CheckDepositTerms(bool toBeChecked = true)
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.SmallLocators.DepositTerms.GetAttribute<DescriptionAttribute>().Description + " input"));
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(VDPLocators.SmallLocators.DepositTerms.GetAttribute<DescriptionAttribute>().Description + " span")));
            }
        }
        #endregion

        #region TryBeforeYouBuy
        public override void ClickTenDayMoneyBackGuaranteeText()
        {
            IList<IWebElement> element1 = FindElements(By.CssSelector(VDPLocators.CommonLocators.TenDayMoneyBackGuarantee
                .GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element1[1]);
            ClickElement(element1[1]);
        }
        #endregion

        public override void SetPricingBreakdown(bool isExpand, bool isModal)
        {
            if (!isModal)
            {
                ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.PricingBreakdownPnl.GetAttribute<DescriptionAttribute>().Description)));
            }
            TogglePricingBreakdown(isExpand, isModal);
        }

        public override PaymentLeadForm GetPaymentModalLeadFormContent()
        {
            var paymentLeadForm = new PaymentLeadForm
            {
                PricingBreakdown = new PricingBreakdown
                {
                    DealerPriceLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDealerPriceLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    DealerPrice = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDealerPriceValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    MSRPLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalMSRPLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    MSRP = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalMSRPValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    InterestChargeLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalInterestChargeLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    InterestCharge = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalInterestChargeValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    DownPaymentLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDownPaymentLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    DownPayment = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDownPaymentValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    TradeInLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTradeInLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    TradeIn = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTradeInValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    TotalObligationLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTotalObligationLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    TotalObligation = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTotalObligationValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    YourEstimate = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalYourEstimate.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    YourEstimateFrequency = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalYourEstimateFequencyLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    FinanceTerm = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFinanceTermLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    FederalLuxuryTaxLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalLuxuryTaxLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    FederalLuxuryTax = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalLuxuryTaxValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim()
                },
                NameLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalNameLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                SummaryDisclaimer = FindElement(By.XPath(VDPLocators.CommonLocators.ModalSummaryDisclaimerX.GetAttribute<DescriptionAttribute>().Description)).Text,
                EmailLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalEmailLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                PhoneNumberLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalPhoneNumberLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                GetPriceAlert = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalGetPriceAlert.GetAttribute<DescriptionAttribute>().Description))?.Text,
                SubmitButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalSubmitBtn.GetAttribute<DescriptionAttribute>().Description))?.Text,
                FormDisclaimer = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFormDisclaimer.GetAttribute<DescriptionAttribute>().Description))?.Text
            };
            return paymentLeadForm;
        }

        public override void ClickVideoPlayIconInGallery()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.VideoPlayIcon.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override string VerifyYoutubeVideoIsPlaying()
        {
            IWebElement iframeYoutube = FindElement(By.CssSelector(VDPLocators.CommonLocators.YoutubeIframe.GetAttribute<DescriptionAttribute>().Description));
            driver.SwitchTo().Frame(iframeYoutube);
            string youtubeClassAttrribute = FindElement(By.CssSelector(VDPLocators.CommonLocators.MoviePlayer.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class");
            driver.SwitchTo().DefaultContent();
            return youtubeClassAttrribute;
        }

        public override bool IsViewMapDisplayed()
        {
            return IsElementVisible(By.CssSelector(VDPLocators.SmallLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description));
        }

        public override void ClickOnViewMapButton()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.SmallLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description));
        }
    }
}